using Agh;
using SigSpec.CodeGeneration.TypeScript;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SigSpec
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SigSpec for SignalR Core");
            Run();
        }

        static void Run()
        {
            var settings = new SigSpecGeneratorSettings();
            var generator = new SigSpecGenerator(settings);

            // TODO: Add PR to SignalR Core with new IHubDescriptionCollectionProvider service
            var document = generator.GenerateForHubs(new Dictionary<string, Type>
            {
                { "room", typeof(RoomHub) },
            });

            var json = document.ToJson();

            Console.WriteLine("\nGenerated SigSpec document:");
            Console.WriteLine(json);

            var codeGeneratorSettings = new SigSpecToTypeScriptGeneratorSettings();
            var codeGenerator = new SigSpecToTypeScriptGenerator(codeGeneratorSettings);
            var file = codeGenerator.GenerateFile(document);

            Console.WriteLine("\n\nGenerated SigSpec TypeScript code:");
            Console.WriteLine(file);
            var fi = new FileInfo("./../../../../../../Agh.eSzachy/ClientApp/src/Api.ts");
            if (fi.Exists)
            {
                fi.Delete();
            }
            //Create a file to write to.
            using (StreamWriter sw = fi.CreateText())
            {
                sw.Write(file);
            }
            Console.WriteLine(fi.FullName);
        }
    }
}
