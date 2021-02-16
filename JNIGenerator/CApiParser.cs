using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace JNIGenerator
{
    class CApiParser
    {
        public static Api ParseApi(List<string> files)
        {
            var api = new Api();
            api.Structs = new List<Struct>();
            api.Functions = new List<Function>();
            bool parsingStruct = false;
            foreach(var path in files)
            foreach (var line in File.ReadAllLines(path))
            {
                string[] lineParts = line.Split("//");
                string comment = lineParts.Last().Trim();
                int arrayLength = 0;
                if (comment.StartsWith("array-length:"))
                {
                    arrayLength =int.Parse(comment.Split(":").Last().Trim());
                }
                var cleanLine = lineParts.First().Trim();
                if (cleanLine.StartsWith("}"))
                {
                    parsingStruct = false;
                    Console.WriteLine();
                    if (comment.Trim() == "no-jni")
                    {
                        Console.WriteLine($"Ignoring: {api.Structs.Last().Name}");
                        api.Structs.RemoveAt(api.Structs.Count-1);
                    }
                }
                else if (cleanLine.StartsWith("struct") && cleanLine.EndsWith("{"))
                {
                    cleanLine = TrimStruct(cleanLine);
                    var name = TrimLastChar(cleanLine).Trim();
                    Console.WriteLine(name);
                    api.Structs.Add(new Struct()
                    {
                        Name = name,
                        Properties = new List<Property>()
                    });
                    parsingStruct = true;
                }
                else if (parsingStruct)
                {
                    bool isCustom = cleanLine.StartsWith("struct");
                    if (cleanLine.StartsWith("const"))
                        cleanLine = cleanLine.Substring(6);
                    cleanLine = TrimStruct(cleanLine);
                    var declParts = cleanLine.Split(' ');
                    var strct = api.Structs.Last();
                    string type = declParts[0].Trim();
                    for (int i = 1; i < declParts.Length; ++i)
                    {
                        string name = declParts[i].Trim();
                        name = name.Substring(0, name.Length - 1);
                        if (name.StartsWith("*"))
                        {
                            name = name.Substring(1);
                            type += "*";
                        }
                        bool isArray = false;
                        if (type.EndsWith("*")) {
                            type = type.Substring(0, type.Length - 1);
                            isArray = true;
                            if (type == "char" || type == "char*") {
                                type = "char*";
                                isArray = false;
                            }
                        }
                        strct.Properties.Add(new Property
                        {
                            Name = name,
                            Type = new LType
                            {
                                Sourcename = type,
                                Iscustom = isCustom,
                                Isprimitive = !isCustom,
                                Isarray = isArray
                            },
                            ArrayLength = arrayLength
                        });
                        Console.WriteLine($"  {name}: {type}");
                    }
                }
                else if (cleanLine.EndsWith(";"))
                {
                    if (comment == "no-jni")
                        continue;
                    bool isResultCustom = cleanLine.StartsWith("struct");
                    cleanLine = TrimLastChar(TrimStruct(cleanLine));
                    var parts = cleanLine.Split("(");
                    var typeAndName = parts[0].Split(" ");
                    var parameters = TrimLastChar(parts[1]).Split(",").Select(s => s.Trim()).Where(s => s != "void");
                    var type = typeAndName[0];
                    var function = new Function
                    {
                        OperationId = typeAndName[1],
                        Result = new LType { Sourcename = type, Iscustom = isResultCustom, Isprimitive = !isResultCustom },
                        Parameters = parameters.Select(p =>
                        {
                            p = TrimConst(p);
                            bool isCustom = p.StartsWith("struct");
                            var tn = TrimStruct(p).Split(" ");
                            return new Property
                            {
                                Name = tn[1],
                                Type = new LType
                                {
                                    Sourcename = tn[0],
                                    Iscustom = isCustom,
                                    Isprimitive = !isCustom
                                }
                            };
                        }).ToList()
                    };
                    api.Functions.Add(function);

                    Console.WriteLine($"{function.OperationId} -> {function.Result.Sourcename}");
                    foreach (var par in function.Parameters)
                    {
                        Console.WriteLine($"  {par.Name}: {par.Type.Sourcename}");
                    }
                    Console.WriteLine();
                }
            }
            return api;
        }

        private static string TrimLastChar(string cleanLine)
        {
            return cleanLine.Substring(0, cleanLine.Length - 1);
        }

        private static string TrimStruct(string cleanLine)
        {
            if (cleanLine.StartsWith("struct"))
            {
                cleanLine = cleanLine.Substring(6, cleanLine.Length - 6).Trim();
            }

            return cleanLine;
        }

        private static string TrimConst(string cleanLine)
        {
            if (cleanLine.StartsWith("const"))
            {
                cleanLine = cleanLine.Substring(5, cleanLine.Length - 5).Trim();
            }

            return cleanLine;
        }
    }
}