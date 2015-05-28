//*********************************************************
//  NOTE: 
// xiao.qu@us.abb.com addes it for replacing calls to 
// specific methods, such as MessageBox.Show()
//
//  Date: 2012/06/28 
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Mdb;
using System.Reflection;
using Mono.Cecil.Pdb;
//using Mono.Reflection;
//using System.Windows.Forms;

namespace Common
{
    public class Instrument
    {
        public static void ParseMapFile(string file, ref Dictionary<string, string> map)
        {   
            //openfile
            string line;
            int count = 0;
            System.IO.StreamReader filereader = new System.IO.StreamReader(file);
            //read line by line
            while ((line = filereader.ReadLine()) != null)
            {
                count++;
                Console.WriteLine(line); //debug
                line = line.Trim();
                if (line.StartsWith("#") || line.Equals(""))
                    continue;

                string[] methods = line.Split(';');
                if (methods.Length != 2)
                {
                    filereader.Close();
                    throw new InvalidFileFormatException("more/less than one splitter is detected on line "+count.ToString());
                }

                string keystr = methods[0].Trim();
                string valstr = methods[1].Trim();
                map[keystr] = valstr;                
            }

            filereader.Close();            
            
            //string keystr = "System.Windows.Forms.MessageBox::Show";
            //string valstr = "System.Console::Writeline";
            //map[keystr] = valstr;
        }

        private static string getNamespace(string newMethod)
        {
            int index = newMethod.IndexOf("::");
            string line = "";

            if (index <= 0)
                throw new InvalidFileFormatException("invalid method declaration, a class name of the method is expected.");

            line = newMethod.Substring(0, index).Trim();
            return line;
            
            //return "System.Console";
        }

        private static string getMethodName(string newMethod)
        {
            int index1 = newMethod.IndexOf("::");
            int index2 = newMethod.IndexOf("(");
            string line = "";

            if (index1 <= 0)
                throw new InvalidFileFormatException("invalid method declaration, a class name of the method is expected.");

            if (index2 > index1)
                line = newMethod.Substring(index1+ 2, index2-index1-2).Trim();
            else
                if (index2 == -1)
                    line = newMethod.Substring(index1 + 2).Trim();
                else
                    throw new InvalidFileFormatException("invalid method declaration.");

            return line;


            //return "WriteLine";
        }

        private static string[] getParameters(string newMethod)
        {
            int index1 = newMethod.IndexOf("(");
            int index2 = newMethod.IndexOf(")");
            
            if((index2 < index1) || ((index1 == -1) && (index2 != -1)))
                throw new InvalidFileFormatException("invalid method declaration, a pair of parentheses is expected.");
            
            if(((index1 == -1) && (index2 == -1)) || (index2 == index1+1)) //no parameters
                return null;

            string line = newMethod.Substring(index1 + 1, index2 - 1 - index1);
            string[] paras = line.Split(',');
            int paraCnt = paras.Length;
            for (int i=0; i<paraCnt; i++)
                paras[i] = paras[i].Trim();

            return paras;
        }

        private static string getFullName(string method)
        {
            int index = method.IndexOf("(");
            string line = "";

            //if (index <= 0)
            //    throw new InvalidFileFormatException("invalid method declaration.");

            if (index > 0)
                line = method.Substring(0, index).Trim();
            else
                line = method.Trim();

            return line;
        }

        private static bool isSamePara(string[] para1, string[] para2)
        {
            int len1 = para1.Length;
            int len2 = para2.Length;

            if (len1 != len2)
                return false;

            for (int i = 0; i < len1; i++)
            {
                if (para2[i] != para2[i])
                    return false;
            }

            return true;
        }

        private static bool isSameMethod(string src1, string src2)
        {
            string[] para1 = getParameters(src1);
            string[] para2 = getParameters(src2);
            string name1 = getFullName(src1);
            string name2 = getFullName(src2);

            if ((name1.Contains(name2)) && (isSamePara(para1, para2)))
                return true;
            else
                return false;
        }      

        
        public static void MethodInstrument(string targetAssembly, string src, string dst)
        {
            var inputAssembly = targetAssembly;
            var path = Path.GetDirectoryName(inputAssembly);

            var assemblyResolver = new DefaultAssemblyResolver();
            var assemblyLocation = Path.GetDirectoryName(inputAssembly);
            assemblyResolver.AddSearchDirectory(assemblyLocation);

            var readerParameters = new ReaderParameters { AssemblyResolver = assemblyResolver };
            var writerParameters = new WriterParameters();
            var origPdb = Path.ChangeExtension(inputAssembly, "pdb");
            bool existpdb = false;
            if (File.Exists(origPdb))
            {
                existpdb = true;

                var symbolReaderProvider = new PdbReaderProvider();                
                readerParameters.SymbolReaderProvider = symbolReaderProvider;
                readerParameters.ReadSymbols = true;

                //var symbolWriterProvider = new PdbWriterProvider();
                //writerParameters.SymbolWriterProvider = symbolWriterProvider;
                writerParameters.WriteSymbols = true;
            }
            else
            {
                Console.WriteLine(".pdb file is unavailable.");
            }

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(inputAssembly, readerParameters);            
            var module = assemblyDefinition.MainModule;
            //Mono.Collections.Generic.Collection<ModuleDefinition> modules = assemblyDefinition.Modules;
            //var module2 = ModuleDefinition.ReadModule(inputAssembly, readerParameters);


            //----------------------
            
            bool isChanged = false;  

            //----------------------------

            foreach (var typeDefinition in module.Types)
            {
                if (typeDefinition.IsInterface)
                    continue;

                foreach (var methodDefinition in typeDefinition.Methods)
                {
                    ILProcessor ilprocessor = methodDefinition.Body.GetILProcessor();

                    Mono.Collections.Generic.Collection<Mono.Cecil.Cil.Instruction> allinstructions = methodDefinition.Body.Instructions;
                    int instructioncnt = allinstructions.Count;
                    for (int i = 0; i < instructioncnt; i++)
                    {
                        Mono.Cecil.Cil.Instruction instruction = allinstructions[i];

                        string instructpresent = instruction.ToString();

                        OpCode code = instruction.OpCode;
                        string codepresent = code.Name;

                        var operand = instruction.Operand;
                        string methodname = "";                        
                        if (operand != null)
                        {
                            methodname = operand.ToString(); //with parameter
                        }
 
                        if ( (code == OpCodes.Callvirt || code == OpCodes.Call) //codepresent.Contains("call")
                             && (isSameMethod(methodname, src)) ) //or (operand as MethodReference == module.Import(src as MethodInfo))                         
                        {
                            Console.WriteLine(codepresent + " " + methodname); //debug

                            //---------------------- remove --------------------------//
                            /**** remove instruction in case of no replacing method is provided ****/
                            //TODO: 1. I note the stack depth after the call and start removing instructions, beginning with the Code.Call 
                                //  and moving backward until I get to that stack depth again.
                                //  2. return value handling

                            ilprocessor.Remove(instruction);                            
                            instructioncnt--;
                            if (allinstructions[i].OpCode == OpCodes.Pop) //TODO: just for test
                            {
                                ilprocessor.Remove(allinstructions[i]);
                                instructioncnt--;
                            }
                            i--;
                                
                            if (dst == "")
                                continue;

                            //---------------------- replace (remove + insert) ------------------------//

                            /**** create new MethodInfo ****/
                            string m_type = getNamespace(dst);  
                            string m_name = getMethodName(dst);
                            string [] m_para = getParameters(dst);
                            MethodInfo writeLineMethod;

                            //e.g. MethodInfo writeLineMethod = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) });
                            if (m_para != null)
                            {
                                int paraCnt = m_para.Length;
                                Type[] m_para_type = new Type[paraCnt];
                                for (int k = 0; k < paraCnt; k++)
                                    m_para_type[k] = Type.GetType(m_para[k]);
                                 writeLineMethod = Type.GetType(m_type).GetMethod(m_name, m_para_type); 
                            }
                            else //the method does not have input parameters
                                writeLineMethod = Type.GetType(m_type).GetMethod(m_name, new Type[] { }); 

                            if (writeLineMethod == null)
                                throw new InvalidFileFormatException("no MethodInfo is created -- possibly a wrong method delaration.");

                            /*** Import the new (e.g. Console.WriteLine() method) ***/
                            MethodReference writeLine;
                            writeLine = module.Import(writeLineMethod); //convert "MethodInfo/MethodDefinition" to "MethodReference"

                            /*** Creates the CIL instruction for calling the new method (e.g. Console.WriteLine(string value) method) ***/
                            Mono.Cecil.Cil.Instruction callWriteLine;                            
                            callWriteLine = ilprocessor.Create(OpCodes.Call, writeLine);

                            /*** replace old instruction ***/
                            //TODO: 1. for simple-type parameters, create one instance and push to stack; 
                                //  otherwise just a null object?!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                //  2. return value handling

                            ilprocessor.InsertAfter(allinstructions[i],callWriteLine);
                                                        
                            //----------------------------------------------//
                            
                            isChanged = true;
                        }
                    } //foreach instruction of a method
                } //foreach method of a type/class
            } //foreach type/class of loaded assembly

            if (isChanged)
            {
                var temporaryFileName = Path.Combine(path, string.Format("{0}_temp{1}", Path.GetFileNameWithoutExtension(inputAssembly),
                    Path.GetExtension(inputAssembly)));
                var backupFileName = Path.Combine(path, string.Format("{0}_orig{1}", Path.GetFileNameWithoutExtension(inputAssembly),
                    Path.GetExtension(inputAssembly)));

                var tmpPdb = Path.Combine(path, string.Format("{0}_temp{1}", Path.GetFileNameWithoutExtension(inputAssembly), ".pdb"));
                var backupPdb = Path.Combine(path, string.Format("{0}_orig{1}", Path.GetFileNameWithoutExtension(inputAssembly), ".pdb"));

                //write an assembly with symbol file being rewritten
                //module.Write(temporaryFileName, writerParameters); 
                assemblyDefinition.Write(temporaryFileName,writerParameters);

                File.Replace(temporaryFileName, inputAssembly, backupFileName);
                
                if(existpdb)
                    File.Replace(tmpPdb, origPdb, backupPdb);

                //TODO: add snkfile in configuration file and parse (xiao.qu@us.abb.com)
                // If you have access to the key pair, you can ask Cecil to sign it directly.
                //if (snkFile != "")
                //{
                    //sn -R inputAssembly snkFile
                //}

            }

        }
    }
}
