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
    /// Represents a way to change the state of one or more objects,
    /// for example, through a method call or a field update.
    /// </summary>
    public abstract class Transformer
    {
        /// <summary>
        /// Returns false when the execution fails due to exception
        /// Returns true even when some contracts are violated
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="results"></param>
        /// <param name="parameterMap"></param>
        /// <param name="writer"></param>
        /// <param name="exceptionThrown"></param>
        /// <param name="contractViolated"></param>
        /// <param name="forbidNull"></param>
        /// <returns></returns>
        public abstract bool Execute(out ResultTuple ret,
            ResultTuple[] results, Plan.ParameterChooser[] parameterMap, TextWriter executionLog,
            TextWriter writer, out Exception exceptionThrown, out bool contractViolated, bool forbidNull);

        public abstract Type[] ParameterTypes { get; }

        public abstract Type[] TupleTypes { get; }

        public abstract bool[] DefaultActiveTupleTypes { get; }

        // to keep track of the contract violations
        public static Dictionary<Type, bool> contractExnViolatingClasses = new Dictionary<Type, bool>();
        public static Dictionary<MethodBase, bool> contractExnViolatingMethods = new Dictionary<MethodBase, bool>();
        public static Dictionary<KeyValuePair<MethodBase, Type>, bool> exnViolatingMethods = new Dictionary<KeyValuePair<MethodBase, Type>, bool>();
        public static Dictionary<MethodBase, bool> toStrViolatingMethods = new Dictionary<MethodBase, bool>();
        public static Dictionary<MethodBase, bool> hashCodeViolatingMethods = new Dictionary<MethodBase, bool>();
        public static Dictionary<MethodBase, bool> equalsViolatingMethods = new Dictionary<MethodBase, bool>();



        public abstract int TupleIndexOfIthInputParam(int i);

        /// <summary>
        /// The namespace that must be imported for this transformer to compile.
        /// </summary>
        public abstract string Namespace
        {
            get;
        }

        /// <summary>
        /// The assembly which must be referenced for this transformer to compile.
        /// </summary>
        public abstract ReadOnlyCollection<Assembly> Assemblies
        {
            get;
        }

        public abstract string MemberName { get; }

        public abstract string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName);

    }

    // Used only to key into the transformer cache.
    class BaseTypeAndLengthPair
    {
        public readonly Type baseType;
        public readonly int length;

        public BaseTypeAndLengthPair(Type b, int i)
        {
            this.baseType = b;
            this.length = i;
        }

        public override bool Equals(object obj)
        {
            BaseTypeAndLengthPair other = obj as BaseTypeAndLengthPair;
            if (obj == null)
                return false;
            return (this.baseType.Equals(other.baseType) && this.length == other.length);
        }

        public override int GetHashCode()
        {
            return this.baseType.GetHashCode() + this.length.GetHashCode();
        }
    }

    // Only for keying into the cache.
    class TypeValuePair
    {
        Type type;
        object value;

        public TypeValuePair(Type type, object value)
        {
            this.type = type;
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            TypeValuePair other = obj as TypeValuePair;
            if (obj == null)
                return false;

            if (!this.type.Equals(other.type))
                return false;

            return Util.SafeEqual(this.value, other.value);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() + ((value == null || value.GetType().IsValueType) ? 0 : value.GetHashCode());
        }

    }
}