using System;
using System.Collections.Generic;
using System.Text;

using TestGeneratorLibrary.TestStructureInfo;


namespace TestGeneratorLibrary.TestGenerator.TestGeneratorImpl
{
    public class TestGenerator : IGenerator
    { 
        public Dictionary<string,string> Generate(FileInfo fileInfo)
        {
            var fileCode = new Dictionary<string, string>();
            foreach (var classDescription in fileInfo.Classes)
            {
                
                fileCode.Add(classDescription.ClassName + "Test", GenerateTests(classDescription));
            }
            return fileCode;
        }
        
        

        private string GenerateTests(ClassInfo classDescription)
        {
            var sb = new StringBuilder();
            foreach (var method in classDescription.Methods)
            {
                foreach (var methodParameter in method.Parameters)
                {
                    sb.Append(ArrangeBlock(methodParameter));
                }    
            }
            return sb.ToString();
        }

        private string LibraryUsing()
        {
            var sb = new StringBuilder();
            sb.Append(
                $@"using NUnit.Framework;
                   using Moq;
                    "
            );
            return sb.ToString();
        }

        private string ArrangeBlock(ParameterInfo parameterInfo)
        {
            var sb = new StringBuilder();
            sb.Append(
                $@"{parameterInfo.ParameterType} {parameterInfo.ParameterName} = 0;");
            return sb.ToString();
        }
        
        object GetDefaultValue(Type t)
        {
            
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
    }
}