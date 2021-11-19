using System.Collections.Generic;
using System.Linq;
using JNIGenerator;

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
        SetTargetType(property.Type);
    }

    public void SetTargetType(LType swType)
    {
        if (typeMapping.TryGetValue(swType.Sourcename, out string targetType))
        {
            swType.Targetname = targetType;
        }
        if (typeMapping.ContainsKey("object") && (api.Structs.Any(m => m.Name == swType.Sourcename) || api.Enums.Any(m => m.Name == swType.Sourcename)))
        {
            swType.Targetname = typeMapping["object"];
        }

        if (swType.Isarray)
        {
            swType.Targetname = string.Format(typeMapping["array"], swType.Targetname ?? swType.Sourcename);
        } else {
            swType.Targetname = swType.Targetname ?? swType.Sourcename ?? string.Empty;
        }
        SetDefaultValue(swType);
    }

    public void SetDefaultValue(LType swType)
    {
        if (swType.Isarray)
        {
            swType.Default = "emptyArray()";
        }
        else if (swType.Targetname == "Int")
        {
            swType.Default = "0";
        }
        else if (swType.Targetname == "Float")
        {
            swType.Default = "0F";
        }
        else if (swType.Isenum)
        {
            var enm = api.Enums.FirstOrDefault(e => e.Name == swType.Targetname);
            if (enm != null)
                swType.Default = $"{swType.Targetname}.{enm.Values.First()}";
        }
        else
        {
            swType.Default = $"{swType.Targetname}()";
        }
    }
}