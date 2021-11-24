

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGeneratorLibrary.TestStructureInfo;

namespace TestGeneratorLibrary.CodeAnalyze.AnalyzerImpl
{
    public class CodeAnalyzer : IAnalyzer
    {
        public FileInfo AnalyzeFile(string analyzableCode)
        {
            CompilationUnitSyntax root = CSharpSyntaxTree.ParseText(analyzableCode).GetCompilationUnitRoot();
            var classes = new List<ClassInfo>();
            foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                classes.Add(GetClassMethods(classDeclaration));
            }
            return new FileInfo(classes);
        }

        private ClassInfo GetClassMethods(ClassDeclarationSyntax classDeclaration)
        {
            var methods = new List<MethodInfo>();
            foreach (var methodDeclarationSyntax in classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where((methodDeclaration) => methodDeclaration.Modifiers.Any((modifier) 
                => modifier.IsKind(SyntaxKind.PublicKeyword))))
            {
                methods.Add(GetMethodInfo(methodDeclarationSyntax));
            }

            var constructors = new List<ConstructorInfo>();
            foreach (var constructorDeclarationSyntax in classDeclaration.DescendantNodes().OfType<ConstructorDeclarationSyntax>())
            {   
                constructors.Add(GetConstructorInfo(constructorDeclarationSyntax));
            }
            return new ClassInfo(methods, constructors, classDeclaration.Identifier.Text);
        }

        private MethodInfo GetMethodInfo(MethodDeclarationSyntax method)
        {
            var parameters = new List<ParameterInfo>();
            foreach (var parameter in method.ParameterList.Parameters)
            {
                parameters.Add(new ParameterInfo(parameter.Type?.ToString(), parameter.Identifier.ValueText));
            }
            return new MethodInfo(parameters, method.Identifier.ValueText, method.ReturnType.ToString());
        }

        private ConstructorInfo GetConstructorInfo(ConstructorDeclarationSyntax constructor)
        {
            var parameters = new List<ParameterInfo>();
            foreach (var parameter in constructor.ParameterList.Parameters)
            {
                parameters.Add(new ParameterInfo(parameter.Type?.ToString(), parameter.Identifier.ValueText));
            }
            return new ConstructorInfo(parameters, constructor.Identifier.Text);
        }
        
        
    }
}