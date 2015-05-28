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
    ////////////////////

    /// <summary>
    /// This is a special transformer that doesn't transform any state
    /// but instead represents a plan that yields a primitive value.
    /// </summary>
    public class DummyTransformer : Transformer
    {
        public readonly Type ftype;

        private static Dictionary<Type, DummyTransformer> cachedTransformers =
            new Dictionary<Type, DummyTransformer>();

        public static DummyTransformer Get(Type type)
        {
            DummyTransformer t;
            cachedTransformers.TryGetValue(type, out t);
            if (t == null)
                cachedTransformers[type] = t = new DummyTransformer(type);

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

        /// <summary>
        /// Method returns "string_rep_not_available" when no string representation is available
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DummyTransformer";
        }

        // TODO Handle other cases (e.g. newlines?)
        private string ToSourceCodestring(string fvalueToString)
        {
            return "";
        }

        public override bool Equals(object obj)
        {
            DummyTransformer p = obj as DummyTransformer;
            if (p == null)
                return false;
            if (!this.ftype.Equals(p.ftype))
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return ftype.GetHashCode();
        }

        private DummyTransformer(Type type)
        {
            this.ftype = type;
        }

        public override bool Execute(out ResultTuple ret, ResultTuple[] results,
            Plan.ParameterChooser[] parameterMap, TextWriter executionLog, TextWriter debugLog, out Exception exceptionThrown, out bool contractViolated, bool forbidNull)
        {
            contractViolated = false;
            exceptionThrown = null;
            ret = new ResultTuple(ftype, new object[] { null });
            return true;
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

        public override string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName)
        {
            throw new NotImplementedException("not implemented.");
        }

    }


}
