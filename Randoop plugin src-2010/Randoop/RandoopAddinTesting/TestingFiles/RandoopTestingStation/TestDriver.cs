using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TestDriver
{
    [STAThread]
    static void Main()
    {
        RandoopTestGeneral tests = new RandoopTestGeneral();
        tests.TestMethod1();
        tests.TestMethod2();
        tests.TestMethod3();
    }
}
