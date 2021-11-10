using System;
using System.Collections.Generic;

namespace ConsoleApplication.TestedFiles
{
    public class Class2
    {
        public Class1 MyClass;
        public Class2(List<int> list)
        {
            MyClass = new Class1("Hello");
        }

        public Class1 MyMethod(List<int> list)
        {
            return MyClass;
        }
    }

    public class Class3
    {
        public IEnumerable<int> Interface { get; private set; }
        public Class3(IDisposable s, ICloneable c, int a, string str)
        {
            
        }

        public int Method1(int d, int e)
        {
            return 0;
        }
        public void Method2()
        {
            
        }
    }
}