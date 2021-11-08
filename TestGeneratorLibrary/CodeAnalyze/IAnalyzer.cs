using TestGeneratorLibrary.TestStructureInfo;

namespace TestGeneratorLibrary.CodeAnalyze
{
    public interface IAnalyzer
    {
        FileInfo AnalyzeFile(string analyzableCode);
    }
}