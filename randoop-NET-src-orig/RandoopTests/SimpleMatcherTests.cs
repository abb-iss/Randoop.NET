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
using Common;
using System.Diagnostics;

namespace RandoopTests
{
    class SimpleMatcherTests
    {
        internal static void Test()
        {
            string matcher;
            
            matcher = "foo";
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "foo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "f oo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, " foo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, " foo "));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "foo "));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "fooa"));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "afoo"));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "a foo"));

            matcher = "*foo";
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "foo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "fo o"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, " foo "));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "fooa"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "afoo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "af oo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, " afoo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, " af oo"));

            matcher = "foo*";
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "foo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, " foo "));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "fooa"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "fooa "));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "fo oa "));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "afoo"));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "afo o"));

            matcher = "*foo*";
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "foo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, " foo "));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "fooa"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "afoo"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "afoob"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "a f o o b"));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "afocob"));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "a f o c o b"));

            matcher = "fo|*|o*";
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "fo*"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "fo*o"));
            Common.Util.Assert(true == WildcardMatcher.Matches(matcher, "fo*oa"));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "foao"));
            Common.Util.Assert(false == WildcardMatcher.Matches(matcher, "foaoa"));
        }
    }
}

