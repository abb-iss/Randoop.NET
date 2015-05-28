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
using System.Reflection;
using System.IO;
using System.Collections;
using System.Diagnostics;
using Common;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Randoop
{



    /// <summary>
    /// Keeps the set of types, mehtods and constructors being explored
    /// [random explorer extends this class]
    /// Except methods weights, the contents of explorationContext remains constant
    /// </summary>
    public class ActionSet
    {
        public List<Type> types = new List<Type>();

        //type -> constructor list
        private Dictionary<Type, List<ConstructorInfo>> constructors =
               new Dictionary<Type, List<ConstructorInfo>>();

        //constructors for all the types combined
        private List<ConstructorInfo> constructorsList = new List<ConstructorInfo>();

        //type -> method list
        private Dictionary<Type, List<MethodInfo>> methods =
            new Dictionary<Type, List<MethodInfo>>();

        //methods for all types
        private List<MethodInfo> methodsList = new List<MethodInfo>();

        //set of fields of a type
        private Dictionary<Type, List<FieldInfo>> fields =
            new Dictionary<Type, List<FieldInfo>>();

        //set of all fields
        public List<FieldInfo> fieldList = new List<FieldInfo>();
        //array of methods and constructor
        public MemberInfo[] methodsAndConstructorsArray;

        internal List<MemberInfo> roundRobinList = new List<MemberInfo>();

        //weights for a member (not field)
        private Dictionary<MemberInfo, double> weight = new Dictionary<MemberInfo, double>();
        private Dictionary<MemberInfo, double> logWeight = new Dictionary<MemberInfo, double>();


        public double weightedIntervalLength;
        public double logWeightedIntervalLength;

        public ReadOnlyCollection<MethodInfo> GetMethods()
        {
            List<MethodInfo> retval = new List<MethodInfo>();
            retval.AddRange(methodsList);
            return new ReadOnlyCollection<MethodInfo>(retval);
        }

        public ReadOnlyCollection<ConstructorInfo> GetConstructors()
        {
            List<ConstructorInfo> retval = new List<ConstructorInfo>();
            retval.AddRange(constructorsList);
            return new ReadOnlyCollection<ConstructorInfo>(retval);
        }

        /// <summary>
        /// Timer 
        /// </summary>
        public class RRTimer : ITimer
        {

            ActionSet context;

            public RRTimer(ActionSet context)
            {
                this.context = context;
            }

            public bool TimeToStop()
            {
                if (context.roundRobinList.Count == 0)
                    return true;
                return false;
            }
        }


        #region Return members to explore based on different heuristics

        /// <summary>
        /// Gives the next member in round-robin list
        /// First creates a list member if the list is empty
        /// </summary>
        /// <returns></returns>
        public MemberInfo RandomMethodOrConstructorRoundRobin()
        {
            if (roundRobinList.Count == 0)
            {
                foreach (MethodInfo m in methodsList)
                    roundRobinList.Add(m);
                foreach (ConstructorInfo c in constructorsList)
                    roundRobinList.Add(c);

            }

            int randomIndex = Common.Enviroment.Random.Next(roundRobinList.Count);
            MemberInfo retval = roundRobinList[randomIndex];
            roundRobinList.RemoveAt(randomIndex);
            return retval;
        }

        public MemberInfo RandomMethodOrConstructorUniform()
        {
            return methodsAndConstructorsArray[Common.Enviroment.Random.Next(methodsAndConstructorsArray.Length)];
        }

        public MemberInfo RandomActiveMethodorConstructor(Dictionary<Type, List<MemberInfo>> activeMembers, Type t)
        {
            try
            {
                List<MemberInfo> memberList = activeMembers[t];
                Debug.Assert(memberList.Count != 0, "There has to be at least 1 member in an active class");

                return memberList[Common.Enviroment.Random.Next(memberList.Count)];

            }
            catch (Exception e)
            {
                throw new RandoopBug("The type " + t.ToString() + " is expected to be present in activeMembers");
            }
        }


        public FieldInfo RandomField()
        {
            return fieldList[Common.Enviroment.Random.Next(fieldList.Count)];
        }



        #endregion

        /// <summary>
        /// Populates fields based on input types and filters
        /// Performs dependency analysis on types
        /// </summary>
        /// <param name="allTypes"></param>
        /// <param name="filter"></param>
        public ActionSet(Collection<Type> allTypes, IReflectionFilter filter)
        {        
            types.AddRange(allTypes);

            //print # of classes
            Console.Write("{0} classes.", allTypes.Count);
            foreach (Type t in allTypes)
            {
                //Console.WriteLine("Processing type {0}", t.ToString());

                List<FieldInfo> fieldInfoDictList;
                fields.TryGetValue(t, out fieldInfoDictList);
                if (fieldInfoDictList == null)
                    fields[t] = fieldInfoDictList = new List<FieldInfo>();

                List<ConstructorInfo> constructorDictList;
                constructors.TryGetValue(t, out constructorDictList);
                if (constructorDictList == null)
                    constructors[t] = constructorDictList = new List<ConstructorInfo>();

                List<MethodInfo> methodDictList;
                methods.TryGetValue(t, out methodDictList);
                if (methodDictList == null)
                    methods[t] = methodDictList = new List<MethodInfo>();

                {
                    string message;
                    if (!filter.OkToUse(t, out message))
                    {
                        //Console.WriteLine(message);
                        continue;
                    }
                    //Console.WriteLine(message);
                }

                foreach (FieldInfo fi in t.GetFields())
                {
                    string message;
                    if (!filter.OkToUse(fi.DeclaringType, out message))
                    {
                        //Console.WriteLine("\t" + message);
                        continue;
                    }

                    if (!filter.OkToUse(fi, out message))
                    {
                        //Console.WriteLine("\t" + message);
                        continue;
                    }

                    //Console.WriteLine(message);

                    FieldInfo info = fi;

                    fieldInfoDictList.Add(info);
                    fieldList.Add(info);


                }

                foreach (ConstructorInfo ci in t.GetConstructors())
                {
                    string message;
                    if (!filter.OkToUse(ci.DeclaringType, out message))
                    {
                        //Console.WriteLine("\t" + message);
                        continue;
                    }

                    if (!filter.OkToUse(ci, out message))
                    {
                        //Console.WriteLine("\t" + message);
                        continue;
                    }

                    //Console.WriteLine(message);

                    ConstructorInfo info = ci;

                    constructorDictList.Add(info);

                    constructorsList.Add(info);
                }

                foreach (MethodInfo mi in t.GetMethods())
                {
                    MethodInfo info = mi;

                    string message;
                    if (!filter.OkToUse(mi.DeclaringType, out message))
                    {
                        //Console.WriteLine("\t" + message);
                        continue;
                    }

                    if (!filter.OkToUse(mi, out message))
                    {
                        //Console.WriteLine("\t" + message);
                        continue;
                    }

                    //Console.WriteLine(message);

                    methodDictList.Add(info);

                    methodsList.Add(info);
                }
            }

            Collection<MemberInfo> methodsAndConstructorsAsList = new Collection<MemberInfo>();
            foreach (Type t in types)
            {
                foreach (MethodInfo m in this.methods[t])
                    methodsAndConstructorsAsList.Add(m);
                foreach (ConstructorInfo c in this.constructors[t])
                    methodsAndConstructorsAsList.Add(c);
            }
            methodsAndConstructorsArray = new MemberInfo[methodsAndConstructorsAsList.Count];
            for (int i = 0; i < methodsAndConstructorsAsList.Count; i++)
                methodsAndConstructorsArray[i] = methodsAndConstructorsAsList[i];

            foreach (MethodInfo m in methodsList)
                roundRobinList.Add(m);
            foreach (ConstructorInfo c in constructorsList)
                roundRobinList.Add(c);

            if (methodsAndConstructorsArray.Length == 0)
            {
                throw new EmpytActionSetException();
            }

            Console.Write(" {0} members.", methodsAndConstructorsArray.Length);
        }

    }

    [Serializable]
    public class EmpytActionSetException : Exception
    {
    }

}
