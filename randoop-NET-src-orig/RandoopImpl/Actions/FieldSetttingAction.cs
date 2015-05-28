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

    //FieldSettingAction a = receiverVariable.CreatingCall as FieldSettingAction;
    //               Util.Assert(parameters.Length == 1);
    //               

    public class FieldSettingTransformer : Transformer
    {
        public readonly FieldInfo ffield;
        public readonly FieldInfo coverageInfo;

        private static Dictionary<FieldInfo, FieldSettingTransformer> cachedTransformers =
            new Dictionary<FieldInfo, FieldSettingTransformer>();

        public override string MemberName
        {
            get
            {
                return ffield.Name;
            }
        }

        public override int TupleIndexOfIthInputParam(int i)
        {
            throw new NotImplementedException("Operation not supported (no input parameters in tuple)");
        }

        public override Type[] TupleTypes
        {
            get { return new Type[] { ffield.DeclaringType }; }
        }

        public override bool[] DefaultActiveTupleTypes
        {
            get { return new bool[] { true }; }
        }

        public override Type[] ParameterTypes
        {
            get
            {
                Type[] retval = new Type[1];
                retval[0] = ffield.FieldType;
                return retval;
            }
        }

        public static FieldSettingTransformer Get(FieldInfo field)
        {
            FieldSettingTransformer t;
            cachedTransformers.TryGetValue(field, out t);
            if (t == null)
                cachedTransformers[field] = t = new FieldSettingTransformer(field);
            return t;
        }

        private FieldSettingTransformer(FieldInfo field)
        {
            this.ffield = field;
            this.coverageInfo = field;
        }

        public override string ToString()
        {
            return "Set field \"" + ffield.ToString() + "\" of class " + ffield.DeclaringType.ToString();
        }

        public override bool Equals(object obj)
        {
            FieldSettingTransformer f = obj as FieldSettingTransformer;
            if (f == null)
                return false;
            return (this.ffield.Equals(f.ffield));
        }

        public override int GetHashCode()
        {
            return this.ffield.GetHashCode();
        }

        public override string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName)
        {
            return
                arguments[0]
                + "."
                + ffield.Name
                + " = "
                + "("
                + SourceCodePrinting.ToCodeString(ffield.FieldType)
                + ")"
                + arguments[1]
                + ";";
        }

        public override bool Execute(out ResultTuple ret, ResultTuple[] results,
            Plan.ParameterChooser[] parameterMap, TextWriter executionLog, TextWriter debugLog, out Exception exceptionThrown, out bool contractViolated, bool forbidNull)
        {
            contractViolated = false;

            Util.Assert(parameterMap.Length == 2);

            object receiver = results[parameterMap[0].planIndex].tuple[parameterMap[0].resultIndex];
            object val = results[parameterMap[1].planIndex].tuple[parameterMap[1].resultIndex];

            if (!this.coverageInfo.IsStatic)
                Util.Assert(receiver != null);

            if (forbidNull)
                Util.Assert(val != null);

            CodeExecutor.CodeToExecute call = delegate() { ffield.SetValue(receiver, val); };

            executionLog.WriteLine("set field " + ffield.Name);
            executionLog.Flush();

            if (!CodeExecutor.ExecuteReflectionCall(call, debugLog, out exceptionThrown))
            {
                ret = null;
                return false;
            }

            ret = new ResultTuple(ffield, receiver);
            return true;
        }

        public override string Namespace
        {
            get
            {
                return this.ffield.DeclaringType.Namespace;
            }
        }

        public override ReadOnlyCollection<Assembly> Assemblies
        {
            get
            {
                return ReflectionUtils.GetRelatedAssemblies(this.ffield);
            }
        }

    }

}
