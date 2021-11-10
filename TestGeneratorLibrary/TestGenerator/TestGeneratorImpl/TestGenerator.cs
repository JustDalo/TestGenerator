using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using TestGeneratorLibrary.TestStructureInfo;


namespace TestGeneratorLibrary.TestGenerator.TestGeneratorImpl
{
    public class TestGenerator : IGenerator
    {
        private readonly SyntaxGenerator _generator;

        public TestGenerator()
        {
            _generator = SyntaxGenerator.GetGenerator(new AdhocWorkspace(), LanguageNames.CSharp);
        }

        public Dictionary<string, string> Generate(FileInfo fileInfo)
        {
            var fileCode = new Dictionary<string, string>();
            foreach (var classInfo in fileInfo.Classes)
            {
                var newNode = _generator.CompilationUnit(GetUsingList(classInfo)).NormalizeWhitespace().ToFullString();
                fileCode.Add(classInfo.ClassName + "Test", newNode);
            }

            return fileCode;
        }

        private List<SyntaxNode> GetUsingList(ClassInfo classInfo)
        {
            List<SyntaxNode> listOfUses = new List<SyntaxNode>
            {
                _generator.NamespaceImportDeclaration("System"),
                _generator.NamespaceImportDeclaration("NUnit.Framework"),
                _generator.NamespaceImportDeclaration("System.Collections.Generic"),
                _generator.NamespaceImportDeclaration("System.Linq"),
                _generator.NamespaceImportDeclaration("TestGeneratorLibrary"),

                _generator.NamespaceDeclaration(classInfo.ClassName + "Test", CreateClassDefinition(classInfo)),
            };
            return listOfUses;
        }

        private SyntaxNode CreateClassDefinition(ClassInfo classInfo)
        {
            var classAttribute = _generator.Attribute("TestFixture");
            var classDefinition = _generator.ClassDeclaration(
                classInfo.ClassName,
                typeParameters: null,
                accessibility: Accessibility.Public,
                modifiers: DeclarationModifiers.None,
                baseType: null,
                interfaceTypes: null,
                members: CreateTestMethods(classInfo));
            var classDefinitionAttribute = _generator.AddAttributes(classDefinition, classAttribute);
            return classDefinitionAttribute;
        }

        private List<SyntaxNode> CreateTestMethods(ClassInfo classInfo)
        {
            List<SyntaxNode> testMethods = new List<SyntaxNode>();
            foreach (var method in classInfo.Methods)
            {
                var attribute = _generator.Attribute("Test");
                SyntaxNode methodTest = _generator.MethodDeclaration(
                    method.Name + "Test",
                    null,
                    null,
                    null,
                    Accessibility.Public,
                    DeclarationModifiers.None,
                    GetMethodBody(method)
                );
                var methodTestAttribute = _generator.AddAttributes(methodTest, attribute);
                testMethods.Add(methodTestAttribute);
            }

            return testMethods;
        }

        private List<SyntaxNode> GetMethodBody(MethodInfo methodInfo)
        {
            var methodStatements = new List<SyntaxNode>();
            methodStatements.AddRange(GetMethodArrangeBlock(methodInfo));
            methodStatements.Add(GetMethodActBlock(methodInfo));
            methodStatements.AddRange(GetMethodAssertBlock());
            return methodStatements;
        }

        private List<SyntaxNode> GetMethodArrangeBlock(MethodInfo methodInfo)
        {
            var fields = new List<SyntaxNode>();
            foreach (var field in methodInfo.Parameters)
            {
                var fieldDeclaration =
                    _generator.IdentifierName(
                        $@"{field.ParameterType} {field.ParameterName} = default({field.ParameterType})");
                fields.Add(fieldDeclaration);
            }

            return fields;
        }

        private SyntaxNode GetMethodActBlock(MethodInfo methodInfo)
        {
            var parameterList = new StringBuilder();
            if (methodInfo.Parameters.Capacity != 0)
            {
                ParameterInfo last = methodInfo.Parameters.Last();


                foreach (var parameter in methodInfo.Parameters)
                {
                    if (parameter.Equals(last))
                    {
                        parameterList.Append(parameter.ParameterName);
                    }
                    else
                    {
                        parameterList.Append(parameter.ParameterName + ", ");
                    }
                }
            }

            if (methodInfo.ReturnType == "void")
            {
                return _generator.IdentifierName($@"{methodInfo.Name}({parameterList})");
            }
            return _generator.IdentifierName(
                $@"{methodInfo.ReturnType} actual = {methodInfo.Name}({parameterList})");
        }

        private List<SyntaxNode> GetMethodAssertBlock()
        {
            var assertBlock = new List<SyntaxNode>()
            {
                _generator.IdentifierName("int expected = 0"),
                _generator.IdentifierName("Assert.That(actual, Is.EqualTo(expected))"),
                _generator.IdentifierName("Assert.Fail(\"autogenerated\")"),
            };

            return assertBlock;
        }
    }
}