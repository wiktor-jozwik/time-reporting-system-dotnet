using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using NtrTrs.Models;

public static class EntryService {
        public static DateTime getRequestedDateTime(string dateString) {
            DateTime dateTime =  DateTime.ParseExact(dateString, "yyyy-MM", null);
            
            return dateTime;
        }

        public static string getFileNameFromDate(string name, DateTime date) {
            return $"Data/entries/{name}-{date.ToString("yyyy-MM")}.json";
        }

        public static MonthModel getMonthData(string filePath) {
            return FileParser.readJson<MonthModel>(filePath);
        }
        public static List<EntryModel> getMonthEntries(string filePath) {
            MonthModel monthData = FileParser.readJson<MonthModel>(filePath);
            return monthData.Entries;
        }
}