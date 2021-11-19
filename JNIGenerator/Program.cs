using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace JNIGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = JObject.Parse(File.ReadAllText(args[0]));
            var projectDir = Path.GetDirectoryName(args[0]);
            var path = config["headers_folder"].ToString();
            var files = ((JArray)config["headers"]).Select(s => Path.Combine(path, s.ToString())).ToList();

            Api api = CApiParser.ParseApi(files);
            string apiPackage = config["kotlin_api_package"].ToString();
            string apiName = config["kotlin_api_name"].ToString();
            string modelPackage = config["kotlin_models_package"].ToString();
            TemplateSerializer.WriteKotlinApi(api, config["kotlin_api_path"].ToString(), projectDir, apiPackage, apiName, modelPackage);
            TemplateSerializer.WriteKotlinDataClasses(api, config["kotlin_models_path"].ToString(), projectDir, modelPackage);
            string jniPath = config["jni_path"].ToString();
            string jniDir = Path.GetDirectoryName(jniPath);
            
            TemplateSerializer.WriteJNI(api, jniPath, projectDir, apiPackage, apiName, modelPackage, files.Select(f => Path.GetRelativePath(jniDir, f)).ToList());
        }
    }
}