using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace NtrTrs.Services
{
    public class EntryService
    {
        private NtrTrsContext _context { get; set; }

        private ActivityService _activityService { get; set; }
        public EntryService(NtrTrsContext context, ActivityService activityService) {
            _context = context;
            _activityService = activityService;
        }

        public List<Entry> GetEntriesFromAllMonths(List<MonthEntry> allMonthsData)
        {
            List<Entry> allEntries = new List<Entry>();

            foreach(var month in allMonthsData) {
                allEntries.AddRange(month.Entries);
            }

            return allEntries;
        }

        public Entry GetEntryById(int id)
        {
            return _GetEntryById(id);
        }

        public void CreateEntry(Entry entry)
        {
            _context.Entries.Add(entry);
            _context.SaveChanges();
        }

        public void EditEntry(int id, string code, DateTime date, string subcode, int time, string description)
        {
            Entry entry = _GetEntryById(id);
            if (entry != null)
            {
                if (date.GetHashCode() != 0)
                {
                    entry.Date = date;
                }
                if (time != 0)
                {
                    entry.Time = time;
                }
                if (subcode != null)
                {
                    entry.Subcode = subcode;
                }
                if (description!= null)
                {
                    entry.Description = description;
                }

                if (code != null)
                {
                    Activity activity = _activityService.GetActivityByCode(code);
                    if (activity != null)
                    {
                        entry.Activity = activity;
                    }
                }
                _context.Update(entry);
                _context.SaveChanges();
            }  
        }
        
        public void DeleteEntry(int id)
        {
            Entry entryToRemove = _GetEntryById(id);

            if (entryToRemove != null)
            {
                _context.Entries.Remove(entryToRemove);
                _context.SaveChanges();
            }

        }

        private Entry _GetEntryById(int id)
        {
            return _context.Entries.Where(e => e.Id == id).Include(e => e.Activity).FirstOrDefault();
        }
    }
}