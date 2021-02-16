using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JNIGenerator;
using Newtonsoft.Json.Linq;

public class StructSnipet : Snipet
{
    public string Name { get; }
    public string ModelPackage { get; private set; }
    public List<Property> Properties { get; }
    public StructSnipet(string template, Api api, Struct strct, Dictionary<string, string> typeMapping, string projectDir, string modelPackage = null) : base(Path.Combine(projectDir, template))
    {
        Name = strct.Name;
        ModelPackage = modelPackage;
        Properties = strct.Properties.Select(p => p.Clone()).ToList();
        foreach (var prop in Properties)
        {
            new TypeConverter(typeMapping, api).SetTargetType(prop);
        }
    }
}
