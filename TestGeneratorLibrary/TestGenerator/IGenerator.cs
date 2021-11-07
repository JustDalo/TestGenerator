using System.Collections.Generic;
using System.IO;

namespace TestGeneratorLibrary.TestGenerator
{
    public interface IGenerator
    {
        Dictionary<string, string> Generate(FileInfo fileInfo);
    }
}