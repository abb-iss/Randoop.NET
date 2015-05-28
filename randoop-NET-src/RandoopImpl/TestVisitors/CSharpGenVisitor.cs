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
using Common;

namespace Randoop
{
    /// <summary>
    /// Creates C# source code for a Plan.
    /// 
    /// WARNING: THIS CODE IS TRICKY! The main reason it's tricky
    /// is because the plan data structure is itself tricky, and
    /// that complexity spills over into this code.
    /// 
    /// The trickiest part is coming up with variable names for
    /// the values that the plan creates and propagating that
    /// information as the plan is visited.
    /// </summary>
    public class CodeGenerator
    {

        private CodeGenerator()
        {
            // Empty body.
        }

        /// <summary>
        /// Returns C# source code for the given plan.
        /// The code is returns as a list of strings, one
        /// per line (each line contains a statement).
        /// </summary>
        public static ReadOnlyCollection<string> AsCSharpCode(Plan p)
        {
            if (p == null) throw new ArgumentNullException();
            CSharpGenVisitor v = new CSharpGenVisitor();
            v.Visit(p);
            return v.Code;
        }

        public class OutPortId
        {
            private readonly List<int> list;
            private readonly int i;

            public OutPortId(Collection<int> list, int i)
            {
                if (list == null) throw new ArgumentNullException();
                this.list = new List<int>(list);
                this.i = i;
            }

            public override bool Equals(object obj)
            {
                OutPortId other = obj as OutPortId;
                bool retval = true;
                if (other == null)
                {
                    retval = false;
                }
                else if (this.i != other.i)
                {
                    retval = false;
                }
                else if (this.list.Count != other.list.Count)
                {
                    retval = false;
                }
                else
                {
                    for (int c = 0; c < this.list.Count; c++)
                        if (this.list[c] != other.list[c])
                        {
                            retval = false;
                            break;
                        }
                }

                string s1 = this.ToString();
                string s2 = other.ToString();

                if (retval == true && this.GetHashCode() != other.GetHashCode())
                    throw new Exception();
                if (retval == false && s1.Equals(s2))
                    throw new Exception();
                if (retval == true && !s1.Equals(s2))
                    throw new Exception();

                return retval;
            }

            public override int GetHashCode()
            {
                int hash = 7;
                foreach (int e in list)
                    hash = 31 * hash + e;
                hash = 31 * hash + i;
                return hash;
            }

            public override string ToString()
            {
                StringBuilder b = new StringBuilder();
                b.Append("[");
                foreach (int le in list)
                    b.Append(" " + le);
                b.Append(" ]");
                b.Append("." + i);
                return b.ToString();
            }
        }

        public class NameMap
        {
            private Dictionary<OutPortId, string> names = new Dictionary<OutPortId, string>();

            // For sanity checking (not for functionality).
            private Dictionary<string, string> namesghost = new Dictionary<string, string>();

            // For sanity checking (not for functionality).
            private List<string> keys = new List<string>();

            public string Get(OutPortId id)
            {
                return names[id];
            }

            public void set(OutPortId id, string s)
            {
                bool alreadyThere = keys.Contains(id.ToString());

                int keySize = names.Keys.Count;

                if (names.ContainsKey(id) && !namesghost.ContainsKey(id.ToString()))
                    throw new Exception();

                names[id] = s;
                namesghost[id.ToString()] = s;

                if (names.Keys.Count != namesghost.Keys.Count)
                    throw new Exception();

                if (alreadyThere && keySize < names.Keys.Count)
                    throw new Exception();

                keys.Add(id.ToString());

            }

        }

        public class NameManager
        {
            private int nameCounter = 0;

            public NameMap foobar = new NameMap();

            public void ProcessPlan(Plan p, Collection<int> path)
            {
                // Create new name for p's result.
                string freshvar = "v" + nameCounter++;

                if (p.transformer is MethodCall)
                {
                    MapOutputPortsMethod(p, path, freshvar);
                }
                else if ((p.transformer is ConstructorCallTransformer))
                {
                    MapOutputPortsConstructor(p, path, freshvar);
                }
                else if (p.transformer is ArrayOrArrayListBuilderTransformer)
                {
                    MapOutputPortsArray(p, path, freshvar);
                }
                else if (p.transformer is DummyTransformer)
                {
                    MapOutputPortsDummy(p, path, freshvar);
                }
                else if (p.transformer is PrimitiveValueTransformer)
                {
                    MapOutputPortsPrimitive(p, path, freshvar);
                }
                else //TODO: do this 
                {
                    Util.Assert(p.transformer is FieldSettingTransformer);
                    MapOutputPortsFieldSetter(p, path, freshvar);
                }
            }

            private void MapOutputPortsDummy(Plan p, Collection<int> path, string freshvar)
            {
                OutPortId newObjPort = new OutPortId(path, 0);
                foobar.set(newObjPort, freshvar);
            }

            private void MapOutputPortsPrimitive(Plan p, Collection<int> path, string freshvar)
            {
                // Map primitive.
                OutPortId newObjPort = new OutPortId(path, 0);
                foobar.set(newObjPort, freshvar);
            }

            private void MapOutputPortsFieldSetter(Plan p, Collection<int> path, string freshvar)
            {
                // Map receiver.
                Plan.ParameterChooser c = p.parameterChoosers[0];
                OutPortId pOutputPort = new OutPortId(path, 0);
                path.Add(c.planIndex);
                OutPortId recPort = new OutPortId(path, c.resultIndex);
                path.RemoveAt(path.Count - 1);
                foobar.set(pOutputPort, foobar.Get(recPort));
            }

            private void MapOutputPortsArray(Plan p, Collection<int> path, string freshvar)
            {
                // Map new array object.
                OutPortId newObjPort = new OutPortId(path, 0);
                foobar.set(newObjPort, freshvar);

                // Map parameters.
                for (int i = 0; i < p.parameterChoosers.Length; i++)
                {
                    Plan.ParameterChooser c = p.parameterChoosers[i];
                    OutPortId pOutputPort = new OutPortId(path, i + 1);
                    path.Add(c.planIndex);
                    OutPortId paramPort = new OutPortId(path, c.resultIndex);
                    path.RemoveAt(path.Count - 1);
                    foobar.set(pOutputPort, foobar.Get(paramPort));
                }
            }

            private void MapOutputPortsConstructor(Plan p, Collection<int> path, string freshvar)
            {
                // Map new object.
                OutPortId newObjPort = new OutPortId(path, 0);
                foobar.set(newObjPort, freshvar);

                // Map mutated parameters.
                for (int i = 0; i < p.parameterChoosers.Length; i++)
                {
                    Plan.ParameterChooser c = p.parameterChoosers[i];
                    OutPortId pOutputPort = new OutPortId(path, i + 1);
                    path.Add(c.planIndex);
                    OutPortId paramPort = new OutPortId(path, c.resultIndex);
                    path.RemoveAt(path.Count - 1);
                    foobar.set(pOutputPort, foobar.Get(paramPort));
                }
            }

            private void MapOutputPortsMethod(Plan p, Collection<int> path, string freshvar)
            {
                // Map receiver.
                Plan.ParameterChooser c = p.parameterChoosers[0];
                OutPortId pOutputPort = new OutPortId(path, 0);
                path.Add(c.planIndex);
                OutPortId recPort = new OutPortId(path, c.resultIndex);
                path.RemoveAt(path.Count - 1);
                foobar.set(pOutputPort, foobar.Get(recPort));

                // Map return value.
                MethodCall mct = p.transformer as MethodCall;
                if (!mct.method.ReturnType.Equals(typeof(void)))
                {
                    OutPortId newObjPort = new OutPortId(path, 1);
                    foobar.set(newObjPort, freshvar);
                }

                // Map mutated parameters.
                // Start from 1 because already handled receiver.
                for (int i = 1; i < p.parameterChoosers.Length; i++)
                {
                    c = p.parameterChoosers[i];
                    pOutputPort = new OutPortId(path, i + 1); // Add 1 because of retval.
                    path.Add(c.planIndex);
                    OutPortId paramPort = new OutPortId(path, c.resultIndex);
                    path.RemoveAt(path.Count - 1);
                    foobar.set(pOutputPort, foobar.Get(paramPort));
                }
            }
        }

        // TODO give it a more specific name.
        public class PlanVisitor
        {
            protected NameManager nameManager;

            public void Visit(Plan p)
            {
                nameManager = new NameManager();
                Visit(p, new Collection<int>());
            }

            protected virtual void Visit(Plan p, Collection<int> path)
            {
                for (int i = 0; i < p.parentPlans.Length; i++)
                {
                    Plan p2 = p.parentPlans[i];
                    path.Add(i);
                    Visit(p2, path);
                    path.RemoveAt(path.Count - 1);
                }
                nameManager.ProcessPlan(p, path);
            }
        }

        public class DebuggingStringVisitor : PlanVisitor
        {
            protected override void Visit(Plan p, Collection<int> path)
            {
                base.Visit(p, path);

            }
        }

        private class CSharpGenVisitor : PlanVisitor
        {
            public ReadOnlyCollection<string> Code
            {
                get
                {
                    return new ReadOnlyCollection<string>(b);
                }
            }

            Collection<string> b = new Collection<string>();

            protected override void Visit(Plan p, Collection<int> path)
            {
                base.Visit(p, path);

                if (p.transformer is MethodCall)
                {
                    HandleMethod(p, path);
                }
                else if ((p.transformer is ConstructorCallTransformer))
                {
                    HandleConstructor(p, path);
                }
                else if (p.transformer is ArrayOrArrayListBuilderTransformer)
                {
                    HandleArray(p, path);
                }
                else if (p.transformer is DummyTransformer)
                {
                    HandleDummy(p, path);
                }
                else if (p.transformer is PrimitiveValueTransformer)
                {
                    HandlePrimitive(p, path);
                }
                else //TODO: do this 
                {
                    Util.Assert(p.transformer is FieldSettingTransformer);
                    HandleField(p, path);
                }

            }

            private void HandleField(Plan p, Collection<int> path)
            {
                Collection<string> args = new Collection<string>();
                for (int i = 0; i < p.parameterChoosers.Length; i++)
                {
                    Plan.ParameterChooser c = p.parameterChoosers[i];
                    path.Add(c.planIndex);
                    OutPortId paramPort = new OutPortId(path, c.resultIndex);
                    path.RemoveAt(path.Count - 1);
                    args.Add(nameManager.foobar.Get(paramPort));
                }
                b.Add(p.transformer.ToCSharpCode(new ReadOnlyCollection<string>(args), "dummy"));
            }

            private void HandlePrimitive(Plan p, Collection<int> path)
            {
                OutPortId newPort = new OutPortId(path, 0);
                string newName = nameManager.foobar.Get(newPort);
                b.Add(p.transformer.ToCSharpCode(new ReadOnlyCollection<string>(new List<string>()), newName));
            }

            private void HandleDummy(Plan p, Collection<int> path)
            {
                // Dummy transformer has no associated code.
            }

            private void HandleConstructor(Plan p, Collection<int> path)
            {
                OutPortId newObjPort = new OutPortId(path, 0);
                string newObjName = nameManager.foobar.Get(newObjPort);

                Collection<string> args = new Collection<string>();
                for (int i = 0; i < p.parameterChoosers.Length; i++)
                {
                    Plan.ParameterChooser c = p.parameterChoosers[i];
                    path.Add(c.planIndex);
                    OutPortId paramPort = new OutPortId(path, c.resultIndex);
                    path.RemoveAt(path.Count - 1);
                    args.Add(nameManager.foobar.Get(paramPort));
                }
                b.Add(p.transformer.ToCSharpCode(new ReadOnlyCollection<string>(args), nameManager.foobar.Get(newObjPort)));

            }

            private void HandleMethod(Plan p, Collection<int> path)
            {
                MethodCall mct = p.transformer as MethodCall;
                string freshvar = null;
                if (!mct.method.ReturnType.Equals(typeof(void)))
                {
                    OutPortId newObjPort = new OutPortId(path, 1);
                    freshvar = nameManager.foobar.Get(newObjPort);
                }


                Collection<string> arguments = new Collection<string>();

                for (int i = 0; i < p.parameterChoosers.Length; i++)
                {
                    if (i == 0 && mct.method.IsStatic)
                    {
                        arguments.Add(null);
                        continue;
                    }
                    Plan.ParameterChooser c = p.parameterChoosers[i];
                    path.Add(c.planIndex);
                    OutPortId paramPort = new OutPortId(path, c.resultIndex);
                    path.RemoveAt(path.Count - 1);
                    arguments.Add(nameManager.foobar.Get(paramPort));
                }
                b.Add(p.transformer.ToCSharpCode(new ReadOnlyCollection<string>(arguments), freshvar));
            }


            private void HandleArray(Plan p, Collection<int> path)
            {
                OutPortId newObjPort = new OutPortId(path, 0);
                string newObjName = nameManager.foobar.Get(newObjPort);

                Collection<string> args = new Collection<string>();
                for (int i = 0; i < p.parameterChoosers.Length; i++)
                {
                    Plan.ParameterChooser c = p.parameterChoosers[i];
                    path.Add(c.planIndex);
                    OutPortId paramPort = new OutPortId(path, c.resultIndex);
                    path.RemoveAt(path.Count - 1);
                    args.Add(nameManager.foobar.Get(paramPort));
                }
                b.Add(p.transformer.ToCSharpCode(new ReadOnlyCollection<string>(args), nameManager.foobar.Get(newObjPort)));
            }
        }
    }
}
