using System;
using System.Linq;
using System.Reflection;

namespace InspectApi
{
    internal static class Program
    {
        private static int Main()
        {
            Console.WriteLine("Inspecting ObjectIR.Core.dll API...");
            var asmPath = Path.GetFullPath("..\\..\\bin\\Debug\\net10.0\\ObjectIR.Core.dll");
            if (!File.Exists(asmPath))
            {
                Console.Error.WriteLine($"Assembly not found: {asmPath}");
                return 2;
            }
            var asm = Assembly.LoadFrom(asmPath);
            Console.WriteLine($"Loaded: {asm.FullName}");

            var typesToFind = new[] { "ObjectIR.Core.Builder.InstructionBuilder", "ObjectIR.Core.Builder.MethodBuilder", "ObjectIR.Core.Builder.IRBuilder", "ObjectIR.Core.Builder.ClassBuilder" };

            foreach (var tname in typesToFind)
            {
                var t = asm.GetType(tname);
                if (t == null)
                {
                    Console.WriteLine($"Type not found: {tname}");
                    continue;
                }
                Console.WriteLine($"\nType: {t.FullName}");
                var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).OrderBy(m => m.Name);
                foreach (var m in methods)
                {
                    var paramsStr = string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name));
                    Console.WriteLine($"- {m.ReturnType.Name} {m.Name}({paramsStr})");
                }

                var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var p in props)
                {
                    Console.WriteLine($"  Prop: {p.PropertyType.Name} {p.Name}");
                }
            }

            return 0;
        }
    }
}
