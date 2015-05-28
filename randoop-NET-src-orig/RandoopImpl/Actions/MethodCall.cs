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
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Security;
using Common;
using System.Collections.ObjectModel;

namespace Randoop
{
    public class MethodCall : Transformer
    {
        public readonly MethodInfo method;
        private readonly Type[] resultTypes;
        private readonly bool[] defaultActiveResultTypes;
        public bool DeclaringMethodOverridesEquals;

        public int timesExecuted = 0;
        public double executionTimeAccum = 0;

        public int ReceiverUnchangedCount = 0;

        private static Dictionary<MethodInfo, MethodCall> cachedTransformers =
            new Dictionary<MethodInfo, MethodCall>();

        public static MethodCall Get(MethodInfo field)
        {
            MethodCall t;
            cachedTransformers.TryGetValue(field, out t);
            if (t == null)
                cachedTransformers[field] = t = new MethodCall(field);
            return t;
        }

        public override string MemberName
        {
            get
            {
                return method.Name;
            }
        }


        public override int TupleIndexOfIthInputParam(int i)
        {
            if (i < 0 && i > resultTypes.Length - 2 /* retval is not an input */) throw new ArgumentException("index out of range.");
            if (i == 0) return 0; // receiver
            return (i + 1); // skip retval slot.
        }

        public override Type[] ParameterTypes
        {
            get
            {
                ParameterInfo[] parameters = method.GetParameters();
                Type[] retval = new Type[parameters.Length + 1]; // add one for receiver
                retval[0] = method.DeclaringType;
                for (int i = 0; i < parameters.Length; i++)
                    retval[i + 1] = parameters[i].ParameterType;
                return retval;
            }
        }

        public override Type[] TupleTypes
        {
            get { return resultTypes; }
        }

        public override bool[] DefaultActiveTupleTypes
        {
            get { return defaultActiveResultTypes; }
        }

        public override string ToString()
        {
            return method.DeclaringType.FullName + "." + method.ToString();
        }

        public override bool Equals(object obj)
        {
            MethodCall t = obj as MethodCall;
            if (t == null)
                return false;
            return (this.method.Equals(t.method));
        }

        public override int GetHashCode()
        {
            return method.GetHashCode();
        }

        private MethodCall(MethodInfo method)
        {
            Util.Assert(method is MethodInfo);
            this.method = method as MethodInfo;

            ParameterInfo[] pis = method.GetParameters();

            resultTypes = new Type[pis.Length + 2];
            defaultActiveResultTypes = new bool[pis.Length + 2];

            resultTypes[0] = method.DeclaringType;

            if (method.IsStatic)
                defaultActiveResultTypes[0] = false;
            else
                defaultActiveResultTypes[0] = true;

            resultTypes[1] = (method as MethodInfo).ReturnType;
            if ((method as MethodInfo).ReturnType.Equals(typeof(void)))
                defaultActiveResultTypes[1] = false;
            else
                defaultActiveResultTypes[1] = true;


            for (int i = 0; i < pis.Length; i++)
            {
                resultTypes[i + 2] = pis[i].ParameterType;
                //if (pis[i].IsOut)
                //    defaultActiveResultTypes[i + 2] = true;
                //else
                defaultActiveResultTypes[i + 2] = false;
            }

            // Exceptions
            if (method.Name.Equals("GetHashCode"))
            {
                for (int i = 0; i < defaultActiveResultTypes.Length; i++)
                    defaultActiveResultTypes[i] = false;
            }

            MethodInfo equalsMethod = method.DeclaringType.GetMethod("Equals",
                new Type[] { typeof(object) },
                null /* ok because we're using DefaultBinder */);

            if (equalsMethod != null)
                DeclaringMethodOverridesEquals = true;
            else
                DeclaringMethodOverridesEquals = false;
        }

        // arguments[0] is the receiver. if static method, arguments[0] ignored.
        //
        // TODO: This method can be largely improved. For one, it should
        // be broken up into smaller methods depending on the nature of
        // the method (regular method, operator, property, etc.).
        public override string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName)
        {
            StringBuilder b = new StringBuilder();
            //return value
            string retType = method.ReturnType.ToString();
            if (!method.ReturnType.Equals(typeof(void)))
            {
                retType = SourceCodePrinting.ToCodeString(method.ReturnType);
                b.Append(retType + " " + newValueName + " = ");
            }



            //for overloaded operators
            bool isOverloadedOp = false;
            string overloadOp = "";

            bool isCastOp = false;
            string castOp = "";

            if (method.IsStatic)
            {
                //dont append the type name in case of operator overloading
                //operator overloading?

                if (ReflectionUtils.IsOverloadedOperator(method, out overloadOp))
                {
                    isOverloadedOp = true;
                }
                else if (ReflectionUtils.IsCastOperator(method, out castOp))
                {
                    isCastOp = true;
                }
                else if (method.IsSpecialName && method.Name.StartsWith("op_"))
                {
                    castOp = "(NOT HANDLED: " + method.ToString() + ")"; // TODO improve this message.
                }
                else
                {
                    isOverloadedOp = false;
                    isCastOp = false;
                    b.Append(SourceCodePrinting.ToCodeString(method.DeclaringType));
                    b.Append(".");
                }
            }
            else
            {
                Type methodDeclaringType = ParameterTypes[0];

                b.Append("((");
                b.Append(SourceCodePrinting.ToCodeString(methodDeclaringType));
                b.Append(")");
                b.Append(arguments[0]);
                b.Append(")");
                b.Append(".");

            }

            //check if its a property    
            //TODO: setter should not have a return value
            bool isSetItem = false;
            bool isItemOf = false;
            bool isGetItem = false;
            bool isChars = false;

            if (method.IsSpecialName)
            {
                string s = (method.Name);

                bool isDefaultProperty = false;
                foreach (MemberInfo mi in method.DeclaringType.GetDefaultMembers())
                {
                    if (!(mi is PropertyInfo))
                        continue;

                    PropertyInfo pi = mi as PropertyInfo;
                    if (method.Equals(pi.GetGetMethod()))
                    {
                        isDefaultProperty = true;
                        isGetItem = true;
                        b.Remove(b.Length - 1, 1); // Remove the "." that was inserted above.
                        b.Append("[");
                    }
                    else if (method.Equals(pi.GetSetMethod()))
                    {
                        isDefaultProperty = true;
                        isSetItem = true;
                        b.Remove(b.Length - 1, 1); // Remove the "." that was inserted above.
                        b.Append("[");
                    }
                }

                if (isDefaultProperty)
                {
                    // Already processed.
                }
                else if (s.StartsWith("get_ItemOf"))
                {
                    isItemOf = true;
                    b.Remove(b.Length - 1, 1); // Remove the "." that was inserted above.
                    b.Append("[");
                }
                //shuvendu: Important: replace get_Item with [] but get_ItemType with ItemType()
                // The last clause is because some classes define an "Item" property
                // that has no index.
                else if (s.Equals("get_Item") && method.GetParameters().Length > 0)
                {
                    isGetItem = true;
                    b.Remove(b.Length - 1, 1); // Remove the "." that was inserted above.
                    b.Append("[");
                }
                // The last clause is because some classes define an "Item" property
                // that has no index.
                else if ((s.Equals("set_Item") || s.Equals("set_ItemOf")) && method.GetParameters().Length > 1)
                {
                    isSetItem = true;
                    b.Remove(b.Length - 1, 1); // Remove the "." that was inserted above.
                    b.Append("[");
                }
                else if ((s.StartsWith("get_")))
                {
                    s = s.Replace("get_", "");
                    b.Append(s);
                    //                        b.Append("(");
                }
                else if (s.StartsWith("set_"))
                {
                    s = s.Replace("set_", "");
                    b.Append(s + " = ");
                }
                else if (isOverloadedOp)
                {
                    // Nothing to do here.
                }
                else if (isCastOp)
                {
                    // Nothing to do here.
                }
                else
                {
                    // Operator not handled in output generation.
                }


            }
            else
            {
                b.Append(method.Name);
                b.Append("(");
            }

            //arguments tuples 

            for (int i = 1; i < arguments.Count; i++)
            {
                //we also need to remove the typename that comes with the static method
                if (isOverloadedOp && i == 2)
                {
                    b.Append(overloadOp);
                }
                else if (isCastOp && i == 1)
                {
                    b.Append(castOp);
                }
                else if (isSetItem && i == arguments.Count - 1)
                {
                    b.Append("] = ");
                }
                else
                {
                    if (i > 1)
                        b.Append(", ");
                }


                // Cast.
                b.Append("(");
                b.Append(SourceCodePrinting.ToCodeString(ParameterTypes[i]));
                b.Append(")");
                b.Append(arguments[i]);
            }

            if (!method.IsSpecialName)
            {
                b.Append(")");
            }
            else if (isGetItem || isChars || isItemOf)
            {
                b.Append("]");
            }
            b.Append(" ;");
            return b.ToString();
        }



        public override bool Execute(out ResultTuple ret, ResultTuple[] parameters,
            Plan.ParameterChooser[] parameterMap, TextWriter executionLog, TextWriter debugLog, out Exception exceptionThrown, out bool contractViolated, bool forbidNull)
        {
            this.timesExecuted++;
            long startTime = 0;
            Timer.QueryPerformanceCounter(ref startTime);

            object[] objects = new object[method.GetParameters().Length];
            // Get the actual objects from the results using parameterIndices;
            object receiver = parameters[parameterMap[0].planIndex].tuple[parameterMap[0].resultIndex];
            for (int i = 0; i < method.GetParameters().Length; i++)
            {
                Plan.ParameterChooser pair = parameterMap[i + 1];
                objects[i] = parameters[pair.planIndex].tuple[pair.resultIndex];
            }

            // FIXME This should be true! It currently isn't.
            //if (!this.coverageInfo.methodInfo.IsStatic)
            //    Util.Assert(receiver != null);

            if (forbidNull)
                foreach (object o in objects)
                    Util.Assert(o != null);

            CodeExecutor.CodeToExecute call;

            object returnValue = null;
            contractViolated = false; //default value of contract violation

            call = delegate() { returnValue = method.Invoke(receiver, objects); };


            bool retval = true;

            executionLog.WriteLine("execute method " + this.method.Name);
            executionLog.Flush();

            if (!CodeExecutor.ExecuteReflectionCall(call, debugLog, out exceptionThrown))
            {
                //for exns we can ony add the class to faulty classes when its a guideline violation
                if (Util.GuidelineViolation(exceptionThrown.GetType()))
                {

                    PlanManager.numDistinctContractViolPlans++;

                    KeyValuePair<MethodBase, Type> k = new KeyValuePair<MethodBase, Type>(this.method, exceptionThrown.GetType());
                    if (!exnViolatingMethods.ContainsKey(k))
                    {
                        PlanManager.numContractViolatingPlans++;
                        exnViolatingMethods[k] = true;
                    }

                    //add this class to the faulty classes
                    contractExnViolatingClasses[method.DeclaringType] = true;
                    contractExnViolatingMethods[method] = true;
                }

                ret = null;
                return false;
            }
            else
                ret = new ResultTuple(method, receiver, returnValue, objects);



            //check if the objects in the output tuple violated basic contracts
            if (ret != null)
            {
                foreach (object o in ret.tuple)
                {
                    if (o == null) continue;

                    bool toStrViol, hashCodeViol, equalsViol;
                    int count;
                    if (Util.ViolatesContracts(o, out count, out toStrViol, out hashCodeViol, out equalsViol))
                    {
                        contractViolated = true;
                        contractExnViolatingMethods[method] = true;

                        PlanManager.numDistinctContractViolPlans++;

                        bool newcontractViolation = false;

                        if (toStrViol)
                        {

                            if (!toStrViolatingMethods.ContainsKey(method))
                                newcontractViolation = true;
                            toStrViolatingMethods[method] = true;
                        }
                        if (hashCodeViol)
                        {
                            if (!hashCodeViolatingMethods.ContainsKey(method))
                                newcontractViolation = true;

                            hashCodeViolatingMethods[method] = true;
                        }
                        if (equalsViol)
                        {
                            if (!equalsViolatingMethods.ContainsKey(method))
                                newcontractViolation = true;

                            equalsViolatingMethods[method] = true;
                        }

                        if (newcontractViolation)
                            PlanManager.numContractViolatingPlans++;

                        //add this class to the faulty classes
                        contractExnViolatingClasses[method.DeclaringType] = true;

                        retval = false;
                    }

                }
            }



            long endTime = 0;
            Timer.QueryPerformanceCounter(ref endTime);
            executionTimeAccum += ((double)(endTime - startTime)) / ((double)(Timer.PerfTimerFrequency));

            return retval;
        }

        public override string Namespace
        {
            get
            {
                return this.method.DeclaringType.Namespace;
            }
        }

        public override ReadOnlyCollection<Assembly> Assemblies
        {
            get
            {
                return ReflectionUtils.GetRelatedAssemblies(this.method);
            }
        }
    }

}
