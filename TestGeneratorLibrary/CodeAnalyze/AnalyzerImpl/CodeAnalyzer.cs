using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CompilationUnitSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax;

namespace TestGeneratorLibrary.CodeAnalyze.AnalyzerImpl
{
    public class CodeAnalyzer : IAnalyzer
    {
        private List<string> CodeMethods;
        public CodeAnalyzer()
        {
            CodeMethods = new List<string>();
        }
        public FileInfo AnalyzeFile(string analyzableCode)
        {
            CompilationUnitSyntax root = CSharpSyntaxTree.ParseText(analyzableCode).GetCompilationUnitRoot();
            var classes = new List<string>();
            foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                
            }
            return null;
        }
    }
}