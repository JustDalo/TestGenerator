using System.Collections.Generic;

namespace TestGeneratorLibrary.TestStructureInfo
{
    public class ConstructorInfo
    {
        public string Name { get; private set; }
        public List<string> Parameters { get; private set; }

        public ConstructorInfo(List<string> parameters, string name)
        {
            this.Name = name;
            this.Parameters = parameters;
        }
    }
}