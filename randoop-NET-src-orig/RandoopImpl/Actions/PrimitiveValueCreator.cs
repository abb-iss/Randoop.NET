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

    /// <summary>
    /// This is a special transformer that doesn't transform any state
    /// but instead represents a plan that yields a primitive value.
    /// </summary>
    public class PrimitiveValueTransformer : Transformer
    {
        public readonly Type ftype;
        public readonly object fvalue;

        private static Dictionary<TypeValuePair, PrimitiveValueTransformer> cachedTransformers =
            new Dictionary<TypeValuePair, PrimitiveValueTransformer>();

        public static PrimitiveValueTransformer Get(Type type, object value)
        {
            //if (ReflectionUtils.IsStaticClass(type))
            //{
            //    throw new ArgumentException("Type is static class: " + type.ToString());
            //}

            TypeValuePair p = new TypeValuePair(type, value);
            PrimitiveValueTransformer t;
            cachedTransformers.TryGetValue(p, out t);
            if (t == null)
                cachedTransformers[p] = t = new PrimitiveValueTransformer(type, value);

            return t;
        }


        public override string MemberName
        {
            get
            {
                return "n/a";
            }
        }


        public override int TupleIndexOfIthInputParam(int i)
        {
            throw new NotImplementedException("Operation not supported.");
        }

        public override Type[] TupleTypes
        {
            get { return new Type[] { ftype }; }
        }

        public override bool[] DefaultActiveTupleTypes
        {
            get { return new bool[] { true }; }
        }

        public override Type[] ParameterTypes
        {
            get
            {
                return new Type[0];
            }
        }

        public bool IsNull
        {
            get
            {
                if (ftype.IsPrimitive)
                    return false;
                if (fvalue == null)
                    return true;
                else
                    return false;
            }
        }

        private static Type systemEnum = Type.GetType("System.Enum");

        /// <summary>
        /// Method returns "string_rep_not_available" when no string representation is available
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (fvalue == null)
                return "null";

            if (fvalue.GetType().Equals(typeof(string)))
            {
                return SourceCodePrinting.toSourceCodeString((string)fvalue);
            }

            string fvalueToString = null;
            Type fvalueType = null;
            try
            {
                fvalueToString = fvalue.ToString();
                fvalueType = fvalue.GetType();
            }
            catch (Exception)
            {
                return "not_printable_ToString_Crashed";
            }

            if (fvalueType.IsPrimitive)
            {
                if (fvalueType.Equals(typeof(bool)))
                    return "(" + ftype.ToString() + ")" + (fvalueToString.ToLower());

                if (fvalueType.Equals(typeof(byte)))
                    return "(" + ftype.ToString() + ")" + SourceCodePrinting.ToCodeString2((byte)fvalue);

                if (fvalueType.Equals(typeof(short)))
                    return "(" + ftype.ToString() + ")" + SourceCodePrinting.ToCodeString2((short)fvalue);

                if (fvalueType.Equals(typeof(int)))
                    return "(" + ftype.ToString() + ")" + SourceCodePrinting.ToCodeString2((int)fvalue);

                if (fvalueType.Equals(typeof(long)))
                    return "(" + ftype.ToString() + ")" + SourceCodePrinting.ToCodeString2((long)fvalue);

                if (fvalueType.Equals(typeof(float)))
                    return "(" + ftype.ToString() + ")" + SourceCodePrinting.ToCodeString2((float)fvalue);

                if (fvalueType.Equals(typeof(double)))
                    return "(" + ftype.ToString() + ")" + SourceCodePrinting.ToCodeString2((double)fvalue);

                if (fvalueType.Equals(typeof(char)))
                {
                    return "\'" + fvalueToString + "\'";
                }

                else
                    return "(" + ftype.ToString() + ") (" + fvalueToString + ")";

            }

            if (ftype.BaseType.Equals(systemEnum))
            {
                return SourceCodePrinting.ToCodeString(ftype) + "." + fvalueToString;
            }

            if (fvalueType.IsValueType)
            {
                //check to see if this is the default value

                bool vEqArray = false;
                try
                {
                    ValueType v = fvalue as ValueType;
                    vEqArray = v.Equals(Array.CreateInstance(ftype, 1).GetValue(0));
                }
                catch (Exception)
                {
                    // Let vEqArray stay false. 
                }

                if (vEqArray)
                {

                    string tStr = "typeof(" + SourceCodePrinting.ToCodeString(ftype) + ")";
                    string s = "(" + SourceCodePrinting.ToCodeString(ftype) + ")(System.Array.CreateInstance(" + tStr + ", 1)).GetValue(0)";
                    return s;

                }
                else
                {
                    try
                    {
                        //just perform a typecast to be sure (e.g. 1.1 is not recognized as a float unless cast
                        return "(" + SourceCodePrinting.ToCodeString(ftype) + ")(" + fvalueToString + ")";
                    }
                    catch
                    {
                        //This operation raises an exception sometime
                        //Util.Warning("The default value of ValueType<" + ftype.Name + "> is not" + fvalue.ToString());

                        return ("\"__randoop_string_rep_not_available_forType<" + ftype.Name + ">\"");
                    }
                }
            }

            //just plain unlucky in this case
            return ("\"__randoop_string_rep_not_available_forType<" + ftype.Name + ">\"");
        }

        // TODO Handle other cases (e.g. newlines?)
        private string ToSourceCodestring(string fvalueToString)
        {
            return
                fvalueToString.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        public override bool Equals(object obj)
        {
            PrimitiveValueTransformer p = obj as PrimitiveValueTransformer;
            if (p == null)
                return false;
            if (!this.ftype.Equals(p.ftype))
                return false;
            if (fvalue == null)
            {
                if (p.fvalue == null)
                    return true;
                else
                    return false;
            }
            else
            {
                try
                {
                    return fvalue.Equals(p.fvalue);
                }
                catch (Exception)
                {
                    // Execution of code under test led to an exception.
                    return false;
                }
            }

        }

        public override int GetHashCode()
        {
            return ftype.GetHashCode() + ((fvalue == null || fvalue.GetType().IsValueType) ? 0 : fvalue.GetHashCode());
        }

        private PrimitiveValueTransformer(Type type, object value)
        {
            if (type.IsValueType)
            {
                if (value == null)
                    throw new ArgumentException("value can't be null for a value type.");

                if (!value.GetType().Equals(type))
                    throw new ArgumentException("type of value is not equal to type parameters.");
            }
            else
            {
                if (value != null && !value.GetType().Equals(typeof(string)))
                    throw new ArgumentException("value must be null for a non-value, non-string type.");
            }
            this.ftype = type;
            this.fvalue = value;
        }

        public override string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName)
        {
            StringBuilder b = new StringBuilder();
            b.Append(
                SourceCodePrinting.ToCodeString(this.ftype)
                + " "
                + newValueName
                + " = "
                + this.ToString()
                + ";");
            return b.ToString();
        }

        public override bool Execute(out ResultTuple ret, ResultTuple[] results,
            Plan.ParameterChooser[] parameterMap, TextWriter executionLog, TextWriter debugLog, out Exception exceptionThrown, out bool contractViolated, bool forbidNull)
        {
            contractViolated = false;

            //if (forbidNull)
            //    Util.Assert(fvalue != null);

            exceptionThrown = null;
            ret = new ResultTuple(ftype, new object[] { fvalue });
            return true;
        }

        private void AddIfDifferentFromValue(Collection<object> l, object o)
        {
            if (!Util.SafeEqual(o, this.fvalue))
                l.Add(o);
        }

        public override string Namespace
        {
            get
            {
                return this.ftype.Namespace;
            }
        }

        public override ReadOnlyCollection<Assembly> Assemblies
        {
            get
            {
                return ReflectionUtils.GetRelatedAssemblies(this.ftype);
            }
        }
    }
}
