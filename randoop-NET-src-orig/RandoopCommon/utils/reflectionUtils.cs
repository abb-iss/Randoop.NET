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
using System.Collections.ObjectModel;

namespace Common
{
    public class ReflectionUtils
    {

        private class TypePair
        {
            private Type t1;
            private Type t2;

            public TypePair(Type t1, Type t2)
            {
                this.t1 = t1;
                this.t2 = t2;
            }

            public override bool Equals(object obj)
            {
                TypePair other = obj as TypePair;
                if (other == null) return false;
                return this.t1.Equals(other.t1) && this.t2.Equals(other.t2);
            }

            public override int GetHashCode()
            {
                int result = 17;
                result = 37 * result + t1.GetHashCode();
                result = 37 * result + t2.GetHashCode();
                return result;
            }
        }

#region cached data structures
        private static Dictionary <Type,Collection<Type>> cachedRelatedTypes = new Dictionary<Type,Collection<Type>>();
        private static Dictionary<Type, Collection<Assembly>> cachedRelatedAssembliesPerType = new Dictionary<Type,Collection<Assembly>>();
        private static Dictionary<FieldInfo, Collection<Assembly>> cachedRelatedAssembliesPerField = new Dictionary<FieldInfo,Collection<Assembly>>();
        private static Dictionary<MethodBase, Collection<Assembly>> cachedRelatedAssembliesPerMethod = new Dictionary<MethodBase,Collection<Assembly>>();
        private static Dictionary<TypePair, bool> subTypeCached = new Dictionary<TypePair, bool>();

#endregion


        public static bool IsSubclassOrEqual(Type t1, Type t2)
        {
            if (t1 == null) throw new ArgumentNullException();
            if (t2 == null) throw new ArgumentNullException();

            if (t1.Equals(t2)) return true;

            bool result;
            TypePair typePair = new TypePair(t1, t2);
            if (!subTypeCached.TryGetValue(typePair, out result))
            {
                result = subTypeCached[typePair] = t1.IsSubclassOf(t2);
            }
            return result;
        }

        public static Collection<Type> GetExplorableTypes(Collection<Assembly> assemblies)
        {
            Collection<Type> retval = new Collection<Type>();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type t in assembly.GetExportedTypes())
                {
                    retval.Add(t);
                }
            }
            return retval;
        }


        /// <summary>
        /// Checks if mi is a known overloaded operator
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public static bool IsOverloadedOperator(MethodInfo mi, out string op)
        {
            op = "";
            bool status = true;
            if (mi.Name.Contains("op_Equality"))
            {
                op = "==";
            }
            else if (mi.Name.Contains("op_LogicalNot"))
            {
                op = "!";
            }
            else if (mi.Name.Contains("op_Inequality"))
            {
                op = "!=";
            }
            else if (mi.Name.Contains("op_GreaterThan"))
            {
                op = ">";
            }
            else if (mi.Name.Contains("op_LessThan"))
            {
                op = "<";
            }
            else if (mi.Name.Contains("op_LessThanOrEqual"))
            {
                op = "<=";
            }
            else if (mi.Name.Contains("op_GreaterThanOrEqual"))
            {
                op = ">=";
            }
            else if (mi.Name.Contains("op_Addition"))
            {
                op = "+";
            }
            else if (mi.Name.Contains("op_Subtraction"))
            {
                op = "-";
            }
            else if (mi.Name.Contains("op_Multiply"))
            {
                op = "*";
            }
            else if (mi.Name.Contains("op_Division"))
            {
                op = "/";
            }
            else if (mi.Name.Contains("op_Increment"))
            {
                op = "++";
            }
            else if (mi.Name.Contains("op_Decrement"))
            {
                op = "--";
            }
            else if (mi.Name.Contains("op_UnaryNegation"))
            {
                op = "-";
            }
            else if (mi.Name.Contains("op_UnaryPlus"))
            {
                op = "+";
            }
            else if (mi.Name.Contains("op_Modulus"))
            {
                op = "%";
            }
            else if (mi.Name.Contains("op_ExclusiveOr"))
            {
                op = "^";
            }
            else if (mi.Name.Contains("op_BitwiseAnd"))
            {
                op = "&";
            }
            else if (mi.Name.Contains("op_BitwiseOr"))
            {
                op = "|";
            }
            else if (mi.Name.Contains("op_OnesComplement"))
            {
                op = "~";
            }
            else if (mi.Name.Contains("op_Negation"))
            {
                op = "!";
            }
            else if (mi.Name.Contains("op_LeftShift"))
            {
                op = "<<";
            }
            else if (mi.Name.Contains("op_RightShift"))
            {
                op = ">>";
            }
            else
            {
                status = false;
            }

            return status;
        }

        /// <summary>
        /// Casts between various types
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="castOp"></param>
        /// <returns></returns>
        public static bool IsCastOperator(MethodInfo mi, out string castOp)
        {
            bool status = true;
            castOp = "";
            if (mi.Name.Contains("op_Explicit") ||
                mi.Name.Contains("op_Implicit"))
            {
                castOp = "(" + SourceCodePrinting.ToCodeString(mi.ReturnType) + ")";
            }
            else if (mi.Name.Contains("op_True"))
            {
                castOp = "(bool)";
            }
            else if (mi.Name.Contains("op_False"))
            {
                castOp = "(bool)";
            }
            else
            {
                status = false;
            }
            return status;

        }

        public static ReadOnlyCollection<Type> GetRelatedTypes(Type type)
        {
            Collection<Type> ret;
            cachedRelatedTypes.TryGetValue(type, out ret);

            if (ret != null)
                return new ReadOnlyCollection<Type>(ret);


            List<Type> relatedTypes = new List<Type>();

            // Add the type and its base types.
            relatedTypes.Add(type);
            relatedTypes.AddRange(BaseTypesRecursive(type));
            relatedTypes.AddRange(CollectInterfaces(type));

            cachedRelatedTypes.Add(type, new Collection<Type>(relatedTypes));

            return new ReadOnlyCollection<Type>(relatedTypes);
        }

        // TODO add explanation of motivation behind this definition
        // of "related assemblies."
        public static ReadOnlyCollection<Assembly> GetRelatedAssemblies(Type type)
        {

            Collection<Assembly> ret;
            cachedRelatedAssembliesPerType.TryGetValue(type, out ret);

            if (ret != null)
                return new ReadOnlyCollection<Assembly>(ret);

            ReadOnlyCollection<Type> relatedTypes = GetRelatedTypes(type);

            // Collect related assemblies.
            Collection<Assembly> retval = new Collection<Assembly>();
            foreach (Type t in relatedTypes)
            {
                if (t.Assembly != null)
                    retval.Add(t.Assembly);
            }

            cachedRelatedAssembliesPerType.Add(type, new Collection<Assembly>(retval));

            return new ReadOnlyCollection<Assembly>(retval);
        }

        public static ReadOnlyCollection<Assembly> GetRelatedAssemblies(FieldInfo field)
        {

            Collection<Assembly> ret;
            cachedRelatedAssembliesPerField.TryGetValue(field, out ret);

            if (ret != null)
                return new ReadOnlyCollection<Assembly>(ret);

            List<Type> relatedTypes = new List<Type>();

            // Add the type and base types of the field.
            relatedTypes.Add(field.FieldType);
            relatedTypes.AddRange(BaseTypesRecursive(field.FieldType));
            relatedTypes.AddRange(CollectInterfaces(field.FieldType));

            // Add the types and base types of the declaring type.
            relatedTypes.Add(field.DeclaringType);
            relatedTypes.AddRange(BaseTypesRecursive(field.DeclaringType));
            relatedTypes.AddRange(CollectInterfaces(field.DeclaringType));

            // Collect related assemblies.
            List<Assembly> retval = new List<Assembly>();
            foreach (Type t in relatedTypes)
            {
                if (t.Assembly != null)
                    retval.Add(t.Assembly);
            }

            cachedRelatedAssembliesPerField.Add(field, new Collection<Assembly>(retval));
            return new ReadOnlyCollection<Assembly>(retval);
        }

        public static ReadOnlyCollection<Assembly> GetRelatedAssemblies(MethodBase m)
        {

            Collection<Assembly> ret;
            cachedRelatedAssembliesPerMethod.TryGetValue(m, out ret);

            if (ret != null)
                return new ReadOnlyCollection<Assembly>(ret);

            List<Type> relatedTypes = new List<Type>();

            // Add the type and base types of the declaring type.
            relatedTypes.Add(m.DeclaringType);
            relatedTypes.AddRange(BaseTypesRecursive(m.DeclaringType));
            relatedTypes.AddRange(CollectInterfaces(m.DeclaringType));

            // Add the types and base types of the parameters.
            foreach (ParameterInfo pi in m.GetParameters())
            {
                relatedTypes.Add(pi.ParameterType);
                relatedTypes.AddRange(BaseTypesRecursive(pi.ParameterType));
                relatedTypes.AddRange(CollectInterfaces(pi.ParameterType));
            }

            // Collect related assemblies.
            Collection<Assembly> retval = new Collection<Assembly>();
            foreach (Type t in relatedTypes)
            {
                if (t.Assembly != null)
                    retval.Add(t.Assembly);
            }
            cachedRelatedAssembliesPerMethod.Add(m, new Collection<Assembly>(retval));

            return new ReadOnlyCollection<Assembly>(retval);
        }

        private static ReadOnlyCollection<Type> CollectInterfaces(Type type)
        {
            if (type == null) throw new ArgumentNullException();

            List<Type> retval = new List<Type>();

            foreach (Type inter in type.GetInterfaces())
            {
                retval.AddRange(CollectInterfaces(inter));
                retval.Add(inter);
            }

            return new ReadOnlyCollection<Type>(retval);
        }

        private static Collection<Type> BaseTypesRecursive(Type type)
        {
            Collection<Type> retval = new Collection<Type>();
            Type baseType = type.BaseType;
            while (baseType != null)
            {
                retval.Add(baseType);
                baseType = baseType.BaseType;
            }
            return retval;
        }


        public static bool IsPublic(Type t)
        {
            return t.IsNestedPublic || t.IsPublic;
        }


    }
}
