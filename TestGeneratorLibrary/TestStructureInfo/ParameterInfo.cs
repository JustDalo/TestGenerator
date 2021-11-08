using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGeneratorLibrary.TestStructureInfo
{
    public class ParameterInfo
    {
        public string ParameterType;
        public string ParameterName;

        public ParameterInfo(string parameterType, string parameterName)
        {
            ParameterType = parameterType;
            ParameterName = parameterName;
        }
    }
}