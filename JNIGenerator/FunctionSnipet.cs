using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JNIGenerator
{
    class FunctionSnipet : Snipet
    {
        public FunctionSnipet(string template, Api api, Function function, Dictionary<string, string> typeMapping, string projectDir, string apiPackage = null, string apiName = null) : base(Path.Combine(projectDir, template))
        {
            Function = function.Clone();
            ApiPackage = apiPackage;
            ApiName = apiName;
            if (Function.Result != null)
                new TypeConverter(typeMapping, api).SetTargetType(Function.Result);
            Name = Function.OperationId;
            Parameters = Function.Parameters.Select(p => p.Clone()).ToList();
            foreach (var p in Parameters)
                new TypeConverter(typeMapping, api).SetTargetType(p);
        }

        public Function Function { get; }
        public string ApiPackage { get; private set; }
        public string ApiName { get; private set; }
        public List<Property> Parameters { get; }
        public string Name { get; }


    }
}
