using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Randoop;


namespace RandoopTesting
{

    static class CopyDir
    {
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }

    
    [TestClass]
    public class BasicRandoopTesitng
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<string> result;
            string currentpath = System.IO.Directory.GetCurrentDirectory();
            string rootpath = currentpath.Substring(0, currentpath.IndexOf("Randoop plugin src-2010"));
            string path = string.Concat(rootpath, "Randoop plugin src-2010\\Randoop\\RandoopAddinTesting\\TestingFiles\\RandoopTestingFiles");
            result = FileHelper.GetFilesRecursive(path);
            Assert.IsTrue(result.Count() == 1);
            //Assert.IsTrue(result[0].Equals("C:\\TestingFiles\\RandoopTestingFiles\\file.txt"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            string path = @"C:\TestingFiles\RandoopTestingStation";
            string currentpath = System.IO.Directory.GetCurrentDirectory();
            string rootpath = currentpath.Substring(0, currentpath.IndexOf("Randoop plugin src-2010"));
            string path0 = string.Concat(rootpath, "Randoop plugin src-2010\\Randoop\\RandoopAddinTesting\\TestingFiles\\RandoopTestingStation-org");
            if(Directory.Exists(path))
                Directory.Delete(path, true);
            CopyDir.Copy(path0, path);

            toMSTest obj = new toMSTest();
            obj.Convert(path, 500);        
            List<string> result = FileHelper.GetFilesRecursive(path);
            Assert.IsTrue(result.Contains("C:\\TestingFiles\\RandoopTestingStation\\RandoopTest.cs"));
        }
    }
}
