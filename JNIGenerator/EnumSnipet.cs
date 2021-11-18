using System.Collections.Generic;
using System.IO;
using JNIGenerator;

public class EnumSnipet : Snipet
{
    public string Name { get; }
    public string ModelPackage { get; private set; }
    public List<string> Values { get; }
    public EnumSnipet(string template, Api api, CEnum enm, string projectDir, string modelPackage = null) : base(Path.Combine(projectDir, template))
    {
        Name = enm.Name;
        ModelPackage = modelPackage;
        Values = enm.Values;
    }
}
