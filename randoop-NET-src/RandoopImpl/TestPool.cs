//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using Common;
using System.IO;

namespace Randoop
{
    /// <summary>
    /// A collection of plans.
    /// For a given type, the database always returns at least one plan
    /// (the default plan, which is 0 or null).
    /// </summary>
    public class PlanDataBase
    {
        // The name of the database.
        private string name;

        public TypeMatchingMode typeMatchingMode;

        // Plans are indexed by the types they can yield.
        private Dictionary<Type, Collection<Plan>> plansByType = new Dictionary<Type, Collection<Plan>>();

        private TypeMap typeMap = new TypeMap();

        // We also maintain a set for quick lookup of plan membership.
        public Dictionary<Plan, bool> planSet = new Dictionary<Plan, bool>();

        public int NumPlans = 0;

        public int totalFailedAdditions = 0;
        public int totalAttemptedAdditions = 0;
        public bool allowPlanRepeat = false;

        public PlanDataBase(string name, TypeMatchingMode typeMatchingMode)
        {
            this.name = name;
            this.typeMatchingMode = typeMatchingMode;
        }

        public PlanDataBase(string name, TypeMatchingMode typeMatchingMode, bool allowPlanRepeat)
        {
            this.allowPlanRepeat = allowPlanRepeat;
            this.name = name;
            this.typeMatchingMode = typeMatchingMode;
        }

        public void AddEnumConstantsToPlanDB(Collection<Type> types)
        {
            foreach (Type t in types)
            {
                if (t.IsEnum)
                {
                    Array array = Enum.GetValues(t);
                    for (int i = 0; i < array.Length; i++)
                        AddPlan(Plan.Constant(t, array.GetValue(i)));
                }
            }
        }
            
        internal void AddPlan(Plan p, ResultTuple resultTuple)
        {
            if (p == null) throw new ArgumentNullException();
            if (resultTuple == null) throw new ArgumentNullException();

            AddplanInternal(p, resultTuple);
        }

        public void AddPlan(Plan p)
        {
            if (p == null) throw new ArgumentNullException();

            AddplanInternal(p, null);
        }

        // resultTuple can be null.
        private bool AddplanInternal(Plan p, ResultTuple resultTuple)
        {
            totalAttemptedAdditions++;
            bool addSuccessful = false;

            if (planSet.ContainsKey(p))
            {
                totalFailedAdditions++;

                return false;
            }
            else
            {
                planSet[p] = true;
                NumPlans++;

                Dictionary<Type, bool> processedTypes = new Dictionary<Type, bool>();

                for (int i = 0; i < p.transformer.TupleTypes.Length; i++)
                {
                    if (!p.IsActiveTupleElement(i))
                    {
                        continue;
                    }

                    Type t = p.transformer.TupleTypes[i];

                    Collection<Plan> l = null;
                    if (plansByType.ContainsKey(t))
                    {
                        l = plansByType[t];
                    }
                    else
                    {
                        l = new Collection<Plan>();

                        // Always add 0 to the list of plans for a type.
                        bool success;
                        Plan pl = DefaultPlan(t, out success);
                        if (success)
                        {
                            l.Add(pl);
                            typeMap.AddTypeWithPlans(t);
                        }
                        plansByType[t] = l;
                    }

                    //we don't allow chars because the it can create unreadable chars
                    if (/*t.IsValueType && */t.IsPrimitive && !t.ToString().StartsWith("System.Char"))
                    {
                        if (p.transformer is PrimitiveValueTransformer)
                        {
                            l.Add(p);
                            typeMap.AddTypeWithPlans(t);
                        }
                        else
                        {
                            if (resultTuple != null)
                            {
                                object pValue = resultTuple.tuple[i];
                                Util.Assert(pValue != null && pValue.GetType().Equals(t));

                                bool alreadyInDB = false;
                                foreach (Plan existingPlan in l)
                                {
                                    Util.Assert(existingPlan.transformer is PrimitiveValueTransformer);
                                    object existingPlanValue = (existingPlan.transformer as PrimitiveValueTransformer).fvalue;
                                    Util.Assert(existingPlanValue != null
                                            && existingPlanValue.GetType().Equals(pValue.GetType()));
                                    if (pValue.Equals(existingPlanValue))
                                        alreadyInDB = true;
                                }
                                if (!alreadyInDB)
                                {
                                    Plan constantPlan = Plan.Constant(t, pValue);
                                    l.Add(constantPlan);
                                    typeMap.AddTypeWithPlans(t);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (processedTypes.ContainsKey(t))
                            continue;

                        processedTypes[t] = true;
                        bool success;
                        Plan pl = DefaultPlan(t, out success);
                        // Util.Assert(l.Contains(p) && success ? p.Equals(pl) : true);
                        typeMap.AddTypeWithPlans(t);
                        l.Add(p);
                    }
                }

                addSuccessful = true;
            }

            return addSuccessful;
        }

        /// <summary>
        /// 0 for primitive types, null for reference types.
        /// for enum types/value types, the first member is returned
        /// 
        /// The returned plan is valid if success is true
        /// </summary>
        public static Plan DefaultPlan(Type t, out bool success)
        {
            success = true;

            if (t.Equals(typeof(void)))
                return Plan.DummyVoidPlan;


            if (t.IsEnum)
            {
                Array array = Enum.GetValues(t);
                return Plan.Constant(t, array.GetValue(0) as ValueType);
            }
            //special care for chars as the default value is ^@ unreadable in the output
            if (t.Name.Equals("Char"))
            {
                return Plan.Constant(t, '0');
            }
            if (t.IsValueType)
            {

                try
                {
                    Array array = Array.CreateInstance(t, 1);
                    return Plan.Constant(t, array.GetValue(0) as ValueType);
                }
                catch (Exception)
                {
                    success = false;
                    //the plan does not even make sense
                    return Plan.DummyVoidPlan;
                }
            }
            else if (t.IsPointer)
            {
                // Cannot handle pointers.
                success = false;
                return Plan.DummyVoidPlan;
            }
            else
            {
                return Plan.Constant(t, null);
            }
        }

        internal Plan RandomPlan(Type typeDesired)
        {
            CollectionOfCollections<Plan> plans = GetPlans(typeDesired);

            // TODO GetPlans should do null or zero-sized list, and you
            // should only have to check for one.
            if (plans == null || plans.Size() == 0)
                return null;

            int randnum = Common.Enviroment.Random.Next(plans.Size()); //xiao.qu@us.abb.com adds for debug
            return plans.Get(randnum);
        }

        /// <summary>
        /// Returns a list of plans of type t in the database.
        /// New: The list returned can be empty if its a valuetype and a default value can't be created
        /// </summary>
        /// <param name="t">The type of plan desired.</param>
        /// <returns></returns>
        private CollectionOfCollections<Plan> GetPlans(Type t)
        {
            if (!plansByType.ContainsKey(t))
            {
                bool success;
                Plan pl = DefaultPlan(t, out success);
                if (success)
                {
                    AddPlan(pl);
                }
                else
                    return null;
            }

            Util.Assert(plansByType[t] != null);

            if (t.IsInterface)
                return GetPlansInterface(t);

            CollectionOfCollections<Plan> retval = new CollectionOfCollections<Plan>();
            retval.Add(plansByType[t]);

            if (typeMatchingMode == TypeMatchingMode.SubTypes)
            {
                foreach (Type dbType in typeMap.TypesWithPlans(t).Keys)
                {
                    Util.Assert(ReflectionUtils.IsSubclassOrEqual(dbType, t));
                    retval.Add(plansByType[dbType]);
                }
            }

            return retval;
        }

        public CollectionOfCollections<Plan> GetPlansInterface(Type t)
        {
            if (!t.IsInterface)
                throw new ArgumentException("must be an interface.");

            CollectionOfCollections<Plan> retval = new CollectionOfCollections<Plan>();
            foreach (Type dbType in plansByType.Keys)
            {
                if (dbType.Equals(t))
                    retval.Add(plansByType[dbType]);
                else
                {
                    foreach (Type interfaceType in dbType.GetInterfaces())
                        if (t.Equals(interfaceType))
                        {
                            retval.Add(plansByType[dbType]);
                            break;
                        }
                }
            }
            return retval;
        }

        internal bool Containsplan(Plan v)
        {
            if (planSet.ContainsKey(v))
                return true;
            return false;
        }



        internal ICollection<Type> TypesInDataBase()
        {
            return this.plansByType.Keys;
        }

        /// <summary>
        /// Clears the set of plans in the database
        /// </summary>
        public void Clear()
        {
            plansByType.Clear();
            planSet.Clear();
        }


        // Add some non-zero constants to the mix.
        public void AddConstantsToTDB(RandoopConfiguration config)
        {
            foreach (SimpleTypeValues vs in config.simpleTypeValues)
            {
                Type type = Type.GetType(vs.simpleType);

                if (type == null)
                {
                    throw new Common.RandoopBareExceptions.InternalError("invalid simple type in XML config file.");
                }

                foreach (FileName fn in vs.fileNames)
                {
                    string fileName = fn.fileName;
                    if (!File.Exists(fileName))
                    {
                        throw new Common.RandoopBareExceptions.InvalidUserParamsException("Configuration file does not exist: " + fileName);
                    }

                    if (type.Equals(typeof(sbyte)))
                    {
                        SByteReader r = new SByteReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(sbyte), o));
                    }
                    else if (type.Equals(typeof(byte)))
                    {
                        ByteReader r = new ByteReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(byte), o));
                    }
                    else if (type.Equals(typeof(short)))
                    {
                        ShortReader r = new ShortReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(short), o));
                    }
                    else if (type.Equals(typeof(ushort)))
                    {
                        UshortReader r = new UshortReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(ushort), o));
                    }
                    else if (type.Equals(typeof(int)))
                    {
                        IntReader r = new IntReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(int), o));
                    }
                    else if (type.Equals(typeof(uint)))
                    {
                        UintReader r = new UintReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(uint), o));
                    }
                    else if (type.Equals(typeof(long)))
                    {
                        LongReader r = new LongReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(long), o));
                    }
                    else if (type.Equals(typeof(ulong)))
                    {
                        UlongReader r = new UlongReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(ulong), o));
                    }
                    else if (type.Equals(typeof(char)))
                    {
                        CharReader r = new CharReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(char), o));
                    }
                    else if (type.Equals(typeof(float)))
                    {
                        FloatReader r = new FloatReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(float), o));
                    }
                    else if (type.Equals(typeof(double)))
                    {
                        DoubleReader r = new DoubleReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(double), o));
                    }
                    else if (type.Equals(typeof(bool)))
                    {
                        BoolReader r = new BoolReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(bool), o));
                    }
                    else if (type.Equals(typeof(decimal)))
                    {
                        DecimalReader r = new DecimalReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(decimal), o));
                    }
                    else
                    {
                        if (!type.Equals(typeof(string)))
                        {
                            throw new Common.RandoopBareExceptions.InternalError("invalid simple type in XML config file.");
                        }
                        Common.StringReader r = new Common.StringReader();
                        foreach (object o in r.Read(fileName))
                            this.AddPlan(Plan.Constant(typeof(string), o));
                    }
                }
            }
        }

        public void PrintPrimitives(TextWriter w)
        {
            foreach (Type t in this.plansByType.Keys)
            {
                if (!t.IsPrimitive && !t.Equals(typeof(string)))
                    continue;

                w.WriteLine("========== " + t.ToString());
                int count = 0;
                foreach (Plan p in this.plansByType[t])
                {
                    PrimitiveValueTransformer tr = p.transformer as PrimitiveValueTransformer;
                    Util.Assert(t != null);
                    if (count > 0)
                        w.Write(", ");
                    w.Write(tr.fvalue);
                    count++;
                }
                w.WriteLine();
            }
        }


        private class TypeMap
        {
            private Dictionary<Type, Dictionary<Type, bool>> subTypesWithPlans =
                new Dictionary<Type, Dictionary<Type, bool>>();

            private /* Set */ Dictionary<Type, bool> typesWithPlans = new Dictionary<Type, bool>();

            public void AddTypeWithPlans(Type newType)
            {
                if (newType == null) throw new ArgumentNullException();

                // Type is already in mapping.
                if (typesWithPlans.ContainsKey(newType))
                    return;

                typesWithPlans[newType] = true;

                foreach (Type t in subTypesWithPlans.Keys)
                {
                    if (ReflectionUtils.IsSubclassOrEqual(newType, t))
                    {
                        subTypesWithPlans[t][newType] = true;
                    }
                }
            }

            private void CreateEntry(Type newType)
            {
                Dictionary<Type, bool> set = new Dictionary<Type, bool>();

                foreach (Type t in subTypesWithPlans.Keys)
                {
                    if (ReflectionUtils.IsSubclassOrEqual(t, newType))
                    {
                        foreach (Type t2 in subTypesWithPlans[t].Keys)
                        {
                            set[t2] = true;
                        }
                    }
                }

                subTypesWithPlans[newType] = set;
            }

            public Dictionary<Type, bool> TypesWithPlans(Type t)
            {
                if (t == null) throw new ArgumentNullException();

                Dictionary<Type, bool> result;
                if (!subTypesWithPlans.TryGetValue(t, out result))
                {
                    CreateEntry(t);
                    result = subTypesWithPlans[t];
                }

                return result;
            }
        }
    }
}
