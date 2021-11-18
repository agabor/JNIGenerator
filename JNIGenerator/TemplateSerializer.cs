using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scriban;

namespace JNIGenerator;

static class TemplateSerializer
{
    private static Dictionary<string, string> C2KotlinTypes = new Dictionary<string, string> {
                {"float", "Float"},
                {"int", "Int"},
                {"char*", "String"},
                {"array", "Array<{0}>"}
            };
    private static Dictionary<string, string> C2JniTypes = new Dictionary<string, string> {
              {"float", "jfloat"},
              {"int", "jint"},
              {"char*", "jstring"},
              {"array", "Array<{0}>"},
              {"object", "jobject"}
            };
    public static void WriteKotlinApi(Api api, string path, string projectDir, string apiPackage, string apiName, string modelPackage)
    {
        using (StreamWriter file = new StreamWriter(path, false))
        {
            var template = Template.Parse(File.ReadAllText(Path.Combine(projectDir, "kotlinapi.scriban")));
            var functions = api.Functions.Select(r => new FunctionSnipet("kotlinfunction.scriban", api, r, C2KotlinTypes, projectDir).Render());
            file.Write(template.Render(new { Functions = functions, ApiPackage = apiPackage, ApiName = apiName, ModelPackage = modelPackage }));
            file.Close();
        }
    }
    public static void WriteJNI(Api api, string path, string projectDir, string apiPackage, string apiName, string modelPackage, List<string> headers)
    {
        using (StreamWriter file = new StreamWriter(path, false))
        {
            var template = Template.Parse(File.ReadAllText(Path.Combine(projectDir, "jni.scriban")));
            var functions = api.Functions.Select(r => new FunctionSnipet("jnifunction.scriban", api, r, C2JniTypes, projectDir, apiPackage.Replace('.', '_'), apiName).Render());
            var structs = api.Structs.Select(r => new StructSnipet("jnistructfunctions.scriban", api, r, C2JniTypes, projectDir, modelPackage.Replace('.', '/')).Render());
            file.Write(template.Render(new { Structs = structs, Functions = functions, Headers = headers }));
            file.Close();
        }
    }

    public static void WriteKotlinDataClasses(Api api, string path, string projectDir, string modelPackage)
    {
        foreach (var strct in api.Structs)
        {
            using (StreamWriter file = new StreamWriter(Path.Combine(path, $"{strct.Name}.kt"), false))
            {
                file.Write(new StructSnipet("kotlindataclass.scriban", api, strct, C2KotlinTypes, projectDir, modelPackage).Render());
                file.Close();
            }
        }
        foreach (var enm in api.Enums)
        {
            using (StreamWriter file = new StreamWriter(Path.Combine(path, $"{enm.Name}.kt"), false))
            {
                file.Write(new EnumSnipet("kotlinenumclass.scriban", api, enm, projectDir, modelPackage).Render());
                file.Close();
            }
        }
    }

}
