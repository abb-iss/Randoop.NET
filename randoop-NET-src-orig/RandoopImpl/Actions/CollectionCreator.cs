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

    public abstract class ArrayOrArrayListBuilderTransformer : Transformer
    {
        public readonly Type baseType;
        public readonly int length;


        public override string MemberName
        {
            get
            {
                return "n/a";
            }
        }

        public override Type[] ParameterTypes
        {
            get
            {
                Type[] retval = new Type[length];
                for (int i = 0; i < length; i++)
                    retval[i] = baseType;
                return retval;
            }
        }

        protected ArrayOrArrayListBuilderTransformer(Type arrayBaseType, int arrayLength)
        {
            Util.Assert(arrayBaseType != null && arrayLength >= 0);
            this.baseType = arrayBaseType;
            this.length = arrayLength;
        }
    }

}
