//*********************************************************
//
//    xiao.qu@us.abb.com creats/edits this file on a copy of "ArrayListConstructor.cs"
//    Date: 02/29/2012 
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
    public class ListBuilderTransformer : ArrayOrArrayListBuilderTransformer
    {
        private static Dictionary<BaseTypeAndLengthPair, ListBuilderTransformer> cachedTransformers =
          new Dictionary<BaseTypeAndLengthPair, ListBuilderTransformer>();

        public static ListBuilderTransformer Get(Type arrayBaseType, int arrayLength)
        {
            ListBuilderTransformer t;
            BaseTypeAndLengthPair p = new BaseTypeAndLengthPair(arrayBaseType, arrayLength);
            cachedTransformers.TryGetValue(p, out t);
            if (t == null)
                cachedTransformers[p] = t = new ListBuilderTransformer(arrayBaseType, arrayLength);
            return t;
        }

        public override int TupleIndexOfIthInputParam(int i)
        {
            throw new NotImplementedException("Operation not supported (no input parameters in tuple)");
        }

        public override Type[] TupleTypes
        {            
            //Type genericListType = typeof(List<>).MakeGenericType(myType);
            //get { return new Type[] { typeof(List<int>) }; }
            get { return new Type[] { typeof(List<>).MakeGenericType(baseType) }; }
        }

        public override bool[] DefaultActiveTupleTypes
        {
            get { return new bool[] { true }; }
        }

        public override bool Equals(object obj)
        {
            ListBuilderTransformer t = obj as ListBuilderTransformer;
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
            return "List of " + baseType.FullName + " of length " + length;
        }

        private ListBuilderTransformer(Type arrayBaseType, int arrayLength)
            : base(arrayBaseType, arrayLength)
        {
        }

        public override string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName)
        {
            Util.Assert(arguments.Count == this.ParameterTypes.Length);

            StringBuilder b = new StringBuilder();
            string retType = "System.Collections.Generic.List<"+this.baseType+">";
            b.Append(retType + " " + newValueName + " =  new " + retType + "();\n\r");

            for (int i = 0; i < arguments.Count; i++)
            {
                b.Append("    "+newValueName + "." + "Add(" + arguments[i] + "); \n\r");
            }
            return b.ToString();
        }

        
        public override bool Execute(out ResultTuple ret, ResultTuple[] parameters,
            Plan.ParameterChooser[] parameterMap, TextWriter executionLog, TextWriter debugLog, out Exception exceptionThrown, out bool contractViolated, bool forbidNull)
        {
            contractViolated = false;

            //List<int> a = new List<int>();
            Type genericListType = typeof(List<>).MakeGenericType(baseType);
            IList a = (IList)Activator.CreateInstance(genericListType);             

            for (int i = 0; i < length; i++)
            {
                Plan.ParameterChooser pair = parameterMap[i];

                if (forbidNull)
                    Util.Assert(parameters[pair.planIndex].tuple[pair.resultIndex] != null);

                a.Add(parameters[pair.planIndex].tuple[pair.resultIndex]);
                //a.Add((int)(parameters[pair.planIndex].tuple[pair.resultIndex]));
            }

            exceptionThrown = null;
            ret = new ResultTuple(this, a);
            executionLog.WriteLine("execute listconstructor type " + a.GetType().ToString());//xiao.qu@us.abb.com adds
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
