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
using System.Reflection;
using Common;

namespace Randoop
{
    public class IVisitor
    {
        public virtual void Accept(Plan p)
        {
            // Empty body.
        }

        public virtual void Accept(Transformer t)
        {
            // Empty body.
        }
    }

    public class CollectReferencedAssembliesVisitor : IVisitor
    {
        static private Dictionary<Assembly, string> cachedLocations = new Dictionary<Assembly, string>();

        // TODO this is actually a set. Are there no sets in .NET collections?
        Collection<string> declaringTypes = new Collection<string>();

        // TODO this is actually a set. Are there no sets in .NET collections?
        Collection<string> assemblyNames = new Collection<string>();

        public override void Accept(Transformer t)
        {
            // TODO This assert breaks when using /internal in System.Data.
            Util.Assert(t.Namespace != null);

            ICollection<Assembly> assemblies = t.Assemblies;

            if (!declaringTypes.Contains(t.Namespace))
            {
                declaringTypes.Add(t.Namespace);
            }

            foreach (Assembly assembly in assemblies)
            {
                string res = null;
                cachedLocations.TryGetValue(assembly, out res);
                if (res == null)
                {   
                    string r = assembly.Location;
                    if (!assemblyNames.Contains(r))
                    {
                        assemblyNames.Add(r);
                        cachedLocations.Add(assembly, r);
                    }
                }
                else
                {
                    assemblyNames.Add(res);
                }
            }
        }

        public Collection<string> GetDeclaringTypes()
        {
            return declaringTypes;
        }

        public Collection<string> GetAssemblies()
        {
            return assemblyNames;
        }

    }

}
