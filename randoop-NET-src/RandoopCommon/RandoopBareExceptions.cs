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

namespace Common.RandoopBareExceptions
{
    [Serializable]
    public class InvalidUserParamsException : Exception
    {
        public InvalidUserParamsException(string message)
            : base(Common.Enviroment.RandoopBareInvalidUserParametersErrorMessage + ": " + message)
        {
        }
    }

    [Serializable]
    public class InternalError : Exception
    {
        public InternalError(string message)
            : base(Common.Enviroment.RandoopBareInternalErrorMessage + ": " + message)
        {
        }
    }
}
