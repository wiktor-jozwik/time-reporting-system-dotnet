using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using NtrTrs.Models;

public static class FileParser
{
    const string LOGGED_USER_FILE_PATH = "Data/logged_user.txt";
    public static dynamic readJson<T>(string filePath){
        string jsonString = System.IO.File.ReadAllText(filePath);

        if (typeof(T) == typeof(MonthModel)) {
            return System.Text.Json.JsonSerializer.Deserialize<MonthModel>(jsonString);
        } else if (typeof(T) == typeof(UserList)) {
            return System.Text.Json.JsonSerializer.Deserialize<UserList>(jsonString);
        } else {
            return false;
        }
    }

    public static dynamic writeJson<T>(object model, string filePath){

    if (typeof(T) == typeof(MonthModel) && (model.GetType() == typeof(EntryModel))) {
        if (!File.Exists(filePath)) {
            using (FileStream fs = File.Create(filePath)){};
        }
        var readJsonData = System.IO.File.ReadAllText(filePath);

        var monthEntries = JsonConvert.DeserializeObject<MonthModel>(readJsonData) 
                            ?? new MonthModel();

        if (monthEntries.Entries != null) {
            monthEntries.Entries.Add((EntryModel) model);
        } else {
            monthEntries.Entries = new List<EntryModel>{(EntryModel) model};
        }

        var responseData = monthEntries;
        var settings=new JsonSerializerSettings{DateFormatString ="yyyy-MM-dd"};

        string jsonData = JsonConvert.SerializeObject(responseData, Formatting.Indented, settings);
        System.IO.File.WriteAllText(filePath, jsonData);

        return true;
    } else if (typeof(T) == typeof(MonthModel) && (model.GetType() == typeof(MonthModel))) {
        var settings=new JsonSerializerSettings{DateFormatString ="yyyy-MM-dd"};

        string jsonData = JsonConvert.SerializeObject(model, Formatting.Indented, settings);
        System.IO.File.WriteAllText(filePath, jsonData);
        return true;
    } 
    
    else {
        return false;
        }
    }

    public static void logUser(string userName) {
            using (StreamWriter outputFile = new StreamWriter(LOGGED_USER_FILE_PATH)) {
                outputFile.WriteLine(userName.ToLower());
            }
    }

    public static string getLoggedUser() {
            return System.IO.File.ReadAllText(LOGGED_USER_FILE_PATH).Trim();

    }
}