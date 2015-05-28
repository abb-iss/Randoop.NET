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

namespace Common
{
    public class SourceCodePrinting
    {

        // TODO wherever you split on primitive types,
        // have you considered all these:
        // sbyte, byte, short, ushort, int, uint, long, ulong, char, bool, float, double, decimal


        public static String toSourceCodeString(String s)
        {
            if (s == null) throw new ArgumentNullException("s");

            StringBuilder w = new StringBuilder();

            w.Append("(new String(new char[] {");
            for (int i = 0; i < s.Length; i++)
            {
                if (i > 0) w.Append(",");
                int x = s[i];
                w.Append(ToUniCode(x));
            }
            w.Append("}))");

            return w.ToString();
        }

        public static string ToSourceCodeString(char c)
        {
            return ToUniCode(Convert.ToInt32(c));
        }

        private static string ToUniCode(int i)
        {
            return "'\\u" + String.Format("{0:x2}", i).PadLeft(4, '0') + "'";
        }

        public static string ToCodeString2(byte e)
        {
            if (e == byte.MaxValue)
                return "byte.MaxValue";
            if (e == byte.MinValue)
                return "byte.MinValue";
            return WrapNumberIfNegative(e.ToString());
        }

        public static string ToCodeString2(short e)
        {
            if (e == short.MaxValue)
                return "short.MaxValue";
            if (e == short.MinValue)
                return "short.MinValue";
            return WrapNumberIfNegative(e.ToString());
        }

        public static string ToCodeString2(int e)
        {
            if (e == int.MaxValue)
                return "int.MaxValue";
            if (e == int.MinValue)
                return "int.MinValue";
            return WrapNumberIfNegative(e.ToString());
        }

        public static string ToCodeString2(long e)
        {
            if (e == long.MaxValue)
                return "long.MaxValue";
            if (e == long.MinValue)
                return "long.MinValue";
            return WrapNumberIfNegative(e.ToString());
        }

        public static string ToCodeString2(float e)
        {
            if (e == float.MaxValue)
                return "float.MaxValue";
            if (e == float.MinValue)
                return "float.MinValue";
            if (e == float.Epsilon)
                return "float.Epsilon";
            if (float.IsNaN(e))
                return "float.NaN";
            if (float.IsNegativeInfinity(e))
                return "float.NegativeInfinity";
            if (float.IsPositiveInfinity(e))
                return "float.PositiveInfinity";
            return WrapNumberIfNegative(e.ToString());
        }

        public static string ToCodeString2(double e)
        {
            if (e == double.MaxValue)
                return "double.MaxValue";
            if (e == double.MinValue)
                return "double.MinValue";
            if (e == double.Epsilon)
                return "double.Epsilon";
            if (double.IsNaN(e))
                return "double.NaN";
            if (double.IsNegativeInfinity(e))
                return "double.NegativeInfinity";
            if (double.IsPositiveInfinity(e))
                return "double.PositiveInfinity";
            return WrapNumberIfNegative(e.ToString());
        }

        private static string WrapNumberIfNegative(string p)
        {
            if (p[0] == '-')
                return "(" + p + ")";
            return p;
        }

        /// <summary>
        /// A string representing the given enumeration, as C# code.
        /// </summary>
        public static string ToCodeString(Enum e)
        {
            // Get all the values that e represents.
            Collection<string> values = new Collection<string>();
            if (e.ToString().Contains(","))
            {
                foreach (string value in e.ToString().Split(",".ToCharArray()))
                {
                    values.Add(value.Trim());
                }
            }
            else
            {
                values.Add(e.ToString());
            }

            // Return the logical OR of the values.
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                if (i > 0)
                    b.Append(" | ");
                b.Append(e.GetType() + "." + values[i]);
            }
            return b.ToString();
        }

        private class MyGenericType
        {
            private MyGenericType declaringType;
            private string name;
            private List<MyGenericType> genericTypeArguments;
            private int numGenericTypeArgsAccum;

            public static MyGenericType Create(Type t)
            {
                if (t == null) throw new ArgumentNullException();
                if (t.ContainsGenericParameters) throw new ArgumentException("Expected closed constructed type: "
                    + t);
                return new MyGenericType(t, MapTypes(t));
            }

            private MyGenericType(Type t, Dictionary<int, Type> map)
            {


                if (t == null) throw new ArgumentNullException();

                // Assign declaring type.
                if (t.DeclaringType == null)
                {
                    this.declaringType = Dummy();

                    // Assign name.
                    this.name = t.Namespace + "." + t.Name.Split('`')[0];
                }
                else
                {
                    this.declaringType = new MyGenericType(t.DeclaringType, map);

                    // Assign name.
                    this.name = t.Name.Split('`')[0];
                }



                Type[] gTypes = t.GetGenericArguments();

                if (gTypes.Length == 0)
                {
                    // Assign generic type arguments.
                    this.genericTypeArguments = new List<MyGenericType>();

                    // Assign numGenericTypeArgsAccum.
                    this.numGenericTypeArgsAccum = this.declaringType.numGenericTypeArgsAccum;
                }
                else
                {
                    Util.Assert(gTypes.Length > this.declaringType.numGenericTypeArgsAccum);

                    // Assign generic type arguments.
                    this.genericTypeArguments = new List<MyGenericType>();
                    int numGenArgsThis = 0;
                    for (int i = 0; i < gTypes.Length; ++i)
                    {
                        if (i < this.declaringType.numGenericTypeArgsAccum)
                        {
                            Util.Assert(map.ContainsKey(i));
                            continue;
                        }
                        numGenArgsThis++;
                        if (gTypes[i].IsGenericParameter)
                        {
                            Util.Assert(map.ContainsKey(i));
                            this.genericTypeArguments.Add(MyGenericType.Create(map[i]));
                        }
                        else
                        {
                            this.genericTypeArguments.Add(MyGenericType.Create(gTypes[i]));
                        }
                    }

                    // Assign numGenericTypeArgsAccum.
                    this.numGenericTypeArgsAccum = this.declaringType.numGenericTypeArgsAccum
                        + numGenArgsThis;
                }
            }

            //private void PrintArray(Type[] type)
            //{
            //    Console.Write("!!! ");
            //    foreach (Type t in type)
            //        Console.Write(" " + (t == null ? "null" : t.ToString()));
            //    Console.WriteLine();
            //}

            //private void PrintMap(Dictionary<int, Type> map)
            //{
            //    Console.WriteLine("@@@MAP");
            //    foreach (int t in map.Keys)
            //    {
            //        Console.WriteLine(t.ToString() + " @@@ " + map[t]);
            //    }
            //}

            // Called only from private MyGenericType constructor.
            private static Dictionary<int, Type> MapTypes(Type t)
            {
                Dictionary<int, Type> map = new Dictionary<int, Type>();
                if (t.DeclaringType == null)
                    return map;
                Type[] tGenericArgs = t.GetGenericArguments();
                Type[] tParentGenericArguments = t.DeclaringType.GetGenericArguments();
                for (int i = 0; i < tParentGenericArguments.Length; i++)
                {
                    map[i] = tGenericArgs[i];
                }
                return map;
            }

            private MyGenericType()
            {
            }

            public override string ToString()
            {
                StringBuilder b = new StringBuilder();
                if (!this.declaringType.IsDummy)
                {
                    b.Append(this.declaringType.ToString());
                    b.Append(".");
                }

                b.Append(this.name);

                if (this.genericTypeArguments.Count > 0)
                {
                    b.Append("<");
                    for (int i = 0; i < this.genericTypeArguments.Count; i++)
                    {
                        if (i > 0) b.Append(", ");
                        MyGenericType t = this.genericTypeArguments[i];
                        b.Append(t.ToString());
                    }
                    b.Append(">");
                }

                return b.ToString();
            }

            public bool IsDummy { get { return this.name == ""; } }

            private static MyGenericType Dummy()
            {
                MyGenericType retval = new MyGenericType();
                retval.name = "";
                retval.declaringType = null;
                retval.genericTypeArguments = new List<MyGenericType>();
                retval.numGenericTypeArgsAccum = 0;
                return retval;
            }

        }


        public static string ToCodeString(Type t)
        {
            return MyGenericType.Create(t).ToString();
        }



    }
}
