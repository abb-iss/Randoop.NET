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
    public class ArrayListBuilderTransformer : ArrayOrArrayListBuilderTransformer
    {
        private static Dictionary<BaseTypeAndLengthPair, ArrayListBuilderTransformer> cachedTransformers =
          new Dictionary<BaseTypeAndLengthPair, ArrayListBuilderTransformer>();

        public static ArrayListBuilderTransformer Get(Type arrayBaseType, int arrayLength)
        {
            ArrayListBuilderTransformer t;
            BaseTypeAndLengthPair p = new BaseTypeAndLengthPair(arrayBaseType, arrayLength);
            cachedTransformers.TryGetValue(p, out t);
            if (t == null)
                cachedTransformers[p] = t = new ArrayListBuilderTransformer(arrayBaseType, arrayLength);
            return t;
        }

        public override int TupleIndexOfIthInputParam(int i)
        {
            throw new NotImplementedException("Operation not supported (no input parameters in tuple)");
        }

        public override Type[] TupleTypes
        {
            get { return new Type[] { typeof(ArrayList) }; }
        }

        public override bool[] DefaultActiveTupleTypes
        {
            get { return new bool[] { true }; }
        }

        public override bool Equals(object obj)
        {
            ArrayListBuilderTransformer t = obj as ArrayListBuilderTransformer;
            if (t == null)
                return false;
            return (this.baseType.Equals(t.baseType) && this.length == t.length);
        }

        public override int GetHashCode()
        {
            return this.baseType.GetHashCode() + length.GetHashCode();
        }

        public override string ToString()
        {
            return "ArrayList of " + baseType.FullName + " of length " + length;
        }

        private ArrayListBuilderTransformer(Type arrayBaseType, int arrayLength)
            : base(arrayBaseType, arrayLength)
        {
        }

        public override string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName)
        {
            Util.Assert(arguments.Count == this.ParameterTypes.Length);

            StringBuilder b = new StringBuilder();
            string retType = "System.Collections.ArrayList";
            b.Append(retType + " " + newValueName + " =  new " + retType + "();");

            for (int i = 0; i < arguments.Count; i++)
            {
                b.Append(newValueName + "." + "Add(" + arguments[i] + "); ");
            }
            return b.ToString();
        }

        public override bool Execute(out ResultTuple ret, ResultTuple[] parameters,
            Plan.ParameterChooser[] parameterMap, TextWriter executionLog, TextWriter debugLog, out Exception exceptionThrown, out bool contractViolated, bool forbidNull)
        {
            contractViolated = false;

            ArrayList a = new ArrayList();

            for (int i = 0; i < length; i++)
            {

                Plan.ParameterChooser pair = parameterMap[i];

                if (forbidNull)
                    Util.Assert(parameters[pair.planIndex].tuple[pair.resultIndex] != null);

                a.Add(parameters[pair.planIndex].tuple[pair.resultIndex]);
            }

            exceptionThrown = null;
            ret = new ResultTuple(this, a);
            return true;
        }

        public override string Namespace
        {
            get
            {
                return this.baseType.Namespace;
            }
        }

        public override ReadOnlyCollection<Assembly> Assemblies
        {
            get
            {
                return ReflectionUtils.GetRelatedAssemblies(this.baseType);
            }
        }
    }

}
