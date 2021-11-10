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
}