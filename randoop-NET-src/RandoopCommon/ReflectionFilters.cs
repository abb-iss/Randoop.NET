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
using System.Text.RegularExpressions;
using Common;

namespace Randoop
{
    public interface IReflectionFilter
    {
        bool OkToUse(FieldInfo f, out string message);
        bool OkToUse(MethodInfo m, out string message);
        bool OkToUse(ConstructorInfo c, out string mssage);
        bool OkToUse(Type c, out string message);

    }

    public class ConfigFilesFilter : IReflectionFilter
    {
        private readonly List<string> require_types = new List<string>();
        private readonly List<string> require_members = new List<string>();
        private readonly List<string> require_fields = new List<string>();
        private readonly List<string> forbid_types = new List<string>();
        private readonly List<string> forbid_members = new List<string>();
        private readonly List<string> forbid_fields = new List<string>();

        private ConfigFilesFilter()
        {
        }

        public ConfigFilesFilter(RandoopConfiguration config)
        {
            if (config == null) throw new ArgumentNullException("config");

            Common.StringReader r = new StringReader();

            foreach (FileName path in config.forbid_typesFiles)
                forbid_types.AddRange(r.Read(path.fileName));

            foreach (FileName path in config.forbid_membersFiles)
                forbid_members.AddRange(r.Read(path.fileName));

            foreach (FileName path in config.forbid_fieldsFiles)
                forbid_fields.AddRange(r.Read(path.fileName));

            foreach (FileName path in config.require_typesFiles)
                require_types.AddRange(r.Read(path.fileName));

            foreach (FileName path in config.require_membersFiles)
                require_members.AddRange(r.Read(path.fileName));

            foreach (FileName path in config.require_fieldsFiles)
                require_fields.AddRange(r.Read(path.fileName));
        }

        public static ConfigFilesFilter CreateDefaultConfigFilesFilter()
        {
            ConfigFilesFilter f = new ConfigFilesFilter();
            Common.StringReader r = new Common.StringReader();
            f.require_types.AddRange(r.Read(Common.Enviroment.RequiretypesDefaultConfigFile));
            f.require_members.AddRange(r.Read(Common.Enviroment.RequiremembersDefaultConfigFile));
            f.require_fields.AddRange(r.Read(Common.Enviroment.RequirefieldsDefaultConfigFile));
            f.forbid_types.AddRange(r.Read(Common.Enviroment.ForbidtypesDefaultConfigFile));
            f.forbid_members.AddRange(r.Read(Common.Enviroment.ForbidmembersDefaultConfigFile));
            f.forbid_fields.AddRange(r.Read(Common.Enviroment.ForbidfieldsDefaultConfigFile));
            return f;
        }

        public bool OkToUse(FieldInfo f, out string message)
        {
            if (f == null) throw new ArgumentNullException("f");
            return OkToUseInternal(f.ToString(), this.require_fields, this.forbid_fields, out message);
        }

        public bool OkToUse(MethodInfo m, out string message)
        {
            if (m == null) throw new ArgumentNullException("m");
            string toCheckStr = m.DeclaringType.ToString() + "." + m.ToString(); //xiao.qu@us.abb.com
            return OkToUseInternal(toCheckStr + "/" + m.ReturnType.ToString(), this.require_members, this.forbid_members, out message); //xiao.qu@us.abb.com
            //return OkToUseInternal(m.ToString() + "/" + m.ReturnType.ToString(), this.require_members, this.forbid_members, out message); //xiao.qu@us.abb.com
        }

        public bool OkToUse(ConstructorInfo c, out string message)
        {
            if (c == null) throw new ArgumentNullException("c");
            string toCheckStr = c.DeclaringType.ToString() + "." + c.ToString(); //xiao.qu@us.abb.com
            return OkToUseInternal(toCheckStr, this.require_members, this.forbid_members, out message); //xiao.qu@us.abb.com
            //return OkToUseInternal(c.ToString(), this.require_members, this.forbid_members, out message); //xiao.qu@us.abb.com
        }

        public bool OkToUse(Type t, out string message)
        {
            if (t == null) throw new ArgumentNullException("t");
            return OkToUseInternal(t.ToString(), this.require_types, this.forbid_types, out message);
        }

        private static bool OkToUseInternal(string s, List<string> restrict, List<string> forbid, out string message)
        {
            bool matchesAtLeasOneRestrictPattern = false;
            foreach (string pattern in restrict)
                if (WildcardMatcher.Matches(pattern, s))
                {
                    matchesAtLeasOneRestrictPattern = true;
                    break;
                }

            if (!matchesAtLeasOneRestrictPattern)
            {
                message = "Will not use: matches no restrict pattern:" + s;
                return false;
            }

            foreach (string pattern in forbid)
            {
                if (WildcardMatcher.Matches(pattern, s, out message))
                {
                    message = "Will not use: matches forbid pattern \"" + pattern + "\":" + s;
                    return false;
                }
            }
            message = "@@@OK1" + s;
            return true;
        }

        public void PrintFilter(System.IO.TextWriter textWriter)
        {
            if (textWriter == null) throw new ArgumentNullException("textWriter");

            textWriter.WriteLine("========== Patterns for allowed types:");

            foreach (string s in this.require_types)
            {
                textWriter.WriteLine(s);
            }

            textWriter.WriteLine("========== Patterns for forbidden types:");

            foreach (string s in this.forbid_types)
            {
                textWriter.WriteLine(s);
            }

            textWriter.WriteLine("========== Patterns for allowed methods/constructors:");

            foreach (string s in this.require_members)
            {
                textWriter.WriteLine(s);
            }

            textWriter.WriteLine("========== Patterns for forbidden methods/constructors:");

            foreach (string s in this.forbid_members)
            {
                textWriter.WriteLine(s);
            }

            textWriter.WriteLine("========== Patterns for allowed fields:");

            foreach (string s in this.require_fields)
            {
                textWriter.WriteLine(s);
            }

            textWriter.WriteLine("========== Patterns for forbidden fields:");

            foreach (string s in this.forbid_fields)
            {
                textWriter.WriteLine(s);
            }
        }
    }

    // Main Filters used in different classes

    /// <summary>
    /// A filter that is composed of other filters
    /// Interpreted as the intersection of filters
    /// </summary>
    public class ComposableFilter : IReflectionFilter
    {
        Collection<IReflectionFilter> filters = new Collection<IReflectionFilter>();

        public ComposableFilter(params IReflectionFilter[] f)
        {
            foreach (IReflectionFilter filter in f)
                filters.Add(filter);
        }

        public void Add(IReflectionFilter f)
        {
            filters.Add(f);

        }

        public bool OkToUse(FieldInfo f, out string message)
        {
            message = null;
            foreach (IReflectionFilter filter in filters)
                if (!filter.OkToUse(f, out message))
                    return false;
            return true;
        }

        public bool OkToUse(MethodInfo m, out string message)
        {
            message = null;
            foreach (IReflectionFilter filter in filters)
                if (!filter.OkToUse(m, out message))
                    return false;
            return true;
        }

        public bool OkToUse(ConstructorInfo c, out string message)
        {
            message = null;
            foreach (IReflectionFilter filter in filters)
                if (!filter.OkToUse(c, out message))
                    return false;
            return true;
        }

        public bool OkToUse(Type t, out string message)
        {
            message = null;
            foreach (IReflectionFilter filter in filters)
                if (!filter.OkToUse(t, out message))
                    return false;
            return true;
        }

    }

    /// <summary>
    /// This filter can be created with two different behaviors.
    /// If created with useInternal==false, then only public members are ok.
    /// This is the filter to modify if we want to prevent something from being explored
    /// </summary>
    public class VisibilityFilter : IReflectionFilter
    {
        public bool useStaticMethods;
        public bool useInternal;

        public VisibilityFilter(RandoopConfiguration env)
        {
            this.useStaticMethods = env.usestatic;
            this.useInternal = env.useinternal;
        }

        public VisibilityFilter(bool useStatic, bool useInternal)
        {
            this.useStaticMethods = useStatic;
            this.useInternal = useInternal;
        }

        public bool OkToUse(FieldInfo fi, out string message)
        {
            //Console.Write("FFF" + fi.ToString() + " ");
            if (!fi.IsPublic)
            {
                //Console.WriteLine("FALSE");
                message = "Will not use: field is not public: " + fi.ToString();
                return false;
            }

            // Since we only currently use fields for FieldSettingTransformers,
            // we do not want any fields that cannot be modified.
            if (fi.IsInitOnly)
            {
                message = "Will not use: field is readonly: " + fi.ToString();
                return false;
            }

            if (fi.IsLiteral)
            {
                message = "Will not use: field is const: " + fi.ToString();
                return false;
            }

            // We probably don't want to modify static fields because
            // our notion of plans does not account for global state.
            if (fi.IsStatic)
            {
                //Console.WriteLine("FALSE");
                message = "Will not use: field is static: " + fi.ToString();
                return false;
            }

            //Console.WriteLine("TRUE");
            message = "@@@OK6" + fi.ToString();
            return true;
        }

        public bool OkToUse(Type t, out string message)
        {
            // The only purpose of including an interface would be to
            // add the methods they declare to the list of methods to
            // explore. But this is not necessary because classes that
            // implement the interface will declare these methods, so
            // we'll get the methods when we process the implementing
            // classes. Also, a method may implement an interface but
            // declare the implemented methods private (for example).
            if (t.IsInterface)
            {
                message = "Will not use: type is interface: " + t.ToString();
                return false;
            }

            if (ReflectionUtils.IsSubclassOrEqual(t, typeof(System.Delegate)))
            {
                message = "Will not use: type is delegate: " + t.ToString();
                return false;
            }

            if (!useInternal && !t.IsPublic)
            {
                message = "Will not use: type is no public: " + t.ToString();
                return false;
            }

            if (t.IsNested)
            {
                if (t.IsNestedPrivate)
                {
                    message = "Will not use: type is nested private: " + t.ToString();
                    return false;
                }
            }
            else
            {
                if (t.IsNotPublic)
                {
                    message = "Will not use: type is not public: " + t.ToString();
                    return false;
                }
            }
            message = "@@@OK7" + t.ToString();
            return true;

        }

        public bool OkToUse(ConstructorInfo c, out string message)
        {

            if (c.DeclaringType.IsAbstract)
            {
                message = "Will not use: constructor's declaring type is abstract: " + c.ToString();
                return false;
            }

            return OkToUseBase(c, out message);
        }

        public bool OkToUse(MethodInfo m, out string message)
        {
                return OkToUseBase(m, out message);            
        }

        /// <summary>
        /// TODO: Resolve IsPublic, IsNotPublic semantics of reflection
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        private bool OkToUseBase(MethodBase ci, out string message)
        {
            if (ci.IsAbstract)
            {
                message = "Will not use: method or constructor is abstract: " + ci.ToString();
                return false;
            }

            foreach (ParameterInfo pi in ci.GetParameters())
            {
                if (pi.ParameterType.Name.EndsWith("&"))
                {
                    message = "Will not use: method or constructor has a parameter containing \"&\": " + ci.ToString();
                    return false;
                }
                if (pi.IsOut)
                {
                    message = "Will not use: method or constructor has an out parameter: " + ci.ToString();
                    return false;
                }
                if (pi.ParameterType.IsGenericParameter)
                {
                    message = "Will not use: method or constructor has a generic parameter: " + ci.ToString();
                    return false;
                }
            }

            if (!this.useInternal && !ci.IsPublic)
            {
                message = "Will not use: method or constructor is not public: " + ci.ToString();
                return false;
            }

            if (ci.IsPrivate)
            {
                message = "Will not use: method or constructor is private: " + ci.ToString();
                return false;
            }

            if (ci.IsStatic)
            {
                if (!useStaticMethods)
                {
                    message = "Will not use: method or constructor is static: " + ci.ToString();
                    return false;
                }
            }

            if (ci.DeclaringType.Equals(typeof(object)))
            {
                message = "Will not use: method is System.Object's: " + ci.ToString();
                return false;
            }


            foreach (Attribute attr in ci.GetCustomAttributes(true))
            {

                if (attr is ObsoleteAttribute)
                {
                    //WriteLine("Obsolete Method " + ci.DeclaringType + "::" + ci.Name + "()" + "detected\n");
                    message = "Will not use: has attribute System.ObsoleteAttribute: " + ci.ToString();
                    return false;
                }
                //TODO: there are still cases where an obsolete method is not caught. e.g. System.Xml.XmlSchema::ElementType

            }

            message = "@@@OK8" + ci.ToString();
            return true;
        }
    }

}
