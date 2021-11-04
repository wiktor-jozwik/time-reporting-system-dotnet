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
        } else if (typeof(T) == typeof(AcitvityList)) {
            return System.Text.Json.JsonSerializer.Deserialize<AcitvityList>(jsonString);
        } 
        else {
            return false;
        }
    }

    public static void writeEntry(EntryModel data, string filePath) {
        if (!File.Exists(filePath)) {
            using (FileStream fs = File.Create(filePath)){};
        }
        var readJsonData = System.IO.File.ReadAllText(filePath);

        var monthEntries = JsonConvert.DeserializeObject<MonthModel>(readJsonData) 
                            ?? new MonthModel();

        if (monthEntries.Entries != null) {
            monthEntries.Entries.Add((EntryModel) data);
        } else {
            monthEntries.Entries = new List<EntryModel>{(EntryModel) data};
        }

        var responseData = monthEntries;
        var settings=new JsonSerializerSettings{DateFormatString ="yyyy-MM-dd"};

        string jsonData = JsonConvert.SerializeObject(responseData, Formatting.Indented, settings);
        System.IO.File.WriteAllText(filePath, jsonData);

    }

    public static void writeMonth(MonthModel data, string filePath) {
        var settings=new JsonSerializerSettings{DateFormatString ="yyyy-MM-dd"};

        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public static void writeActivity(ActivityModel data) {
        string filePath = "Data/activity.json";

        AcitvityList activities = FileParser.readJson<AcitvityList>("Data/activity.json");

        if (activities.Activities != null) {
            activities.Activities.Add(data);
        } else {
            activities.Activities = new List<ActivityModel>{data};
        }
        
        string jsonData = JsonConvert.SerializeObject(activities, Formatting.Indented);
        System.IO.File.WriteAllText(filePath, jsonData);
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