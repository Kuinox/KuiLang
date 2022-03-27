using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KuiLang.Build
{
    public class BuildAssembly
    {
        public BuildAssembly(string fileName, string[] files)
        {
            FileName = fileName;
            Files = files;
        }

        public static BuildAssembly LoadAssembly(string assemblyPath)
        {
            string fileName = Path.GetFileName(assemblyPath);
            if (fileName != "assembly.json") throw new ArgumentException("The assembly file must be named 'assembly.json'.");
            var json = JsonSerializer.Deserialize<AssemblyJson>(assemblyPath);
            if (json != null) json.Path = assemblyPath;
            string dirPath = Path.GetDirectoryName(assemblyPath)!;
            string[] files = Directory.GetFiles(dirPath, "*.kl");
            return new BuildAssembly(fileName, files);
        }

        public IReadOnlyCollection<string> Files { get; }
        public string FileName { get; }
    }
}
