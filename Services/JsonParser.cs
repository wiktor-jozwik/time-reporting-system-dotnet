using System.Collections.Generic;
using System.Text.Json;
using NtrTrs.Models;

public static class JsonParserSingleton
{
    // public static readonly JsonParserSingleton _obj = new JsonParserSingleton();
    // private JsonParserSingleton() {} 
    public static dynamic readJson<T>(string fileName){
        string jsonString = System.IO.File.ReadAllText(fileName);

        if (typeof(T) == typeof(MonthModel)) {
            return JsonSerializer.Deserialize<MonthModel>(jsonString);
        } else if (typeof(T) == typeof(UserList)) {
            return JsonSerializer.Deserialize<UserList>(jsonString);
        } else {
            return false;
        }
    }

    // JsonParserSingleton() {}

}