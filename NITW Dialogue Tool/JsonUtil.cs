using Newtonsoft.Json;
using NITW_Dialogue_Tool;
using System;
using System.Collections.Generic;
using System.IO;

class JsonUtil
{
    public static void saveYarnDictionary(yarnDictionary rootz)
    {
        string json = JsonConvert.SerializeObject(rootz);
        json = JsonUtil.JsonPrettify(json);
        File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yarnFiles.json"), json);
    }

    public static yarnDictionary loadYarnDictionary()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yarnFiles.json");
        if (File.Exists(path))
        {
            return (JsonConvert.DeserializeObject<yarnDictionary>(File.ReadAllText(path)));
        }
        else
        {
            yarnDictionary rootz = new yarnDictionary();
            rootz.yarnFiles = new Dictionary<string, yarnFile>();
            return rootz;
        }
            
    }

    public static string JsonPrettify(string json)
    {
        using (var stringReader = new StringReader(json))
        using (var stringWriter = new StringWriter())
        {
            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
            jsonWriter.WriteToken(jsonReader);
            return stringWriter.ToString();
        }
    }
}