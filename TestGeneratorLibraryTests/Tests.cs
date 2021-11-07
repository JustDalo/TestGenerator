using System;
using NUnit.Framework;
using TestGeneratorLibrary;

namespace TestGeneratorLibraryTests
 {
    [TestFixture]
    public class Tests
    {
        private string classOneName;
        private string classTwoName;
        
        private object generatedTests;
        [SetUp]
        public void TestInit()
        {
            string directoryPath = "";

            classOneName = "";
            classTwoName = "";
        }
        [Test]
        public void Test1()
        {
            
        }
    }
}