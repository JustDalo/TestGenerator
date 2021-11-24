using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TestGeneratorLibrary.CodeAnalyze;
using TestGeneratorLibrary.CodeAnalyze.AnalyzerImpl;
using TestGeneratorLibrary.TestGenerator;
using TestGeneratorLibrary.TestGenerator.TestGeneratorImpl;
using TestGeneratorLibrary.TestStructureInfo;
using FileInfo = TestGeneratorLibrary.TestStructureInfo.FileInfo;

namespace TestGeneratorLibraryTests
{
    [TestFixture]
    public class Tests
    {
        private IGenerator _testGenerator;
        private IAnalyzer _codeAnalyzer;

        [SetUp]
        public void SetUp()
        {
            var path = @"C:\Users\ASUS\RiderProjects\MPPproject4\ConsoleApplication\TestedFiles\Class1.cs";
            string[] firstClassStringArray = File.ReadAllLines(path);
            
            var firstClassString = String.Join("\n", firstClassStringArray);
            _codeAnalyzer = new CodeAnalyzer();
            var _fileInfo = _codeAnalyzer.AnalyzeFile(firstClassString);
        }

        [Test]
        public void Test1()
        {
            // var list = new FileInfo( new List<ClassInfo>
            //     {
            //         new ClassInfo(
            //             new List<MethodInfo>()
            //             {
            //                 new MethodInfo(new List<ParameterInfo>()
            //                     {
            //                         new ParameterInfo("int", "a"),
            //                         new ParameterInfo("int", "b")
            //                     },
            //                     "MyMethod",
            //                     "void"),
            //                 new MethodInfo(new List<ParameterInfo>(),
            //                     "MyMethod1",
            //                     "object")
            //             },
            //             new List<ConstructorInfo>()
            //             {
            //                 new ConstructorInfo(new List<ParameterInfo>()
            //                     {
            //                         new ParameterInfo("string", "str")
            //                     },
            //                     "Class1")
            //             }
            //             , "Class1")
            //     });
            //
            // _testGenerator = new TestGenerator();
            // var expected = _testGenerator.Generate(list);
            // Assert.NotNull(expected);
        }
    }
}