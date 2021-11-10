using System.Collections.Generic;
using System.Linq;
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
            var constructor = classInfo.Constructors.Last();
            var classBody = new List<SyntaxNode>();
            classBody.AddRange(CreateTestFields(constructor));
            classBody.Add(CreateSetUpMethod(constructor));
            classBody.AddRange(CreateTestMethods(classInfo));

            var classAttribute = _generator.Attribute("TestFixture");
            var classDefinition = _generator.ClassDeclaration(
                classInfo.ClassName + "Test",
                typeParameters: null,
                accessibility: Accessibility.Public,
                modifiers: DeclarationModifiers.None,
                baseType: null,
                interfaceTypes: null,
                members: classBody);
            var classDefinitionAttribute = _generator.AddAttributes(classDefinition, classAttribute);
            return classDefinitionAttribute;
        }

        private SyntaxNode CreateSetUpMethod(ConstructorInfo constructor)
        {
            var attribute = _generator.Attribute("SetUp");
            var setUpMethod = _generator.MethodDeclaration(
                "SetUp",
                null,
                null,
                null,
                Accessibility.Public,
                DeclarationModifiers.None,
                GetSetUpBody(constructor));
            var setUpMethodAttribute = _generator.AddAttributes(setUpMethod, attribute);
            return setUpMethodAttribute;
        }

        private List<SyntaxNode> GetSetUpBody(ConstructorInfo constructor)
        {
            var setUpBodyFields = new List<SyntaxNode>();
            var parameters = new StringBuilder();
            ParameterInfo last = constructor.Parameters.Last();
            foreach (var parameter in constructor.Parameters)
            {
                if (parameter.ParameterType[0] == 'I')
                {
                    setUpBodyFields.Add(
                        _generator.IdentifierName(
                            $@"_{parameter.ParameterName} = new Mock<{parameter.ParameterType}>()"));
                    parameters.Append(parameter.Equals(last)
                        ? $@"_{parameter.ParameterName}.object"
                        : $@"_{parameter.ParameterName}.object, ");
                }
                else
                {
                    setUpBodyFields.Add(_generator.IdentifierName(
                        $@"var {parameter.ParameterName} = default({parameter.ParameterType})"));
                    parameters.Append(parameter.Equals(last)
                        ? $@"{parameter.ParameterName}"
                        : $@"{parameter.ParameterName}, ");
                }
            }

            setUpBodyFields.Add(
                _generator.IdentifierName($@"_{constructor.Name.ToLower()} = new {constructor.Name}({parameters})"));
            return setUpBodyFields;
        }

        private List<SyntaxNode> CreateTestFields(ConstructorInfo constructor)
        {
            var testFields = new List<SyntaxNode>();
            foreach (var parameter in constructor.Parameters)
            {
                if (parameter.ParameterType[0] == 'I')
                {
                    testFields.Add(_generator.FieldDeclaration(
                        "_" + parameter.ParameterName,
                        _generator.IdentifierName($@"Mock<{parameter.ParameterType}>"),
                        Accessibility.Private
                    ));
                }
            }

            testFields.Add(_generator.FieldDeclaration(
                "_" + constructor.Name.ToLower(),
                _generator.IdentifierName($@"{constructor.Name}"),
                Accessibility.Private)
            );

            return testFields;
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
                    GetMethodBody(classInfo, method)
                );
                var methodTestAttribute = _generator.AddAttributes(methodTest, attribute);
                testMethods.Add(methodTestAttribute);
            }

            return testMethods;
        }

        private List<SyntaxNode> GetMethodBody(ClassInfo classInfo, MethodInfo methodInfo)
        {
            var constructor = classInfo.Constructors.Last();
            var methodStatements = new List<SyntaxNode>();
            methodStatements.AddRange(GetMethodArrangeBlock(methodInfo));
            methodStatements.Add(GetMethodActBlock(constructor, methodInfo));
            methodStatements.AddRange(GetMethodAssertBlock(methodInfo));
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

        private SyntaxNode GetMethodActBlock(ConstructorInfo constructor, MethodInfo methodInfo)
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
                return _generator.IdentifierName($@"_{constructor.Name.ToLower()}.{methodInfo.Name}({parameterList})");
            }

            return _generator.IdentifierName(
                $@"{methodInfo.ReturnType} actual = _{constructor.Name.ToLower()}.{methodInfo.Name}({parameterList})");
        }

        private List<SyntaxNode> GetMethodAssertBlock(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType == "void")
            {
                return new List<SyntaxNode>()
                {
                    _generator.IdentifierName("Assert.Fail(\"autogenerated\")"),
                };
            }
            else
            {
                return new List<SyntaxNode>()
                {
                    _generator.IdentifierName("int expected = 0"),
                    _generator.IdentifierName("Assert.That(actual, Is.EqualTo(expected))"),
                    _generator.IdentifierName("Assert.Fail(\"autogenerated\")"),
                };
            }
        }
    }
}