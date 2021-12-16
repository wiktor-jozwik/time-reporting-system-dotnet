using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NtrTrs.Services
{
    public class MonthEntryService
    {
        private NtrTrsContext _context { get; set; }
        public MonthEntryService(NtrTrsContext context) {
            _context = context;
        }

        public MonthEntry GetMonthData(DateTime datetime)
        {
            return _GetMonthData(datetime);
        }

        public void CreateMonthEntry(MonthEntry monthEntry)
        {
            _context.MonthEntries.Add(monthEntry);
            _context.SaveChanges();
        }

        private MonthEntry _GetMonthData(DateTime dateTime)
        {
            return _context.MonthEntries
                            .Include(m => m.Entries)
                            .ThenInclude(e => e.Activity)
                            .Where(m => m.Date.Month == dateTime.Month)
                            .FirstOrDefault();
        }
    }
}