using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Candbox
{
    public class Compiler
    {
        public object Compile(string code)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            string assemblyName = "code.cs";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<MetadataReference> references = new List<MetadataReference>();
            foreach (var asmbl in assemblies)
            {
                try
                {
                    if (!asmbl.IsDynamic && !string.IsNullOrWhiteSpace(asmbl.Location))
                    {
                        references.Add(MetadataReference.CreateFromFile(asmbl.Location));
                    }
                }
                catch (NotSupportedException)
                {
                    continue;
                }
            }

            var runtimePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(object).Assembly.Location), "System.Runtime.dll");
            if (File.Exists(runtimePath))
            {
                references.Add(MetadataReference.CreateFromFile(runtimePath));
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    string diagnostics = "";
                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.WriteLine(diagnostic.ToString());
                        diagnostics += diagnostic.ToString();
                    }
                    return diagnostics;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType("DynamicNamespace.DynamicClass");
                    object instance = Activator.CreateInstance(type);
                    MethodInfo method = type.GetMethod("Main");

                    object response = method.Invoke(instance, null);
                    Console.WriteLine(response);
                    return response;
                }
            }
        }
    }
}
