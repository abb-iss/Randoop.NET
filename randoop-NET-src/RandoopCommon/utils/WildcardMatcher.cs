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
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// A wildcard matcher.
    /// 
    /// A wildcard pattern interprets '*' and '?' in a special way.
    /// An occurrence of '*' means "anything in between, including the
    /// empty string." An occurrence of '?' means "any one single character."
    /// 
    /// To avoid interpreting these two characters, use "|*|" and "|?|" instead.
    /// 
    /// For example:
    /// 
    /// "*foo*" matches any string that contains the substring "foo".
    /// "void*" matches any string that starts with void, e.g. "voidaaa", "void".
    /// "void|*|" matches exactly one string: the string "void*".
    /// 
    /// </summary>
    public class WildcardMatcher
    {
        private const string STAR_NON_META = "|*|";
        private const string STAR_NON_META_ALPHA = "RANDOOPWILDCARDMATCHERSTARNONMETAALPHA";
        private const string QUESTION_NON_META = "|?|";
        private const string QUESTION_NON_META_ALPHA = "RANDOOPWILDCARDMATCHERQUESTIONNONMETAALPHA";

        public static bool Matches(string pattern, string line)
        {
            string message;
            return Matches(pattern, line, out message);
        }

        public static bool Matches(string pattern, string line, out string message)
        {
            if (line == null) throw new ArgumentNullException("line");
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (pattern.Length == 0) throw new ArgumentException("Pattern must contain at least one character.");

            string lineNoWhiteSpace = System.Text.RegularExpressions.Regex.Replace(line, @"\s", "");
            string patternNoWhiteSpace = System.Text.RegularExpressions.Regex.Replace(pattern, @"\s", "");

            // Replace |*| --> RANDOOPWILDCARDMATCHERSTARNONMETAALPHA
            patternNoWhiteSpace = patternNoWhiteSpace.Replace(STAR_NON_META, STAR_NON_META_ALPHA);

            // Replace |?| --> RANDOOPWILDCARDMATCHERQUESTIONNONMETAALPHA
            patternNoWhiteSpace = patternNoWhiteSpace.Replace(QUESTION_NON_META, QUESTION_NON_META_ALPHA);

            string regex = "^" + patternNoWhiteSpace.Replace(@"?", ".").Replace(@"*", ".*") + "$";

            // Replace RANDOOPWILDCARDMATCHERSTARNONMETAALPHA --> \*
            regex = regex.Replace(STAR_NON_META_ALPHA, @"\*");

            // Replace RANDOOPWILDCARDMATCHERSTARNONMETAALPHA --> \?
            regex = regex.Replace(QUESTION_NON_META_ALPHA, @"\?");

            if (Regex.IsMatch(lineNoWhiteSpace, regex))
            {
                message = "pattern " + pattern + " matched string " + line;
                return true;
            }

            message = "pattern " + pattern + " did no match string " + line;
            return false;
        }
    }
}
