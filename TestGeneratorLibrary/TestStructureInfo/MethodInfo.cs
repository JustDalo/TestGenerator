using System.Collections.Generic;

namespace TestGeneratorLibrary.TestStructureInfo
{
    public class MethodInfo
    {
        public string Name { get; private set; }
        public List<string> Parameters { get; private set; }
        public string ReturnType { get; private set; }

        public MethodInfo(List<string> parameters, string name, string returnType)
        {
            this.ReturnType = returnType;
            this.Name = name;
            this.Parameters = parameters;
        }
    }
}