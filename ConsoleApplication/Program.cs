using System;
using TestGeneratorLibrary.DataFlow;

namespace ConsoleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var filesName = new string[] { "Class1.cs" };
            
            new PipeLine().Generate(@"..\..\TestedFiles", filesName, @"..\..\ResultedFiles", 2);
            Console.ReadLine();
        }
    }
}