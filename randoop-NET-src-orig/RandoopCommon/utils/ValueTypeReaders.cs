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
using System.IO;

namespace Common
{

    public abstract class ConfigFileReader<T>
    {
        public abstract T Parse(string line, out string errorMessage);

        /// <summary>
        /// File can contain empty lines, which are ignored.
        /// </summary>
        public List<T> Read(string path)
        {
            StreamReader reader = null;
            try
            {
                // TODO notify user that resource is being used.
                reader = new StreamReader(path);
            }
            catch (Exception e)
            {
                Common.Enviroment.Fail("Encountered a problem when attempting to read file "
                    + path
                    + ". Exception message: "
                    + e.Message);
            }
            string line;
            int lineCount = 0;
            List<T> valueList = new List<T>();
            while ((line = reader.ReadLine()) != null)
            {
                lineCount++;
                if (line.Trim().Equals(""))
                    continue;
                if (line.StartsWith("#"))
                    continue;
                string error;
                valueList.Add(Parse(line, out error));
                if (error != null)
                    Console.WriteLine("*** Failed to parse line {0} in file {1}.", line, path);
            }
            reader.Close();
            return valueList;
        }
    }

    public class SByteReader : ConfigFileReader<sbyte>
    {
        public override sbyte Parse(string line, out string errorMessage)
        {
            sbyte result;
            errorMessage = null;
            if (!sbyte.TryParse(line, out result))
                errorMessage = "Failed to parse sbyte";
            return result;
        }
    }

    public class ByteReader : ConfigFileReader<byte>
    {
        public override byte Parse(string line, out string errorMessage)
        {
            byte result;
            errorMessage = null;
            if (!byte.TryParse(line, out result))
                errorMessage = "Failed to parse byte";
            return result;
        }
    }

    public class ShortReader : ConfigFileReader<short>
    {
        public override short Parse(string line, out string errorMessage)
        {
            short result;
            errorMessage = null;
            if (!short.TryParse(line, out result))
                errorMessage = "Failed to parse short";
            return result;
        }
    }

    public class UshortReader : ConfigFileReader<ushort>
    {
        public override ushort Parse(string line, out string errorMessage)
        {
            ushort result;
            errorMessage = null;
            if (!ushort.TryParse(line, out result))
                errorMessage = "Failed to parse ushort";
            return result;
        }
    }

    public class IntReader : ConfigFileReader<int>
    {
        public override int Parse(string line, out string errorMessage)
        {
            int result;
            errorMessage = null;
            if (!int.TryParse(line, out result))
                errorMessage = "Failed to parse int";
            return result;
        }
    }

    public class UintReader : ConfigFileReader<uint>
    {
        public override uint Parse(string line, out string errorMessage)
        {
            uint result;
            errorMessage = null;
            if (!uint.TryParse(line, out result))
                errorMessage = "Failed to parse uint";
            return result;
        }
    }

    public class LongReader : ConfigFileReader<long>
    {
        public override long Parse(string line, out string errorMessage)
        {
            long result;
            errorMessage = null;
            if (!long.TryParse(line, out result))
                errorMessage = "Failed to parse long";
            return result;
        }
    }

    public class UlongReader : ConfigFileReader<ulong>
    {
        public override ulong Parse(string line, out string errorMessage)
        {
            ulong result;
            errorMessage = null;
            if (!ulong.TryParse(line, out result))
                errorMessage = "Failed to parse ulong";
            return result;
        }
    }

    public class CharReader : ConfigFileReader<char>
    {
        public override char Parse(string line, out string errorMessage)
        {
            char result;
            errorMessage = null;
            if (!char.TryParse(line, out result))
                errorMessage = "Failed to parse char";
            return result;
        }
    }

    public class FloatReader : ConfigFileReader<float>
    {
        public override float Parse(string line, out string errorMessage)
        {
            float result;
            errorMessage = null;
            if (!float.TryParse(line, out result))
                errorMessage = "Failed to parse float";
            return result;
        }
    }

    public class DoubleReader : ConfigFileReader<double>
    {
        public override double Parse(string line, out string errorMessage)
        {
            double result;
            errorMessage = null;
            if (!double.TryParse(line, out result))
                errorMessage = "Failed to parse double";
            return result;
        }
    }

    public class BoolReader : ConfigFileReader<bool>
    {
        public override bool Parse(string line, out string errorMessage)
        {
            bool result;
            errorMessage = null;
            if (!bool.TryParse(line, out result))
                errorMessage = "Failed to parse bool";
            return result;
        }
    }

    public class DecimalReader : ConfigFileReader<decimal>
    {
        public override decimal Parse(string line, out string errorMessage)
        {
            decimal result;
            errorMessage = null;
            if (!decimal.TryParse(line, out result))
                errorMessage = "Failed to parse decimal";
            return result;
        }
    }

    public class StringReader : ConfigFileReader<string>
    {
        public override string Parse(string line, out string errorMessage)
        {
            errorMessage = null;
            return line;
        }
    }
}
