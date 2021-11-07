using System.IO;

namespace TestGeneratorLibrary.CodeAnalyze
{
    public interface IAnalyzer
    {
        FileInfo AnalyzeFile(string analyzableCode);
    }
}