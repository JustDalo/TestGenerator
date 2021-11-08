using System.Collections.Generic;
using TestGeneratorLibrary.TestStructureInfo;


namespace TestGeneratorLibrary.TestGenerator
{
    public interface IGenerator
    {
        Dictionary<string, string> Generate(FileInfo fileInfo);
    }
}