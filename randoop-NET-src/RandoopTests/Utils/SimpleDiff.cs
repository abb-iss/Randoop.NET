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

namespace RandoopTests.Utils
{
    public class SimpleDiff
    {

        public static bool Diff(string file1, string file2)
        {
            if (file1 == null) throw new ArgumentNullException();
            if (file2 == null) throw new ArgumentNullException();

            Common.StringReader reader = new Common.StringReader();
            List<string> lines1 = reader.Read(file1);
            List<string> lines2 = reader.Read(file2);

            int maxLine = Math.Max(lines1.Count - 1, lines2.Count - 1);

            for (int i = 0; i <= maxLine ; i++)
            {
                if (lines1.Count - 1 < i)
                {
                    Console.WriteLine("*** File {0} has fewer lines than file {1}.", file1, file2);
                    Console.WriteLine("*** First line not contained in smaller file:", lines1[i]);
                    return false;
                }

                if (lines2.Count - 1 < i)
                {
                    Console.WriteLine("*** File {0} has fewer lines than file {1}.", file2, file1);
                    Console.WriteLine("*** First line not contained in smaller file:", lines2[i]);
                    return false;
                }

                if (!lines1[i].Equals(lines2[i]))
                {
                    Console.WriteLine("*** Files {0} and {1} differ starting at line {2}.", file1, file2, i);
                    Console.WriteLine("*** File {0} line {1}: {2}.", file1, i, lines1[i]);
                    Console.WriteLine("*** File {0} line {1}: {2}.", file2, i, lines2[i]);
                    return false;
                }
            }

            return true;
        }

    }
}
