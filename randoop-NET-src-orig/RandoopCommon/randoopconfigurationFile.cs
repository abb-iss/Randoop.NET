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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Randoop // TODO change name to Common.
{
    /// <summary>
    /// Specifies the values for the variable parameters that affect
    /// the behavior of Randoop's input generation.
    /// 
    /// The output directory must exist.
    /// </summary>
    [XmlRoot("RandoopConfiguration")]
    public class RandoopConfiguration : CommonSerializer<RandoopConfiguration>
    {
        public void CheckRep()
        {
            if (this.assemblies == null || this.assemblies.Count == 0)
            {
                string msg = "Configuration error: must specify at least one assembly (Assemblies element).";
                throw new ArgumentException(msg);
            }

            if (this.outputdir == null)
            {
                string msg = "Configuration error: must specify output directory (OutputDir element).";
                throw new ArgumentException(msg);
            }

            if (this.statsFile == null)
            {
                string msg = "Configuration error: must specify statistics file (StatsFile element).";
                throw new ArgumentException(msg);
            }

            foreach (SimpleTypeValues vs in this.simpleTypeValues)
            {
                if (Type.GetType(vs.simpleType) == null)
                {
                    string msg = "Invalid simple type: " + vs.simpleType;
                    throw new ArgumentException(msg);
                }
            }
        }

        [XmlElement("TimeLimit")]
        public int timelimit = 100;

        [XmlElement("Assemblies")]
        public Collection<FileName> assemblies = new Collection<FileName>();

        [XmlElement("ExecutionLog")]
        public string executionLog;

        [XmlElement("TypeMatchingMode")]
        public TypeMatchingMode typematchingmode = TypeMatchingMode.SubTypes;

        [XmlElement("RandomSeed")]
        public int randomseed = 0;

        [XmlElement("RandomSource")]
        public RandomSource randomSource = RandomSource.SystemRandom;

        [XmlElement("ArrayMaxSize")]
        public int arraymaxsize = 4;

        [XmlElement("MethodWeighing")]
        public MethodWeighing methodweighing = MethodWeighing.Uniform;

        [XmlElement("ForbidNull")]
        public bool forbidnull = true;

        [XmlElement("StartWithRoundRobin")]
        public bool startwithroundrobin = false;

        [XmlElement("StartWithRoundRobinTimes")]
        public int startwithroundrobintimes = 0;

        [XmlElement("UseInternal")]
        public bool useinternal = false;

        [XmlElement("UseStatic")]
        public bool usestatic = true;

        [XmlElement("Filter")]
        public string filter = "StandardReflectionFilter";

        [XmlElement("Monkey")]
        public bool monkey = false;

        [XmlElement("FairOpt")]
        public bool fairOpt = false;

        //when true, only use the receivers as argument to other methods, ignore parameters
        [XmlElement("ForbidParamObj")]
        public bool forbidparamobj = false;

        [XmlElement("TestFileWriter")]
        public string testfilewriter;

        [XmlElement("ExecutionMode")]
        public ExecutionMode executionmode = ExecutionMode.Reflection;

        [XmlElement("OutputNormalInputs")]
        public bool outputnormalinputs = false;

        [XmlElement("MethodName")]
        public string metnodname = "Test";

        [XmlElement("PlanStartId")]
        public int planstartid = 0;

        [XmlElement("SingleDir")]
        public bool singledir = false;

        [XmlElement("OutputDir")]
        public string outputdir;

        [XmlElement("SimpleTypeValues")]
        public Collection<SimpleTypeValues> simpleTypeValues = new Collection<SimpleTypeValues>();

        [XmlElement("require_typesFile")]
        public Collection<FileName> require_typesFiles = new Collection<FileName>();

        [XmlElement("require_membersFile")]
        public Collection<FileName> require_membersFiles = new Collection<FileName>();

        [XmlElement("require_fieldsFile")]
        public Collection<FileName> require_fieldsFiles = new Collection<FileName>();

        [XmlElement("forbid_typesFile")]
        public Collection<FileName> forbid_typesFiles = new Collection<FileName>();

        [XmlElement("forbid_membersFile")]
        public Collection<FileName> forbid_membersFiles = new Collection<FileName>();

        [XmlElement("forbid_fieldsFile")]
        public Collection<FileName> forbid_fieldsFiles = new Collection<FileName>();

        [XmlElement("StatsFile")]
        public FileName statsFile;

        [XmlElement("TestPrefix")]
        public string testPrefix;


    }

    /// <summary>
    /// Specifies type-based strategy for selecting sub-plans
    /// when constructing a new plan.
    /// </summary>
    public enum TypeMatchingMode { ExactType, SubTypes };

    /// <summary>
    /// Specifies execution mode for plans.
    /// </summary>
    public enum ExecutionMode { Reflection, DontExecute }

    /// <summary>
    /// Specifies strategy for selecting methods to use in
    /// creating new plans.
    /// </summary>
    public enum MethodWeighing
    {
        Uniform,
        RoundRobin
    }

    public enum RandomSource
    {
        SystemRandom,
        Crypto
    }

    /// <summary>
    /// Wrapper around a string that represents a file name.
    /// Used for XML serialization of a RandoopConfiguration.
    /// </summary>
    public class FileName
    {
        public FileName()
        {
            this.fileName = null;
        }

        public FileName(String fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            this.fileName = fileName;
        }

        [XmlAttribute("FileName")]
        public string fileName;
    }

    /// <summary>
    /// A set of strings that represent values for a given simple type or string type.
    /// </summary>
    public class SimpleTypeValues
    {
        [XmlAttribute("SimpleType")]
        public string simpleType;

        [XmlElement("FileName")]
        public Collection<FileName> fileNames;
    }

    /// <summary>
    /// Common routines for serializing and deserializing to a file
    /// Cribbed from PipelineUtils, Lewis Bruck (lbruck), Kathy Lu (kathylu).
    /// </summary>
    /// <typeparam name="T">data type to use</typeparam>
    public abstract class CommonSerializer<T>
    {
        /// <summary>
        /// Load the object values from the file
        /// </summary>
        /// <param name="fileName">name and path of the file</param>
        /// <returns></returns>
        public static T Load(string fileName)
        {
            T obj;

            XmlSerializer tl = new XmlSerializer(typeof(T));

            using (XmlReader reader = new XmlTextReader(fileName))
            {
                obj = (T)tl.Deserialize(reader);
            }

            return obj;
        }

        // TODO resource error handling
        public static void Save(object obj, string fileName)
        {
            XmlSerializer tl = new XmlSerializer(typeof(T));

            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                tl.Serialize(writer, obj);
            }
        }

        public static string GetXml(object obj)
        {
            XmlSerializer tl = new XmlSerializer(typeof(T));

            StringWriter sw = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.Unicode;
            settings.IndentChars = "    ";
            settings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(sw, settings);

            tl.Serialize(writer, obj);

            return sw.ToString();
        }

        /// <summary>
        /// Save the object values to the file
        /// </summary>
        /// <param name="fileName">name and path of the file</param>
        /// TODO resource error handling.
        public void Save(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            Console.WriteLine("Creating xml configuration file " + fileName);
            Save(this, fileName);
        }

        public string GetXml()
        {
            return GetXml(this);
        }
    }


}
