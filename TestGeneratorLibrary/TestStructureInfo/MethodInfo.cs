using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGeneratorLibrary.TestStructureInfo
{
    public class MethodInfo
    {
        public string Name { get; private set; }
        public List<ParameterInfo> Parameters { get; private set; }
        public string ReturnType { get; private set; }

        public MethodInfo(List<ParameterInfo> parameters, string name, string returnType)
        {
            this.ReturnType = returnType;
            this.Name = name;
            this.Parameters = parameters;
        }
    }
}