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

//#define PRINT_FAIR_RANDOOP_LOG

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Common;
using System.Collections.ObjectModel;
using System.Security;

namespace Randoop
{



    /// <summary>
    /// An exploring egine that randomly explores the plan space in its exploration context.
    /// </summary>
    public class RandomExplorer //: ExplorationContext
    {

        /// <summary>
        /// Parsed user-supplied arguments.
        /// </summary>
        //UserParams fuserParams;

        public long startTime;

        private IReflectionFilter filter;

        private int arrayMaxSize;

        private StatsManager stats;

        private ActionSet actions;

        #region Data structures for fair Randoop


    
        //set of types that have at least one method active
        Dictionary<Type, bool> activeTypes = new Dictionary<Type, bool>();
        //the set of active methods for each type
        Dictionary<Type, List<MemberInfo>> activeMembers = new Dictionary<Type, List<MemberInfo>>();

        //there are builder plans that generate objects of this type
        Dictionary<Type, bool> typesWithObjects = new Dictionary<Type, bool>();

        //map from each member (method or constructor) to the set of argument types with no objects (dynamic map)
        Dictionary<MemberInfo, List<Type>> member2pendingTypes = new Dictionary<MemberInfo, List<Type>>();
        //maps each type to the set of methods that depend on an object of the type (Static map)
        Dictionary<Type, List<MemberInfo>> type2pendingMembers = new Dictionary<Type, List<MemberInfo>>();

        //temporary hack to redirect the stats dmp to a file, from the std output
        static TextWriter fairOptLog = null; 

        #endregion

        /// <summary>
        /// Initializes all the components of the explorer.
        /// </summary>
        /// <param name="userParams"></param>
        public RandomExplorer(Collection<Type> types, IReflectionFilter filter, bool exceptionLearning,
            int randomSeed, int arrayMaxSize, StatsManager stats, ActionSet actions)
        //: base(types, filter)
        {
            this.actions = actions;
            this.filter = filter;
            this.arrayMaxSize = arrayMaxSize;
            this.stats = stats;
        }


        /// <summary>
        /// Note that Explore can silently fail (exits with 1), if a second chance AV happens 
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="planManager"></param>
        /// <param name="methodWeigthing"></param>
        /// <param name="forbidNull"></param>
        /// <param name="mutateFields"></param>
        public void Explore(ITimer timer, PlanManager planManager, MethodWeighing methodWeigthing,
            bool forbidNull, bool mutateFields, bool fairOpt)
        {
            int currentPlans = planManager.Plans;
            int currentRedundantAdds = planManager.RedundantAdds;

            // Start timing test generation.
            Timer.QueryPerformanceCounter(ref TimeTracking.generationStartTime);


            //exercise the fair option 
            if (fairOpt)
            {
                Console.WriteLine("Using the fair option ....");

                //fairOptLog = new StreamWriter("Randoop.fairopt.log");

                //compute the initial working set and the active methods for each class
                InitializeActiveMemberAndDependency(planManager);

                for (; ; )
                {


                    //field setting
                    if (mutateFields && this.actions.fieldList.Count > 0 && Common.Enviroment.Random.Next(2) == 0)
                        NewFieldSettingPlan(this.actions.RandomField(), planManager, forbidNull);

                    //If the active set is recomputed from scratch
                    //InitializeActiveMemberAndDependency(planManager);


                    //copy the keys 
                    Dictionary<Type, bool>.KeyCollection keys = new Dictionary<Type, bool>.KeyCollection(activeTypes);
                    List<Type> activeList = new List<Type>();
                    foreach (Type t in keys) activeList.Add(t);

                    RandoopPrintFairStats("Randoop.RandomExplore::Explore", "Starting a round of the fair workset algorithm with " + activeTypes.Count + "activeTypes");

                    foreach (Type t in activeList)
                    {
                        //pick up an active method of the type

                        MemberInfo m;

                        try
                        {
                            m = this.actions.RandomActiveMethodorConstructor(activeMembers, t);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception raised from RandomActiveMethodorConstructor is {0}", e.ToString());
                            continue;
                        }

                        //Console.WriteLine("Picked up method {0}::{1}", m.DeclaringType,m.Name );
                        int prevBuilderPlanCnt = planManager.builderPlans.NumPlans;

                        //Randoop step
                        ExploreMemberInternal(timer, planManager, forbidNull, m);

                        //need to know if the object creation was succesful, since we know the type
                        //we do it indirectly by looking at the builder plans
                        if (planManager.builderPlans.NumPlans > prevBuilderPlanCnt)
                        {
                            Type retType;
                            if (m is MethodInfo)
                            {
                                retType = (m as MethodInfo).ReturnType;
                            }
                            else if (m is ConstructorInfo)
                            {
                                retType = (m as ConstructorInfo).DeclaringType;
                            }
                            else
                            {
                                throw new RandoopBug("Constructor or Method expected");
                            }

                            //use the return type of the method/constructor to incrementally activate more methods/classes
                            try
                            {
                                UpdateActiveMethodsAndClasses(retType);
                            }
                            catch (Exception e) // had a nasty bug in the updateActiveMethodsAndClasses, so using try-catch
                            {
                                Console.WriteLine("Exception raised from UpdateActiveMethodsAndClasses is {0}", e.ToString());
                            }
                        }

                        if (timer.TimeToStop())
                        {
                            //get the size of # of active members
                            int size = 0;
                            foreach (KeyValuePair<Type, List<MemberInfo>> kv in activeMembers)
                            {
                                List<MemberInfo> l = kv.Value;
                                size = size + l.Count;
                            }

                            //stats printing 
                            Console.WriteLine("Finishing the fair exploration...printing activity stats");
                            Console.WriteLine("Stats: <#ActiveTypes, #ActiveMembers, #BuilderPlans, #typesWithObjs> = <{0},{1},{2},{3}>",
                                activeTypes.Count, size, planManager.builderPlans.NumPlans, typesWithObjects.Count);
                            //fairOptLog.Close();

                            return;
                        }
                    }
                }
            }
            else
            {
                #region Old code for the Randoop exploration
                for (; ; )
                {


                    //field setting
                    if (mutateFields && this.actions.fieldList.Count > 0 && Common.Enviroment.Random.Next(2) == 0)
                        NewFieldSettingPlan(this.actions.RandomField(), planManager, forbidNull);

                    MemberInfo m;
                    if (methodWeigthing == MethodWeighing.RoundRobin)
                    {
                        m = this.actions.RandomMethodOrConstructorRoundRobin();
                    }
                    else if (methodWeigthing == MethodWeighing.Uniform)
                    {
                        m = this.actions.RandomMethodOrConstructorUniform();
                    }
                    else
                        throw new RandoopBug("Unrecognized method weighting option.");


                    ExploreMemberInternal(timer, planManager, forbidNull, m);

                    if (timer.TimeToStop())
                    {
                        // this computation gives us the activity stats for the non-fair option
                        InitializeActiveMemberAndDependency(planManager);
                        //get the size of # of active members
                        int size = 0;
                        foreach (KeyValuePair<Type, List<MemberInfo>> kv in activeMembers)
                        {
                            List<MemberInfo> l = kv.Value;
                            size = size + l.Count;
                        }

                        //stats printing 
                        Console.WriteLine("Finishing the non-fair exploration...printing activity stats");
                        Console.WriteLine("Stats: <#ActiveTypes, #ActiveMembers, #BuilderPlans, #typesWithObjs> = <{0},{1},{2},{3}>",
                            activeTypes.Count, size, planManager.builderPlans.NumPlans, typesWithObjects.Count);
                        //fairOptLog.Close();

                        return;
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Incrementally updates the pending set of types each method requires to be active
        /// Updates the set of active types and active members
        /// </summary>
        /// <param name="retType"></param>
        private void UpdateActiveMethodsAndClasses(Type retType)
        {
            //check if the type had ways to already create objects
            if (typesWithObjects.ContainsKey(retType)) return;


            typesWithObjects[retType] = true;

            List<MemberInfo> pendingMembers;
            type2pendingMembers.TryGetValue(retType, out pendingMembers);

            if (pendingMembers == null) return; // no one cares about this type

            RandoopPrintFairStats("UpdateActiveMethodsAndClasses", "Come to updateActiveMembers for <" +  retType + ">" + "with" + pendingMembers.Count + " size depMembers");


            foreach (MemberInfo mi in pendingMembers)
            {
                //remove retType from the list of pending types for this member
                List<Type> depTypes;
                member2pendingTypes.TryGetValue(mi, out depTypes);

                //Debug.Assert(depTypes != null);
                //Debug.Assert(depTypes.Contains(retType), "Invariant broken on the type2DependantMembers and memberPendingArgTypes");

                int size_prev = depTypes.Count;
                depTypes.Remove(retType);

                if (depTypes.Count == 0)
                {

                    //make the member and its declaring class active
                    if (!activeTypes.ContainsKey(mi.DeclaringType))
                    {
                        RandoopPrintFairStats("UpdateActiveMethodsAndClasses", "Adding to ActiveTypes:: with member <" + mi + ">  and type " + mi.DeclaringType);
                        activeTypes[mi.DeclaringType] = true;
                    }

                    List<MemberInfo> activeList;
                    activeMembers.TryGetValue(mi.DeclaringType, out activeList);

                    //Debug.Assert(!activeList.Contains(mi), "The member with a pending argument type can't already be active");
                    if (activeList == null)
                    {
                        activeList = new List<MemberInfo>();
                        activeList.Add(mi);
                    }
                    else
                    {
                        activeList.Add(mi);
                    }
                    activeMembers[mi.DeclaringType] = activeList;

                    //take it out of the memberPendingArgTypes
                    member2pendingTypes.Remove(mi);
                }
                else
                {
                    //do we really need this?
                    member2pendingTypes[mi] = depTypes;
                }

            }

            return;
        }

        /// <summary>
        /// Initializes the data structures for Active members, types, and the dependency of methods on types
        /// </summary>
        /// <param name="planManager"></param>
        /// 
        /// 
        private void InitializeActiveMemberAndDependency(PlanManager planManager)
        {

            //reset all the maps
            activeMembers.Clear();
            activeTypes.Clear();
            typesWithObjects.Clear();
            member2pendingTypes.Clear();
            type2pendingMembers.Clear();


            //get the set of types for which the builderPlans can create an object
            Dictionary<Plan, bool>.KeyCollection plans = planManager.builderPlans.planSet.Keys;
            //we simply look at the return type of the last method of each builder plan. This is sufficient
            //because any intermediate method in a plan will appear as the last method of some prefix plan 
            //in the builder plan database
            foreach (Plan plan in plans)
            {
                Transformer tr = plan.transformer;
                Type t;


                if (tr is MethodCall)
                {
                    t = (tr as MethodCall).method.ReturnType;
                    typesWithObjects[t] = true;
                    RandoopPrintFairStats("InitializeActiveMemberDependency", "[Method]" + t.ToString() + " class is ACTIVE");
                }
                else if (tr is ConstructorCallTransformer)
                {
                    t = (tr as ConstructorCallTransformer).fconstructor.DeclaringType;
                    typesWithObjects[t] = true;
                    RandoopPrintFairStats("InitializeActiveMemberDependency", "[Constructor]" + t.ToString() + " class is ACTIVE");

                }
                else if (tr is PrimitiveValueTransformer) //for primitive values and string
                {
                    t = (tr as PrimitiveValueTransformer).ftype;
                    typesWithObjects[t] = true;
                    RandoopPrintFairStats("InitializeActiveMemberDependency", "[Constant]" + t.ToString() + " class is ACTIVE");
                }
                else
                {
                    //TODO: ignoring field setters and Array constructors
                }

            }

            //we are only going to try to activate the non-filtered types/methods only
            foreach (ConstructorInfo ci in actions.GetConstructors())
            {
                bool activeMember = true;
                ParameterInfo[] args = ci.GetParameters();
                //check if all the arguments can be provided an object
                for (int i = 0; i < args.Length; ++i)
                {
                    ParameterInfo pi = args[i];
                    if (!typesWithObjects.ContainsKey(pi.ParameterType))
                    {
                        RandoopPrintFairStats("InitializeActiveMemberAndDependency", "Can't find argument of type" + pi.ParameterType + "for constructor <" + ci.ToString() + ">");
                        activeMember = false;

                        //compute the dependency of method --> pending types
                        List<Type> output;
                        member2pendingTypes.TryGetValue(ci, out output);
                        if (output == null)
                        {
                            List<Type> l = new List<Type>();
                            l.Add(pi.ParameterType);
                            member2pendingTypes[ci] = l;
                        }
                        else if (!output.Contains(pi.ParameterType))
                        {
                            member2pendingTypes[ci].Add(pi.ParameterType);
                        }


                        //compute the dependency of type --> methods
                        List<MemberInfo> members;
                        type2pendingMembers.TryGetValue(pi.ParameterType, out members);
                        if (members == null)
                        {
                            List<MemberInfo> l = new List<MemberInfo>();
                            l.Add(ci);
                            type2pendingMembers[pi.ParameterType] = l;
                        }
                        else if (!members.Contains(ci))
                        {
                            type2pendingMembers[pi.ParameterType].Add(ci);
                        }


                    }
                }

                if (activeMember) // add it to the active member set and update active types
                {
                    Type ti = ci.DeclaringType;

                    List<MemberInfo> memberList;
                    activeMembers.TryGetValue(ti, out memberList);
                    if (memberList == null)
                    {
                        memberList = new List<MemberInfo>();
                        memberList.Add(ci);
                        activeMembers[ti] = memberList;
                    }
                    else
                    {
                        //there can't be any repeats                        
                        activeMembers[ti].Add(ci);
                    }

                    //activate the type
                    activeTypes[ti] = true;
                    RandoopPrintFairStats("InitializeActiveMemberAndDependency", "Adding <ActiveType, ActiveMember>: <" + ti.ToString() + "," + ci.ToString() + ">");

                }
            }

            foreach (MethodInfo mi in actions.GetMethods())
            {
                bool activeMember = true;

                //receiver is treated separately
                if (!typesWithObjects.ContainsKey(mi.DeclaringType))
                {

                    Type ti = mi.DeclaringType;

                    RandoopPrintFairStats("InitializeActiveMemberAndDependency", "Can't find argument of type" + ti + "for method <" + mi.ToString() + ">");

                    activeMember = false;

                    //compute the dependency of method --> pending types
                    List<Type> output;
                    member2pendingTypes.TryGetValue(mi, out output);
                    if (output == null)
                    {
                        List<Type> l = new List<Type>();
                        l.Add(ti);
                        member2pendingTypes[mi] = l;
                    }
                    else if (!output.Contains(ti))
                    {
                        member2pendingTypes[mi].Add(ti);
                    }


                    //compute the dependency of type --> methods
                    List<MemberInfo> memberList;
                    type2pendingMembers.TryGetValue(ti, out memberList);
                    if (memberList == null)
                    {
                        List<MemberInfo> l = new List<MemberInfo>();
                        l.Add(mi);
                        type2pendingMembers[ti] = l;
                    }
                    else if (!memberList.Contains(mi))
                    {
                        type2pendingMembers[ti].Add(mi);
                    }


                }

                //check if all the arguments can be provided an object
                ParameterInfo[] args = mi.GetParameters();
                for (int i = 0; i < args.Length; ++i)
                {
                    ParameterInfo pi = args[i];
                    if (!typesWithObjects.ContainsKey(pi.ParameterType))
                    {
                        RandoopPrintFairStats("InitializeActiveMemberAndDependency", "Can't find argument of type" + pi.ParameterType + "for method <" + mi.ToString() + ">");
                        activeMember = false;

                        //compute the dependency of method --> pending types
                        List<Type> output;
                        member2pendingTypes.TryGetValue(mi, out output);
                        if (output == null)
                        {
                            List<Type> l = new List<Type>();
                            l.Add(pi.ParameterType);
                            member2pendingTypes[mi] = l;
                        }
                        else if (!output.Contains(pi.ParameterType))
                        {
                            member2pendingTypes[mi].Add(pi.ParameterType);
                        }


                        //compute the dependency of type --> methods
                        List<MemberInfo> members;
                        type2pendingMembers.TryGetValue(pi.ParameterType, out members);
                        if (members == null)
                        {
                            List<MemberInfo> l = new List<MemberInfo>();
                            l.Add(mi);
                            type2pendingMembers[pi.ParameterType] = l;
                        }
                        else if (!members.Contains(mi))
                        {
                            type2pendingMembers[pi.ParameterType].Add(mi);
                        }


                    }
                }

                if (activeMember)
                {
                    Type ti = mi.DeclaringType;

                    List<MemberInfo> memberList;
                    activeMembers.TryGetValue(ti, out memberList);
                    if (memberList == null)
                    {
                        memberList = new List<MemberInfo>();
                        memberList.Add(mi);
                        activeMembers[ti] = memberList;
                    }
                    else
                    {
                        //there can't be any repeats                        
                        activeMembers[ti].Add(mi);
                    }

                    //activate the type
                    activeTypes[ti] = true;
                    RandoopPrintFairStats("InitializeActiveMemberAndDependency", "Adding <ActiveType, ActiveMember>: <" + ti.ToString() + "," + mi.ToString() + ">");

                }

            }
        }

        private static void RandoopPrintFairStats(string method, string s)
        {
#if PRINT_FAIR_RANDOOP_LOG
            Console.WriteLine("Method: {0} :: {1}", method, s);
#endif
            //fairOptLog.WriteLine("Method: {0} :: {1}", method, s);
            //fairOptLog.Flush();
        }

        private void ExploreMemberInternal(ITimer timer, PlanManager planManager, bool forbidNull,
            MemberInfo m)
        {
            Type[] sugg = GetParameterTypeSuggestions(m, planManager);

            if (m is ConstructorInfo)
            {
                NewConstructorPlan((m as ConstructorInfo).DeclaringType, m as ConstructorInfo, planManager,
                    forbidNull, sugg);
            }
            else
            {
                Util.Assert(m is MethodInfo);
                NewMethodPlan((m as MethodInfo).DeclaringType, m as MethodInfo, planManager, forbidNull, sugg);
            }
        }

        // For methods, it only picks receiver of type m.methodInfo.DeclaringType.
        private Type[] GetParameterTypeSuggestions(MemberInfo m, PlanManager planManager)
        {

            if (m is ConstructorInfo)
            {
                ConstructorInfo constructor = m as ConstructorInfo;
                ParameterInfo[] constructorParameters = constructor.GetParameters();

                Type[] constructorArgumentTypes = new Type[constructorParameters.Length];
                for (int i = 0; i < constructorParameters.Length; i++)
                    constructorArgumentTypes[i] = constructorParameters[i].ParameterType;

                return constructorArgumentTypes;
            }
            else
            {
                Util.Assert(m is MethodInfo);
                MethodInfo method = m as MethodInfo;
                ParameterInfo[] methodParameters = method.GetParameters();

                Type[] methodArgumentTypes = new Type[methodParameters.Length];
                for (int i = 0; i < methodParameters.Length; i++)
                    methodArgumentTypes[i] = methodParameters[i].ParameterType;


                Type[] oneResult = new Type[methodArgumentTypes.Length + 1];
                oneResult[0] = method.DeclaringType;
                Array.Copy(methodArgumentTypes, 0, oneResult, 1, methodArgumentTypes.Length);
                return oneResult;
            }
        }

        /// <summary>
        /// Tries to construct a new plan that calls a method on a
        /// receiver of the type given. May fail if there are no
        /// applicable methods or receivers available.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        private void NewMethodPlan(Type type, MethodInfo method, PlanManager planManager,
            bool forbidNull, Type[] methodArgumentTypes)
        {
            this.stats.Selected(MethodCall.Get(method).ToString());

            PlanFilter f;
            if (forbidNull)
            {
                f = delegate(Plan p, int i)
                {
                    if (!method.IsStatic && i == 0 && p.transformer is PrimitiveValueTransformer)
                        return false;

                    //// Non-null heuristic xiao.qu@us.abb.com
                    //if (!methodArgumentTypes[i].IsValueType && (p.transformer is PrimitiveValueTransformer))  
                    //    return false;

                    return true;
                };
            }
            else
            {
                f = delegate(Plan p, int i)
              {
                  if (!method.IsStatic && i == 0 && p.transformer is PrimitiveValueTransformer)
                      return false;
                  return true;
              };
            }

            RandomPlansResult r;
            if (!RandomPlans(out r, methodArgumentTypes, f, planManager.builderPlans, forbidNull, method))
            {
                stats.CreatedNew(CreationResult.NoInputs);
                return;
            }
            //Console.WriteLine("\t\t A plan has been created");

            Plan plan = new Plan(MethodCall.Get(method),
                r.fplans, r.fparameterChoosers);

            planManager.AddMaybeExecutingIfNeeded(plan, this.stats);
        }


        /// <summary>
        /// Tries to construct a new plan that calls a constructor
        /// of the type given. May fail if there are no
        /// applicable constructors.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        private void NewConstructorPlan(Type type, ConstructorInfo constructor, PlanManager planManager,
            bool forbidNull, Type[] constructorArgumentTypes)
        {
            this.stats.Selected(ConstructorCallTransformer.Get(constructor).ToString());

            PlanFilter f;
            if (forbidNull)
            {
                f = delegate(Plan p, int i)
                {
                    // Non-null heuristic
                    if (!constructorArgumentTypes[i].IsValueType && (p.transformer is PrimitiveValueTransformer))
                        return false;

                    return true;
                };
            }
            else
            {
                f = delegate(Plan p, int i)
              {
                  return true;
              };

            }

            RandomPlansResult r;
            if (!RandomPlans(out r, constructorArgumentTypes, f, planManager.builderPlans, forbidNull, null))
            {
                stats.CreatedNew(CreationResult.NoInputs);
                return;
            }
            Plan plan = new Plan(ConstructorCallTransformer.Get(constructor),
                r.fplans, r.fparameterChoosers);

            planManager.AddMaybeExecutingIfNeeded(plan, this.stats);
        }


        /// <summary>
        /// Tries to construct a new plan that sets a field for a
        /// receiver of the type given. May fail if there are no
        /// applicable receivers.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        private void NewFieldSettingPlan(FieldInfo field, PlanManager planManager, bool forbidNull)
        {
            this.stats.Selected(FieldSettingTransformer.Get(field).ToString());

            Type[] parameterTypes = new Type[2];
            parameterTypes[0] = field.DeclaringType;
            parameterTypes[1] = field.FieldType;

            RandomPlansResult r;
            if (!RandomPlans(out r, parameterTypes,
                delegate(Plan p, int i)
                {
                    if (i == 0 && p.transformer is PrimitiveValueTransformer)
                        return false;
                    return true;
                }, planManager.builderPlans, forbidNull, null))
            {
                stats.CreatedNew(CreationResult.NoInputs);
                return;
            }

            Plan plan = new Plan(FieldSettingTransformer.Get(field),
                r.fplans, r.fparameterChoosers);
            planManager.AddMaybeExecutingIfNeeded(plan, this.stats);
        }


        /// <summary>
        /// The result of calling method RandomPlans is an array fplans of plans
        /// and their corresponding parameter choosers.
        /// </summary>
        internal class RandomPlansResult
        {
            public readonly Plan[] fplans;
            public readonly Plan.ParameterChooser[] fparameterChoosers;

            public RandomPlansResult(Plan[] plans, Plan.ParameterChooser[] parameterMap)
            {
                this.fplans = plans;
                this.fparameterChoosers = parameterMap;
            }
        }

        private delegate bool PlanFilter(Plan p, int position);

        /// <summary>
        /// Selects random plans (from planDB) that yield results specified
        /// in typesDesired. Only consider plans for which filter returns true.
        /// </summary>
        private bool RandomPlans(out RandomPlansResult result, Type[] typesDesired, PlanFilter filter,
            PlanDataBase planDB, bool forbidNull, MethodInfo method)
        {
            List<Plan> plans = new List<Plan>();
            Plan.ParameterChooser[] parameterMap = new Plan.ParameterChooser[typesDesired.Length];                  

            for (int i = 0; i < typesDesired.Length; i++)
            {
                Type typeDesired = typesDesired[i];

                //if (typeDesired.IsArray || typeDesired.Equals(typeof(ArrayList))) //xiao.qu@us.abb.com changes as below to add List input generation
                if (typeDesired.IsArray || typeDesired.Equals(typeof(ArrayList)) || (!string.IsNullOrEmpty(typeDesired.FullName) && typeDesired.FullName.Contains("System.Collections.Generic.List")))
                {
                    int randArrayLength = Common.Enviroment.Random.Next(this.arrayMaxSize + 1);

                    Transformer t;
                    Type baseType = null;
                    if (typeDesired.IsArray)
                    {
                        baseType = typeDesired.GetElementType();
                        t = ArrayBuilderTransformer.Get(baseType, randArrayLength);
                    }
                    else //xiao.qu@us.abb.com addes -- start
                    {
                        if (typeDesired.FullName.Contains("System.Collections.Generic.List"))
                        {
                            //string fullType = typeDesired.ToString();
                            //string elementType = fullType.Substring(fullType.IndexOf('[') + 1, fullType.IndexOf(']') - fullType.IndexOf('[') - 1);
                            //baseType = System.Type.GetType(elementType);

                            baseType = typeDesired.GetProperty("Item").PropertyType;
                            
                            if(baseType == null)
                                baseType = typeof(object);

                            t = ListBuilderTransformer.Get(baseType, randArrayLength);
                             
                        }
                        //xiao.qu@us.abb.com addes -- end
                        else
                        {
                            baseType = typeof(object);
                            t = ArrayListBuilderTransformer.Get(typeof(object), randArrayLength);
                        }
                    }

                    RandomPlansResult arrayResult;

                    PlanFilter f;

                    if (forbidNull)
                    {
                        f = delegate(Plan p, int x)
                        {
                            // Non-null heuristic
                            if (baseType.IsValueType && (p.transformer is PrimitiveValueTransformer)
                                && (p.transformer as PrimitiveValueTransformer).IsNull)
                                return false;

                            return true;
                        };
                    }
                    else
                        f = delegate(Plan dontCareP, int dontCareInt) { return true; };

                    if (!RandomPlans(out arrayResult, t.ParameterTypes, f, planDB, forbidNull, null))
                    {
                        //Util.Warning("\tNo array plans for type " + typeDesired.Name + " created");
                        result = null;
                        return false;
                    }

                    plans.Add(new Plan(t, arrayResult.fplans, arrayResult.fparameterChoosers));
                    parameterMap[i] = new Plan.ParameterChooser(i, 0);

                }
                else
                {
                    // Types for which default plans are not created
                    // Don't want to create a plan for a value type which is just a type defn.
                    if (typeDesired.IsValueType && typeDesired.IsGenericTypeDefinition)
                    {
                        result = null; return false;
                    }

                    //Array.createInstance can't create arrays of these types
                    if (typeDesired.Name.Equals("TypedReference") ||
                        typeDesired.Name.Equals("ArgIterator") ||
                        typeDesired.Name.Equals("ByRef") ||
                        typeDesired.Name.Equals("RuntimeArgumentHandle"))
                    {
                        result = null;
                        return false;
                    }

                    if (typeDesired.IsPointer)
                    {
                        result = null;
                        return false;
                    }

                    Plan chosenplan;
                    if (i == 0 && method != null && method.IsStatic)
                    {
                        chosenplan = new Plan(DummyTransformer.Get(typeDesired), new Plan[0], new Randoop.Plan.ParameterChooser[0]);
                    }
                    else
                    {

                        chosenplan = planDB.RandomPlan(typeDesired);                        

                        if (chosenplan == null)
                        {
                            // No plans for type typeDesired created.
                            result = null;
                            return false;
                        }

                        //if ((method != null) && method.Name.Contains("heapSort") && !(chosenplan.transformer is PrimitiveValueTransformer)) //xiao.qu@us.abb.com
                        //    Console.WriteLine("debug.");

                        if (!filter(chosenplan, i))
                        {
                            result = null;
                            return false;
                        }

                        // If plan represents "null" return false.
                        if (forbidNull && (chosenplan.transformer is PrimitiveValueTransformer)
                                       && (chosenplan.transformer as PrimitiveValueTransformer).IsNull)
                        {
                            result = null;
                            return false;
                        }

                    }
                    plans.Add(chosenplan);

                    Collection<int> possibleResultIndices = PossibleResultTupleIndices(chosenplan, typeDesired);
                    if (possibleResultIndices.Count == 0)
                    {
                        result = null;
                        return false;
                    }

                    parameterMap[i] =
                        new Plan.ParameterChooser(i, possibleResultIndices[Common.Enviroment.Random.Next(possibleResultIndices.Count)]);

                }
            }

            result = new RandomPlansResult(plans.ToArray(), parameterMap);
            return true;
        }

        private Collection<int> PossibleResultTupleIndices(Plan chosenplan, Type typeDesired)
        {
            // Now, figure out the result element that we'll pick from this plan.
            Collection<int> possibleResultIndices = new Collection<int>();
            for (int j = 0; j < chosenplan.transformer.TupleTypes.Length; j++)
            {
                Type resultType = chosenplan.transformer.TupleTypes[j];

                if (typeDesired.IsInterface)
                {
                    if (resultType.IsInterface && typeDesired.Equals(resultType))
                        possibleResultIndices.Add(j);
                    else
                    {
                        foreach (Type interfaceType in resultType.GetInterfaces())
                            if (interfaceType.Equals(typeDesired))
                            {
                                possibleResultIndices.Add(j);
                                break;
                            }
                    }
                }
                else
                {
                    if (resultType.Equals(typeDesired) || ReflectionUtils.IsSubclassOrEqual(resultType, typeDesired))
                        possibleResultIndices.Add(j);
                }
            }

            Util.Assert(possibleResultIndices.Count > 0);

            Collection<int> activePossible = new Collection<int>();
            foreach (int i in possibleResultIndices)
                if (chosenplan.IsActiveTupleElement(i))
                    activePossible.Add(i);

            //Util.Assert(activePossible.Count > 0);

            return activePossible;
        }
    }
}
