using System.Collections.Generic;

namespace TestGeneratorLibrary.TestStructureInfo
{
    public class FileInfo
    {
        public List<ClassInfo> Classes{ get; set; }

        public FileInfo(List<ClassInfo> classes)
        {
            this.Classes = classes;
        }
    }
}