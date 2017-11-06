using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EnumGenie.Sources;
using EnumGenie.TypeScript;
using EnumGenie.Writers;
using Microsoft.AspNetCore.Mvc;
using TypeLite;
using TypeLite.Extensions;

namespace WebApiWithSpa.CodeGen
{
    public static class Program
    {
        public static readonly string ProjectName = "WebApiWithSpa";

        public static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine($"Usage: {ProjectName}.CodeGen.exe <pathToAssembly> <outputPath>");
                return 1;
            }

            var dllPath = args[0];
            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"Assembly not found: {dllPath}");
                return 1;
            }

            var outputPaths = args.Reverse().Take(args.Length - 1);

            foreach (var outputPath in outputPaths)
            {
                if (!Directory.Exists(outputPath))
                {
                    Console.WriteLine($"Output path does not exist, creating {outputPath}");
                    Directory.CreateDirectory(outputPath);
                }
            }

            var apiAssembly = Assembly.LoadFrom(dllPath);

            var apiTypes = Types(apiAssembly)
                .Where(t => t.IsSubclassOf(typeof(Controller)))
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod))
                .SelectMany(ParameterAndReturnTypes)
                .SelectMany(Unwrap)
                .Where(IsFromCorrectAssembly)
                .Distinct()
                .OrderBy(t => t.FullName)
                .ToList();

            var typeScriptFluent = TypeScript.Definitions()
                .WithIndentation("  ")
                .WithModuleNameFormatter(tsModule => "Api")
                .WithConvertor<DateTimeOffset>(obj => "string")
                .WithConvertor<Guid>(obj => "string")
                .WithMemberFormatter(mf =>
                {
                    if ((mf.MemberInfo as PropertyInfo)?.GetGetMethod().IsStatic ?? false)
                        return $"// Ignore static: {mf.Name}";

                    var suffix = ((mf.MemberInfo as PropertyInfo)?.PropertyType.IsNullable() ?? false) ? "?" : "";
                    return $"{char.ToLower(mf.Name[0])}{mf.Name.Substring(1)}{suffix}";
                })
                .AsConstEnums(false);

            foreach (var type in apiTypes)
            {
                typeScriptFluent = typeScriptFluent.For(type);
            }

            var tsModel = typeScriptFluent.ModelBuilder.Build();

            foreach (var outputPath in outputPaths)
            {
                var typesPath = Path.Combine(outputPath, "api.d.ts");
                Console.WriteLine($"Writing types to {typesPath}");
                File.WriteAllText(typesPath, typeScriptFluent.Generate());

                var enumsPath = Path.Combine(outputPath, "enums.ts");
                Console.WriteLine($"Writing enums to {enumsPath}");
                new EnumGenie.EnumGenie()
                    .SourceFrom.List(tsModel.Enums.Select(e => e.Type).Distinct())
                    .WriteTo.File(enumsPath, cfg => cfg.TypeScript())
                    .Write();
            }

            Console.WriteLine($"Finished generating TypeScript contracts");
            return 0;
        }

        private static IEnumerable<Type> Types(Assembly assembly)
        {
            var matchingTypes = new List<Type>();

            foreach (var type in assembly.ExportedTypes)
            {
                try
                {
                    matchingTypes.Add(type);
                }
                catch
                {
                    // Om nom nom
                }
            }

            return matchingTypes;
        }

        private static IEnumerable<Type> ParameterAndReturnTypes(MethodInfo method)
        {
            return method.GetParameters().Select(p => p.ParameterType)
                .Concat(new[] { method.ReturnType })
                .Distinct();
        }

        private static bool IsFromCorrectAssembly(Type type)
        {
            return type.Assembly.FullName.StartsWith(ProjectName);
        }

        private static IEnumerable<Type> Unwrap(Type type)
        {
            return type.IsGenericType ? UnwrapGeneric(type)
                : type.IsArray ? UnwrapArray(type)
                    : new[] { type };
        }

        private static IEnumerable<Type> UnwrapArray(Type type)
        {
            return Unwrap(type.GetElementType());
        }

        private static IEnumerable<Type> UnwrapGeneric(Type type)
        {
            return type.GenericTypeArguments.SelectMany(Unwrap).Concat(IsFromCorrectAssembly(type) ? new[] { type } : new Type[0]);
        }
    }
}
