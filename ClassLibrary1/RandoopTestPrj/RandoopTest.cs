using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;


[TestClass]
public class RandoopTest
{
	[TestInitialize]
	public void Initialize()
	{
		//put your own test method initialization code.
	}

	[TestCleanup]
	public void Cleanup()
	{
		//put your own test method cleanup code.
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod1()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)0;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [278]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v6 = (System.Int32)(-1);
      System.Int32 v7 = (System.Int32)46;
      System.Int32 v8 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v6, (System.Int32)v7) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(45, v8,"Regression Failure? [279]");

      System.Int32 v9 = (System.Int32)2;
      System.Int32 v10 = (System.Int32)(-3);
      System.Int32 v11 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v9, (System.Int32)v10) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-1, v11,"Regression Failure? [280]");

      System.Int32 v12 = (System.Int32)1;
      System.Int32 v13 = (System.Int32)(-1);
      System.Int32 v14 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v12, (System.Int32)v13) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v14,"Regression Failure? [281]");

      System.Int32 v15 = (System.Int32)39;
      System.Int32 v16 = (System.Int32)46;
      System.Int32 v17 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v15, (System.Int32)v16) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(85, v17,"Regression Failure? [282]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v19 = (System.Int32)0;
      System.Int32 v20 = (System.Int32)(-3);
      System.Int32 v21 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v19, (System.Int32)v20) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v21,"Regression Failure? [283]");

      System.Int32 v22 = (System.Int32)0;
      System.Int32 v23 = (System.Int32)0;
      System.Int32 v24 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v22, (System.Int32)v23) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v24,"Regression Failure? [284]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)0 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-1) 
      (System.Int32)46 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)2 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)1 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)39 
      (System.Int32)46 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)0 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod2()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v3,"Regression Failure? [291]");

      System.Int32 v4 = (System.Int32)13;
      System.Int32 v5 = (System.Int32)10;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(23, v6,"Regression Failure? [292]");

      System.Int32 v7 = (System.Int32)0;
      System.Int32 v8 = (System.Int32)23;
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(23, v9,"Regression Failure? [293]");

      System.Int32 v10 = (System.Int32)42;
      System.Int32 v11 = (System.Int32)2;
      System.Int32 v12 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v10, (System.Int32)v11) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(44, v12,"Regression Failure? [294]");

      System.Int32 v13 = (System.Int32)45;
      System.Int32 v14 = (System.Int32)46;
      System.Int32 v15 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v13, (System.Int32)v14) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(91, v15,"Regression Failure? [295]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)13 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)23 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)42 
      (System.Int32)2 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)45 
      (System.Int32)46 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod3()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)0;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [319]");

      System.Int32 v4 = (System.Int32)(-3);
      System.Int32 v5 = (System.Int32)3;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [320]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)0 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)(-3) 
      (System.Int32)3 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod4()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v3,"Regression Failure? [321]");

      System.Int32 v4 = (System.Int32)13;
      System.Int32 v5 = (System.Int32)10;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(23, v6,"Regression Failure? [322]");

      System.Int32 v7 = (System.Int32)0;
      System.Int32 v8 = (System.Int32)(-3);
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v9,"Regression Failure? [323]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)13 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod5()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)0;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [324]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v6 = (System.Int32)(-1);
      System.Int32 v7 = (System.Int32)46;
      System.Int32 v8 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v6, (System.Int32)v7) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(45, v8,"Regression Failure? [325]");

      System.Int32 v9 = (System.Int32)2;
      System.Int32 v10 = (System.Int32)(-3);
      System.Int32 v11 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v9, (System.Int32)v10) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-1, v11,"Regression Failure? [326]");

      System.Int32 v12 = (System.Int32)1;
      System.Int32 v13 = (System.Int32)(-1);
      System.Int32 v14 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v12, (System.Int32)v13) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v14,"Regression Failure? [327]");

      System.Int32 v15 = (System.Int32)39;
      System.Int32 v16 = (System.Int32)46;
      System.Int32 v17 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v15, (System.Int32)v16) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(85, v17,"Regression Failure? [328]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v19 = (System.Int32)0;
      System.Int32 v20 = (System.Int32)(-3);
      System.Int32 v21 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v19, (System.Int32)v20) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v21,"Regression Failure? [329]");

      System.Int32 v22 = (System.Int32)0;
      System.Int32 v23 = (System.Int32)0;
      System.Int32 v24 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v22, (System.Int32)v23) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v24,"Regression Failure? [330]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)0 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-1) 
      (System.Int32)46 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)2 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)1 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)39 
      (System.Int32)46 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)0 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod6()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)0;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [337]");

      System.Int32 v4 = (System.Int32)6;
      System.Int32 v5 = (System.Int32)(-8);
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-2, v6,"Regression Failure? [338]");

      System.Int32 v7 = (System.Int32)13;
      System.Int32 v8 = (System.Int32)6;
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(19, v9,"Regression Failure? [339]");

      System.Int32 v10 = (System.Int32)(-3);
      System.Int32 v11 = (System.Int32)(-10);
      System.Int32 v12 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v10, (System.Int32)v11) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-13, v12,"Regression Failure? [340]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)0 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)(-8) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)13 
      (System.Int32)6 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)(-3) 
      (System.Int32)(-10) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod7()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)(-3);
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [362]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v6 = (System.Int32)0;
      System.Int32 v7 = (System.Int32)68;
      System.Int32 v8 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v6, (System.Int32)v7) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(68, v8,"Regression Failure? [363]");

      System.Int32 v9 = (System.Int32)0;
      System.Int32 v10 = (System.Int32)6;
      System.Int32 v11 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v9, (System.Int32)v10) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(6, v11,"Regression Failure? [364]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)0 
      (System.Int32)68 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)6 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod8()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)(-3);
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [389]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v6 = (System.Int32)92;
      System.Int32 v7 = (System.Int32)48;
      System.Int32 v8 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v6, (System.Int32)v7) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(140, v8,"Regression Failure? [390]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v10 = (System.Int32)5;
      System.Int32 v11 = (System.Int32)(-6);
      System.Int32 v12 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v10, (System.Int32)v11) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-1, v12,"Regression Failure? [391]");

      System.Int32 v13 = (System.Int32)23;
      System.Int32 v14 = (System.Int32)54;
      System.Int32 v15 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v13, (System.Int32)v14) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(77, v15,"Regression Failure? [392]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)92 
      (System.Int32)48 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)5 
      (System.Int32)(-6) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)23 
      (System.Int32)54 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod9()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v3,"Regression Failure? [398]");

      System.Int32 v4 = (System.Int32)13;
      System.Int32 v5 = (System.Int32)10;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(23, v6,"Regression Failure? [399]");

      System.Int32 v7 = (System.Int32)23;
      System.Int32 v8 = (System.Int32)23;
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(46, v9,"Regression Failure? [400]");

      System.Int32 v10 = (System.Int32)(-3);
      System.Int32 v11 = (System.Int32)10;
      System.Int32 v12 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v10, (System.Int32)v11) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v12,"Regression Failure? [401]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v14 = (System.Int32)(-10);
      System.Int32 v15 = (System.Int32)10;
      System.Int32 v16 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v14, (System.Int32)v15) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v16,"Regression Failure? [402]");

      System.Int32 v17 = (System.Int32)149;
      System.Int32 v18 = (System.Int32)61;
      System.Int32 v19 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v17, (System.Int32)v18) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(210, v19,"Regression Failure? [403]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)13 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)23 
      (System.Int32)23 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)(-3) 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-10) 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)149 
      (System.Int32)61 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod10()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)105;
      System.Int32 v2 = (System.Int32)(-1);
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(104, v3,"Regression Failure? [410]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)105 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod11()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)0;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [415]");

      System.Int32 v4 = (System.Int32)6;
      System.Int32 v5 = (System.Int32)(-8);
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-2, v6,"Regression Failure? [416]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)0 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)(-8) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination8
	[TestMethod]
	public void TestMethod12()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v3,"Regression Failure? [450]");

      System.Int32 v4 = (System.Int32)13;
      System.Int32 v5 = (System.Int32)10;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(23, v6,"Regression Failure? [451]");

      System.Int32 v7 = (System.Int32)23;
      System.Int32 v8 = (System.Int32)23;
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(46, v9,"Regression Failure? [452]");

      System.Int32 v10 = (System.Int32)(-3);
      System.Int32 v11 = (System.Int32)10;
      System.Int32 v12 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v10, (System.Int32)v11) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v12,"Regression Failure? [453]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v14 = (System.Int32)(-10);
      System.Int32 v15 = (System.Int32)10;
      System.Int32 v16 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v14, (System.Int32)v15) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v16,"Regression Failure? [454]");

      System.Int32 v17 = (System.Int32)149;
      System.Int32 v18 = (System.Int32)61;
      System.Int32 v19 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v17, (System.Int32)v18) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(210, v19,"Regression Failure? [455]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)13 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)23 
      (System.Int32)23 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)(-3) 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-10) 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)149 
      (System.Int32)61 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod13()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod14()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)(-3);
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [136]");

      System.Int32 v4 = (System.Int32)6;
      System.Int32 v5 = (System.Int32)7;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(13, v6,"Regression Failure? [137]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod15()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)0;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [138]");

      System.Int32 v4 = (System.Int32)(-3);
      System.Int32 v5 = (System.Int32)3;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [139]");

      System.Int32 v7 = (System.Int32)7;
      System.Int32 v8 = (System.Int32)(-13);
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-6, v9,"Regression Failure? [140]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)0 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)(-3) 
      (System.Int32)3 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)7 
      (System.Int32)(-13) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod16()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)0;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(7, v3,"Regression Failure? [141]");

      System.Int32 v4 = (System.Int32)6;
      System.Int32 v5 = (System.Int32)(-8);
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-2, v6,"Regression Failure? [142]");

      System.Int32 v7 = (System.Int32)3;
      System.Int32 v8 = (System.Int32)45;
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(48, v9,"Regression Failure? [143]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)0 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)(-8) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)3 
      (System.Int32)45 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod17()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v4 = (System.Int32)24;
      System.Int32 v5 = (System.Int32)2;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(26, v6,"Regression Failure? [188]");

      System.Int32 v7 = (System.Int32)19;
      System.Int32 v8 = (System.Int32)85;
      System.Int32 v9 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v7, (System.Int32)v8) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(104, v9,"Regression Failure? [189]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)24 
      (System.Int32)2 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)19 
      (System.Int32)85 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod18()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v3,"Regression Failure? [197]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod19()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = (System.Int32)17;
      System.Int32 v4 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v2, (System.Int32)v3) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(24, v4,"Regression Failure? [233]");

      System.Int32 v5 = (System.Int32)17;
      System.Int32 v6 = (System.Int32)0;
      System.Int32 v7 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v5, (System.Int32)v6) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v7,"Regression Failure? [234]");

      System.Int32 v8 = (System.Int32)0;
      System.Int32 v9 = (System.Int32)(-1);
      System.Int32 v10 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v8, (System.Int32)v9) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-1, v10,"Regression Failure? [235]");

      System.Int32 v11 = (System.Int32)6;
      System.Int32 v12 = (System.Int32)10;
      System.Int32 v13 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v11, (System.Int32)v12) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(16, v13,"Regression Failure? [236]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)7 
      (System.Int32)17 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)17 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod20()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)10;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v3,"Regression Failure? [237]");

      System.Int32 v4 = (System.Int32)13;
      System.Int32 v5 = (System.Int32)10;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(23, v6,"Regression Failure? [238]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v8 = (System.Int32)(-10);
      System.Int32 v9 = (System.Int32)(-3);
      System.Int32 v10 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v8, (System.Int32)v9) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-13, v10,"Regression Failure? [239]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v13 = (System.Int32)24;
      System.Int32 v14 = (System.Int32)(-1);
      System.Int32 v15 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v13, (System.Int32)v14) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(23, v15,"Regression Failure? [240]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)10 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)13 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-10) 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)24 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod21()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v4 = (System.Int32)(-8);
      System.Int32 v5 = (System.Int32)7;
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-1, v6,"Regression Failure? [245]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v8 = (System.Int32)10;
      System.Int32 v9 = (System.Int32)44;
      System.Int32 v10 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v8, (System.Int32)v9) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(54, v10,"Regression Failure? [246]");

      System.Int32 v11 = (System.Int32)105;
      System.Int32 v12 = (System.Int32)44;
      System.Int32 v13 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v11, (System.Int32)v12) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(149, v13,"Regression Failure? [247]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-8) 
      (System.Int32)7 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)10 
      (System.Int32)44 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)105 
      (System.Int32)44 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination7
	[TestMethod]
	public void TestMethod22()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v2 = (System.Int32)7;
      System.Int32 v3 = (System.Int32)17;
      System.Int32 v4 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v2, (System.Int32)v3) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(24, v4,"Regression Failure? [250]");

      System.Int32 v5 = (System.Int32)17;
      System.Int32 v6 = (System.Int32)0;
      System.Int32 v7 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v5, (System.Int32)v6) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(17, v7,"Regression Failure? [251]");

      System.Int32 v8 = (System.Int32)0;
      System.Int32 v9 = (System.Int32)(-1);
      System.Int32 v10 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v8, (System.Int32)v9) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-1, v10,"Regression Failure? [252]");

      System.Int32 v11 = (System.Int32)6;
      System.Int32 v12 = (System.Int32)10;
      System.Int32 v13 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v11, (System.Int32)v12) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(16, v13,"Regression Failure? [253]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)7 
      (System.Int32)17 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)17 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)10 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod23()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [65]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [22]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [66]");

      System.Int32 v6 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod24()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [68]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [23]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [69]");

      System.Int32 v6 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [70]");

      System.Int32 v7 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v7 ;
      System.Int32 v9 = (System.Int32)(-2);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v9 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-2) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod25()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)(-2);
      System.Int32 v2 = (System.Int32)(-1);
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v3,"Regression Failure? [24]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v5 = (System.Int32)3;
      System.Int32 v6 = (System.Int32)2;
      System.Int32 v7 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v5, (System.Int32)v6) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(5, v7,"Regression Failure? [25]");

      System.Int32 v8 = (System.Int32)5;
      System.Int32 v9 = (System.Int32)0;
      System.Int32 v10 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v8, (System.Int32)v9) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(5, v10,"Regression Failure? [26]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)(-2) 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)3 
      (System.Int32)2 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)5 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod26()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)(-2);
      System.Int32 v2 = (System.Int32)(-1);
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v3,"Regression Failure? [27]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v5 = (System.Int32)3;
      System.Int32 v6 = (System.Int32)2;
      System.Int32 v7 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v5, (System.Int32)v6) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(5, v7,"Regression Failure? [28]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)(-2) 
      (System.Int32)(-1) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)3 
      (System.Int32)2 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod27()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [87]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [31]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [88]");

      System.Int32 v6 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v6 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod28()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v3 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod29()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [93]");

      System.Int32 v2 = (System.Int32)(-6);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)8;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-2);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v6 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-6) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)8 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)(-2) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod30()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [94]");

      System.Int32 v2 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v2,"Regression Failure? [33]");

      System.Int32 v3 = (System.Int32)10;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v3 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)10 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod31()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v3 ;
      System.Int32 v5 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v5,"Regression Failure? [34]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod32()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [100]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = (System.Int32)10;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v10 ;
      System.Int32 v12 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v12 ;
      System.Int32 v14 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(10, v14,"Regression Failure? [38]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)10 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod33()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [101]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = (System.Int32)10;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v10 ;
      System.Int32 v12 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(10, v12,"Regression Failure? [39]");

      System.Int32 v13 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v13,"Regression Failure? [102]");

      System.Int32 v14 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(10, v14,"Regression Failure? [40]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)10 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod34()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v3 ;
      System.Int32 v5 = (System.Int32)(-6);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v5 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)(-6) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod35()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [103]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [41]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod36()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [110]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v8,"Regression Failure? [45]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod37()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [122]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [50]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [123]");

      System.Int32 v6 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [51]");

      System.Int32 v7 = (System.Int32)2;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v7 ;
      System.Int32 v9 = (System.Int32)10;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v9 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)2 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)10 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod38()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [124]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v10 ;
      System.Int32 v12 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(1, v12,"Regression Failure? [52]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod39()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [125]");

      System.Int32 v2 = (System.Int32)(-6);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)8;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)8;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(8, v8,"Regression Failure? [53]");

      System.Int32 v9 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(8, v9,"Regression Failure? [54]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-6) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)8 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)8 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod40()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [126]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v10,"Regression Failure? [127]");

      System.Int32 v11 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(1, v11,"Regression Failure? [55]");

      System.Int32 v12 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v12 ;
      System.Int32 v14 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v14,"Regression Failure? [128]");

      System.Int32 v15 = (System.Int32)5;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v15 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)5 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod41()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v2 = (System.Int32)0;
      System.Int32 v3 = (System.Int32)(-10);
      System.Int32 v4 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v2, (System.Int32)v3) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-10, v4,"Regression Failure? [51]");

      System.Int32 v5 = (System.Int32)4;
      System.Int32 v6 = (System.Int32)0;
      System.Int32 v7 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v5, (System.Int32)v6) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(4, v7,"Regression Failure? [52]");

      System.Int32 v8 = (System.Int32)6;
      System.Int32 v9 = (System.Int32)0;
      System.Int32 v10 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v8, (System.Int32)v9) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(6, v10,"Regression Failure? [53]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)0 
      (System.Int32)(-10) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)4 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod42()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [132]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [59]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [133]");

      System.Int32 v6 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [60]");

      System.Int32 v7 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v7,"Regression Failure? [134]");

      System.Int32 v8 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v8 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod43()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v3 ;
      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [135]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod44()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v2 = (System.Int32)(-6);
      System.Int32 v3 = (System.Int32)0;
      System.Int32 v4 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v2, (System.Int32)v3) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-6, v4,"Regression Failure? [57]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-6) 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod45()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v3 = (System.Int32)10;
      System.Int32 v4 = (System.Int32)0;
      System.Int32 v5 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v3, (System.Int32)v4) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(10, v5,"Regression Failure? [58]");

      System.Int32 v6 = (System.Int32)(-10);
      System.Int32 v7 = (System.Int32)0;
      System.Int32 v8 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v6, (System.Int32)v7) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-10, v8,"Regression Failure? [59]");

      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v11 = (System.Int32)4;
      System.Int32 v12 = (System.Int32)2;
      System.Int32 v13 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v11, (System.Int32)v12) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(6, v13,"Regression Failure? [60]");

      System.Int32 v14 = (System.Int32)6;
      System.Int32 v15 = (System.Int32)(-10);
      System.Int32 v16 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v14, (System.Int32)v15) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-4, v16,"Regression Failure? [61]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)10 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)(-10) 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)4 
      (System.Int32)2 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)6 
      (System.Int32)(-10) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod46()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [139]");

      System.Int32 v2 = (System.Int32)(-6);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)8;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)8;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(8, v8,"Regression Failure? [63]");

      System.Int32 v9 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(8, v9,"Regression Failure? [64]");

      System.Int32 v10 = (System.Int32)10;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v10 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-6) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)8 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)8 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)10 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod47()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [145]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v10,"Regression Failure? [146]");

      System.Int32 v11 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(1, v11,"Regression Failure? [69]");

      System.Int32 v12 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v12 ;
      System.Int32 v14 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v14,"Regression Failure? [147]");

      System.Int32 v15 = (System.Int32)5;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v15 ;
      System.Int32 v17 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(5, v17,"Regression Failure? [70]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)5 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod48()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      System.Int32 v1 = (System.Int32)(-3);
      System.Int32 v2 = (System.Int32)1;
      System.Int32 v3 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v1, (System.Int32)v2) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-2, v3,"Regression Failure? [64]");

      System.Int32 v4 = (System.Int32)(-3);
      System.Int32 v5 = (System.Int32)(-10);
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-13, v6,"Regression Failure? [65]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      (System.Int32)(-3) 
      (System.Int32)1 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)(-3) 
      (System.Int32)(-10) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod49()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v3 ;
      System.Int32 v5 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [71]");

      System.Int32 v6 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [72]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod50()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [151]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [75]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [152]");

      System.Int32 v6 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)5;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)5 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod51()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [157]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v10 ;
      System.Int32 v12 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(1, v12,"Regression Failure? [77]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod52()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [158]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v10,"Regression Failure? [159]");

      System.Int32 v11 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(1, v11,"Regression Failure? [78]");

      System.Int32 v12 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v12 ;
      System.Int32 v14 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v14,"Regression Failure? [160]");

      System.Int32 v15 = (System.Int32)5;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v15 ;
      System.Int32 v17 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(5, v17,"Regression Failure? [79]");

      System.Int32 v18 = (System.Int32)4;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v18 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)5 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)4 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod53()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [161]");

      System.Int32 v2 = (System.Int32)(-6);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [80]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-6) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod54()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [162]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v4 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod55()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v3,"Regression Failure? [174]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod56()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [176]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [87]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [177]");

      System.Int32 v6 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [88]");

      System.Int32 v7 = (System.Int32)2;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v7 ;
      System.Int32 v9 = (System.Int32)(-2);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v9 ;
      System.Int32 v11 = (System.Int32)10;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v11 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)2 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)(-2) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)10 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod57()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [181]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v10,"Regression Failure? [182]");

      System.Int32 v11 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(1, v11,"Regression Failure? [89]");

      System.Int32 v12 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v12 ;
      System.Int32 v14 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v14,"Regression Failure? [183]");

      System.Int32 v15 = (System.Int32)5;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v15 ;
      System.Int32 v17 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v17,"Regression Failure? [184]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)5 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod58()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v3 = (System.Int32)3;
      System.Int32 v4 = (System.Int32)0;
      System.Int32 v5 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v3, (System.Int32)v4) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(3, v5,"Regression Failure? [75]");

      System.Int32 v6 = (System.Int32)2;
      System.Int32 v7 = (System.Int32)0;
      System.Int32 v8 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v6, (System.Int32)v7) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(2, v8,"Regression Failure? [76]");

      System.Int32 v9 = (System.Int32)0;
      System.Int32 v10 = (System.Int32)(-6);
      System.Int32 v11 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v9, (System.Int32)v10) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-6, v11,"Regression Failure? [77]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v13 = (System.Int32)(-3);
      System.Int32 v14 = (System.Int32)3;
      System.Int32 v15 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v13, (System.Int32)v14) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v15,"Regression Failure? [78]");

      ((ClassLibrary1.Class1)v0).cal() ;
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)3 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)2 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)(-6) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-3) 
      (System.Int32)3 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod59()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [190]");

      System.Int32 v2 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v2,"Regression Failure? [191]");

      System.Int32 v3 = (System.Int32)5;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v3 ;
      System.Int32 v5 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v5 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)5 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod60()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [195]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [93]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [196]");

      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-3, v10,"Regression Failure? [197]");

      System.Int32 v11 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v11 ;
      System.Int32 v13 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v13,"Regression Failure? [94]");

      System.Int32 v14 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v14 ;
      System.Int32 v16 = (System.Int32)2;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v16 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)2 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod61()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [198]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [95]");

      System.Int32 v7 = (System.Int32)2;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v7 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      (System.Int32)2 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod62()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [199]");

      System.Int32 v2 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v2,"Regression Failure? [200]");

      System.Int32 v3 = (System.Int32)4;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v3 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)4 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod63()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [204]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [96]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [205]");

      System.Int32 v6 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [97]");

      System.Int32 v7 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v7,"Regression Failure? [206]");

      System.Int32 v8 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v8 ;
      System.Int32 v10 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v10,"Regression Failure? [207]");

      System.Int32 v11 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v11,"Regression Failure? [98]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod64()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [208]");

      System.Int32 v2 = (System.Int32)(-6);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)8;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v4 ;
      System.Int32 v6 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-6, v6,"Regression Failure? [209]");

      System.Int32 v7 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(8, v7,"Regression Failure? [99]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)(-6) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)8 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod65()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v3 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination6
	[TestMethod]
	public void TestMethod66()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v3 = (System.Int32)3;
      System.Int32 v4 = (System.Int32)0;
      System.Int32 v5 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v3, (System.Int32)v4) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(3, v5,"Regression Failure? [87]");

      System.Int32 v6 = (System.Int32)2;
      System.Int32 v7 = (System.Int32)0;
      System.Int32 v8 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v6, (System.Int32)v7) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(2, v8,"Regression Failure? [88]");

      System.Int32 v9 = (System.Int32)0;
      System.Int32 v10 = (System.Int32)(-6);
      System.Int32 v11 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v9, (System.Int32)v10) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-6, v11,"Regression Failure? [89]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v13 = (System.Int32)(-3);
      System.Int32 v14 = (System.Int32)3;
      System.Int32 v15 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v13, (System.Int32)v14) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v15,"Regression Failure? [90]");

      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v17 = (System.Int32)0;
      System.Int32 v18 = (System.Int32)3;
      System.Int32 v19 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v17, (System.Int32)v18) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(3, v19,"Regression Failure? [91]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)3 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)2 
      (System.Int32)0 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      (System.Int32)0 
      (System.Int32)(-6) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-3) 
      (System.Int32)3 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)0 
      (System.Int32)3 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod67()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      ((ClassLibrary1.Class1)v0).cal() ;
      System.Int32 v4 = (System.Int32)(-3);
      System.Int32 v5 = (System.Int32)(-3);
      System.Int32 v6 = ((ClassLibrary1.Class1)v0).CalcSum((System.Int32)v4, (System.Int32)v5) ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-6, v6,"Regression Failure? [9]");

      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      ClassLibrary1.Class1.Void cal() 
      (System.Int32)(-3) 
      (System.Int32)(-3) 
      ClassLibrary1.Class1.Int32 CalcSum(Int32, Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod68()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod69()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [35]");

      System.Int32 v2 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v2,"Regression Failure? [13]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod70()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [36]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v2 ;
      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v4,"Regression Failure? [14]");

      System.Int32 v5 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v5,"Regression Failure? [15]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod71()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v3,"Regression Failure? [39]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod72()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [51]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v6 ;
      System.Int32 v8 = (System.Int32)1;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v8 ;
      System.Int32 v10 = (System.Int32)(-3);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v10 ;
      System.Int32 v12 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v12 ;
      System.Int32 v14 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(1, v14,"Regression Failure? [17]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)1 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)(-3) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod73()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v3,"Regression Failure? [55]");

      System.Int32 v4 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-1, v4,"Regression Failure? [19]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod74()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [57]");

      System.Int32 v2 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v2,"Regression Failure? [58]");

      System.Int32 v3 = (System.Int32)5;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v3 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)5 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination4
	[TestMethod]
	public void TestMethod75()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v1,"Regression Failure? [60]");

      System.Int32 v2 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v2 ;
      System.Int32 v4 = (System.Int32)0;
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v4 ;
      System.Int32 v6 = ((ClassLibrary1.Class2)v0).IVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v6,"Regression Failure? [61]");

      System.Int32 v7 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(0, v7,"Regression Failure? [20]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      ClassLibrary1.Class2.Int32 get_IVal() 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      (System.Int32)0 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      ClassLibrary1.Class2.Int32 get_IVal() 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination1
	[TestMethod]
	public void TestMethod76()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination1
	[TestMethod]
	public void TestMethod77()
	{
      //BEGIN TEST
      ClassLibrary1.Class1 v0 =  new ClassLibrary1.Class1();
      /*
      ClassLibrary1.Class1 constructor Void .ctor() 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination1
	[TestMethod]
	public void TestMethod78()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination1
	[TestMethod]
	public void TestMethod79()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-1);
      ((ClassLibrary1.Class2)v0).IVal = (System.Int32)v1 ;
      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-1) 
      ClassLibrary1.Class2.Void set_IVal(Int32) 
      */
      //END TEST
      return;
	}


	// Test Case Type: normaltermination1
	[TestMethod]
	public void TestMethod80()
	{
      //BEGIN TEST
      ClassLibrary1.Class2 v0 =  new ClassLibrary1.Class2();
      System.Int32 v1 = (System.Int32)(-10);
      ((ClassLibrary1.Class2)v0).JVal = (System.Int32)v1 ;
      System.Int32 v3 = ((ClassLibrary1.Class2)v0).JVal ;
      //Regression assertion (captures the current behavior of the code)
      Assert.AreEqual<int>(-10, v3,"Regression Failure? [58]");

      /*
      ClassLibrary1.Class2 constructor Void .ctor() 
      (System.Int32)(-10) 
      ClassLibrary1.Class2.Void set_JVal(Int32) 
      ClassLibrary1.Class2.Int32 get_JVal() 
      */
      //END TEST
      return;
	}

}
