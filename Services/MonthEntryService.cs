using System;
using System.Collections.Generic;
using System.Linq;

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

        private MonthEntry _GetMonthData(DateTime dateTime)
        {
            return _context.MonthEntries.Where(m => m.Date == dateTime).FirstOrDefault();
        }
    }
}