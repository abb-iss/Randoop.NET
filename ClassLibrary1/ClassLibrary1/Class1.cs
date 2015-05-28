using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{

	public class Class1
	{
		public void cal()
		{
			Class2 objClass2 = new Class2();
			objClass2.IVal = 100;
			objClass2.JVal = 200;
			System.Console.WriteLine(CalcSum(objClass2.IVal, objClass2.JVal));

		}
		public int CalcSum(int i, int j)
		{
			return (i + j);
		}
	}
	public class Class2
	{
		private int _iVal;
		public int IVal
		{
			get
			{
				return _iVal;
			}
			set
			{
				_iVal = value;
			}
		}
		private int _jVal;
		public int JVal
		{
			get
			{
				return _jVal;
			}
			set
			{
				_jVal = value;
			}
		}
	}
}
