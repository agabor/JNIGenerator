using System;
using System.Collections.Generic;
using System.Linq;
using JNIGenerator;
using Newtonsoft.Json.Linq;

public class TypeConverter
{
    private readonly Dictionary<string, string> typeMapping;
    private readonly Api api;

    public TypeConverter(Dictionary<string, string> typeMapping, Api api)
    {
        this.typeMapping = typeMapping;
        this.api = api;
    }
    public void SetTargetType(Property property)
    {
        if (!property.Required)
        {
            if (typeMapping.TryGetValue(property.Type.Sourcename, out string targetType))
            {
                property.Type.Targetname = targetType;
                return;
            }
        }
        SetTargetType(property.Type);
    }

    public void SetTargetType(LType swType)
    {
        if (typeMapping.TryGetValue(swType.Sourcename, out string targetType))
        {
            swType.Targetname = targetType;
        }
        if (typeMapping.ContainsKey("object") && api.Structs.Any(m => m.Name == swType.Sourcename))
        {
            swType.Targetname = typeMapping["object"];
        }

        if (swType.Isarray) {
          swType.Targetname = string.Format(typeMapping["array"], swType.Targetname ?? swType.Sourcename);
          return;
        }
        swType.Targetname = swType.Targetname ?? swType.Sourcename ?? string.Empty;
    }
}