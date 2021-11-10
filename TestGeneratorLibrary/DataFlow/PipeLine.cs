using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestGeneratorLibrary.CodeAnalyze.AnalyzerImpl;
using TestGeneratorLibrary.TestGenerator.TestGeneratorImpl;

namespace TestGeneratorLibrary.DataFlow
{
    public class PipeLine
    {
        public Task Generate(string sourcePath, string[] fileToAnalyze, string destinationPath, int pipeLineRestriction)
        {
            var execOption = new ExecutionDataflowBlockOptions() {MaxDegreeOfParallelism = pipeLineRestriction};
            var linkOptions = new DataflowLinkOptions {PropagateCompletion = true};
            Directory.CreateDirectory(destinationPath);
            var readFile = new TransformBlock<string, string>(async path =>
            {
                using (var reader = new StreamReader(path))
                {
                    return await reader.ReadToEndAsync();
                }
            }, execOption);

            var generateTestClasses = new TransformManyBlock<string, KeyValuePair<string, string>>(
                async sourceCode =>
                {
                    var fileInfo = await Task.Run(() => new CodeAnalyzer().AnalyzeFile(sourceCode));
                    return await Task.Run(() => new TestGenerator.TestGeneratorImpl.TestGenerator().Generate(fileInfo));
                }, execOption);

            var writeFile = new ActionBlock<KeyValuePair<string, string>>(async path =>
            {
                using (var writer = new StreamWriter(destinationPath + '\\' + path.Key + ".cs"))
                {
                    await writer.WriteAsync(path.Value);
                }
            }, execOption);

            readFile.LinkTo(generateTestClasses, linkOptions);
            generateTestClasses.LinkTo(writeFile, linkOptions);

            foreach (var file in fileToAnalyze)
            {
                readFile.Post(sourcePath + @"\" + file);
            }

            readFile.Complete();
            return writeFile.Completion;
        }
    }
}