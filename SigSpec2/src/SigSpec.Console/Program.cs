using Agh;
using SigSpec.CodeGeneration.TypeScript;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Agh.eSzachy.Hubs;

namespace SigSpec
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SigSpec for SignalR Core");
            Run(args[0]);
        }

        static void Run(string path)
        {
            var settings = new SigSpecGeneratorSettings();
            var generator = new SigSpecGenerator(settings);

            // TODO: Add PR to SignalR Core with new IHubDescriptionCollectionProvider service
            var document = generator.GenerateForHubs(new Dictionary<string, Type>
            {
                { "room", typeof(RoomHub) },
                { "game", typeof(GameHub) },
            });

            var json = document.ToJson();
            Console.WriteLine("\nGenerated SigSpec document:");
            Console.WriteLine(json);

            var codeGeneratorSettings = new SigSpecToTypeScriptGeneratorSettings();
            var codeGenerator = new SigSpecToTypeScriptGenerator(codeGeneratorSettings);
            var file = codeGenerator.GenerateFile(document);

            Console.WriteLine("\n\nGenerated SigSpec TypeScript code:");
            Console.WriteLine(file);
            var fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            //Create a file to write to.
            using (var sw = fi.CreateText())
            {
                sw.Write(file);
            }
            Console.WriteLine(fi.FullName);
        }
    }
}
