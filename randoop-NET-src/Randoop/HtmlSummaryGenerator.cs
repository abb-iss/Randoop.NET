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
using System.IO;
using System.Collections.ObjectModel;

namespace Randoop
{
    public class HtmlSummary
    {

        /// <summary>
        /// Ccreates a simple web page summarizing the results
        /// of running Randoop.
        /// </summary>
        public static void CreateIndexHtml(Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>> testsByMessage,
         string fileName, string command)
        {
            StreamWriter w = new StreamWriter(fileName);

            w.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">");
            w.WriteLine("");
            w.WriteLine("<html>");
            w.WriteLine("");
            w.WriteLine("<head>");
            w.WriteLine("<title>Randoop Test Generation Results</title>");
            w.WriteLine("</head>");
            w.WriteLine("");
            w.WriteLine("<body text=\"#000000\" bgcolor=\"#ffffff\">");
            w.WriteLine("");
            w.WriteLine("<font face=\"Arial, Helvetica, sans-serif\">");
            w.WriteLine("");
            w.WriteLine("<center>");

            w.WriteLine("<h3>Randoop Test Generation Results</h3>");
            w.WriteLine("<center>Generated "
                + System.DateTime.Now.ToShortDateString()
                + " "
                + System.DateTime.Now.ToShortTimeString()
                + "</center>");
            w.WriteLine("</center>");
            w.WriteLine("");
            w.WriteLine("<p>");
            w.WriteLine("Command given: <tt>randoop ");
            w.WriteLine(" " + command);
            w.WriteLine("</tt>");
            w.WriteLine("</p>");
            w.WriteLine("");


            StringBuilder tableOfContents = new StringBuilder();
            StringBuilder body = new StringBuilder();

            {
                // First do access violations
                TestCase.ExceptionDescription key =
                    TestCase.ExceptionDescription.GetDescription(typeof(System.AccessViolationException));
                if (testsByMessage.ContainsKey(key))
                {
                    string anchorName = "avs";
                    string title = "<b>Tests that lead to access violations</b>";
                    WriteHtmlTableOfContentsEntry(tableOfContents, title, anchorName, testsByMessage[key].Count, fileName);
                    WriteHtmlForOneExceptionType(body, title, anchorName, testsByMessage[key]);
                    testsByMessage.Remove(key);
                }
            }


            tableOfContents.AppendLine("</ul>");
            tableOfContents.AppendLine("<p><b>Assertion violations</b></p>");
            tableOfContents.AppendLine("<ul>");

            // Assertion violations.
            {
                TestCase.ExceptionDescription key =
                    TestCase.ExceptionDescription.GetDescription(typeof(Common.AssertionViolation));
                if (testsByMessage.ContainsKey(key))
                {
                    string anchorName = "nullrefs";
                    string title = "<b>Tests that lead to assertion violations</b>";
                    WriteHtmlTableOfContentsEntry(tableOfContents, title, anchorName, testsByMessage[key].Count, fileName);
                    WriteHtmlForOneExceptionType(body, title, anchorName, testsByMessage[key]);
                    testsByMessage.Remove(key);
                }
                else
                {
                    tableOfContents.AppendLine("<i>none found.</i>");
                }

            }


            tableOfContents.AppendLine("</ul>");
            tableOfContents.AppendLine("<p><b>Design guideline violations</b></p>");
            tableOfContents.AppendLine("<ul>");

            bool foundDesignGuidelineViolations = false;

            {
                // Null references
                TestCase.ExceptionDescription key =
                    TestCase.ExceptionDescription.GetDescription(typeof(System.NullReferenceException));
                if (testsByMessage.ContainsKey(key))
                {
                    string anchorName = "nullrefs";
                    string title = "<b>Tests that lead to NullReferenceException</b>";
                    WriteHtmlTableOfContentsEntry(tableOfContents, title, anchorName, testsByMessage[key].Count,fileName);
                    WriteHtmlForOneExceptionType(body, title, anchorName, testsByMessage[key]);
                    testsByMessage.Remove(key);
                    foundDesignGuidelineViolations = true;
                }

            }

            {
                // Index out of bounds
                TestCase.ExceptionDescription key =
                    TestCase.ExceptionDescription.GetDescription(typeof(System.IndexOutOfRangeException));
                if (testsByMessage.ContainsKey(key))
                {
                    string anchorName = "indexoutofbounds";
                    string title = "<b>Tests that lead to IndexOutOfRangeException</b>";
                    WriteHtmlTableOfContentsEntry(tableOfContents, title, anchorName, testsByMessage[key].Count, fileName);
                    WriteHtmlForOneExceptionType(body, title, anchorName, testsByMessage[key]);
                    testsByMessage.Remove(key);
                    foundDesignGuidelineViolations = true;
                }
            }

            if (!foundDesignGuidelineViolations)
            {
                tableOfContents.AppendLine("<i>none found.</i>");
            }

            tableOfContents.AppendLine("</ul>");
            tableOfContents.AppendLine("<p><b>Other exceptions</b></p>");
            tableOfContents.AppendLine("<ul>");


            // All others.
            foreach (TestCase.ExceptionDescription s in testsByMessage.Keys)
            {
                string anchorName = s.ExceptionDescriptionString.Replace('.', '_');
                string title = "Tests that lead to " + s.ExceptionDescriptionString;
                WriteHtmlTableOfContentsEntry(tableOfContents, title, anchorName, testsByMessage[s].Count, fileName);
                WriteHtmlForOneExceptionType(body, title, anchorName, testsByMessage[s]);
            }

            w.WriteLine("<hr>");
            w.WriteLine("<center><h3><a href=\"allstats.txt\">Generation statistics</a></h3></center>");

            w.WriteLine("<hr>");
            w.WriteLine("<center><h3>Exception Summary</h3></center>");
            w.WriteLine("<ul>");
            w.Write(tableOfContents.ToString());
            w.WriteLine("</ul>");

            w.WriteLine("<hr>");
            w.WriteLine("<center><h3>Tests</h3></center>");
            w.Write(body.ToString());

            w.WriteLine("</font>");
            w.WriteLine("</body>");
            w.WriteLine("</html>");
            w.WriteLine("");

            w.Close();
        }


        /// <summary>
        /// Ccreates a simple web page summarizing the results
        /// of running Randoop for VS add-in.
        /// </summary>
        public static void CreateIndexHtml2(Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>> testsByMessage,
         string fileName, string command)
        {
            StreamWriter w = new StreamWriter(fileName);

            w.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">");
            w.WriteLine("");
            w.WriteLine("<html>");
            w.WriteLine("");
            w.WriteLine("<head>");
            w.WriteLine("<title>Randoop Test Generation Results</title>");
            w.WriteLine("</head>");
            w.WriteLine("");
            w.WriteLine("<body text=\"#000000\" bgcolor=\"#ffffff\">");
            w.WriteLine("");
            w.WriteLine("<font face=\"Arial, Helvetica, sans-serif\">");
            w.WriteLine("");
            w.WriteLine("<center>");

            w.WriteLine("<h3>Randoop Test Generation Results</h3>");
            w.WriteLine("<center>Generated "
                + System.DateTime.Now.ToShortDateString()
                + " "
                + System.DateTime.Now.ToShortTimeString()
                + "</center>");
            w.WriteLine("</center>");
            w.WriteLine("");
            w.WriteLine("<p>");
            w.WriteLine("Command given: <tt>randoop ");
            w.WriteLine(" " + command);
            w.WriteLine("</tt>");
            w.WriteLine("</p>");
            w.WriteLine("");


            StringBuilder tableOfContents = new StringBuilder();                              

            w.WriteLine("<hr>");
            w.WriteLine("<center><h3><a href=\"allstats.txt\">Generation statistics</a></h3></center>");

            
            w.WriteLine("<hr>");
            w.WriteLine("<center><h3><a href=\"RandoopTest.cs\">Tests</a></h3></center>");
           
            w.WriteLine("</font>");
            w.WriteLine("</body>");
            w.WriteLine("</html>");
            w.WriteLine("");

            w.Close();
        }

        private static void WriteHtmlTableOfContentsEntry(StringBuilder w, string title, string anchorname, int p, string fileName)
        {
            w.AppendLine("<li><a href=\"" + fileName +  "#" + anchorname + "\">" + title + " (" + p + " test" + (p == 1 ? "" : "s") + ")</a>");
        }

        private static void WriteHtmlForOneExceptionType(StringBuilder w, string title, string anchorname,
            Collection<FileInfo> tests)
        {
            w.AppendLine("<hr>");
            w.AppendLine("<a name=\"" + anchorname + "\"><b>" + title + "</b></a>");
            w.AppendLine("<br>");
            w.AppendLine("<ul>");
            foreach (FileInfo fi in tests)
            {
                w.AppendLine("<li><a href=\"" + fi.FullName + "\">" + fi.Name + "</a>");
            }
            w.AppendLine("</ul>");
        }
    }
}
