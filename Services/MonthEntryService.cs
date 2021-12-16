using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using NtrTrs.Models;

namespace NtrTrs.Services
{
    public class MonthEntryService
    {
        private NtrTrsContext _context { get; set; }
        public MonthEntryService(NtrTrsContext context) {
            _context = context;
        }

        public List<MonthEntry> GetAllMonthsData()
        {
            return _context.MonthEntries
                        .Include(m => m.Entries)
                        // .ThenInclude(e => e.User)
                        .ToList();
        }

        public MonthEntry GetMonthDataForUser(DateTime datetime, User user)
        {
            return _GetMonthDataForUser(datetime, user);
        }

        public void CreateMonthEntry(MonthEntry monthEntry)
        {
            _context.MonthEntries.Add(monthEntry);
            _context.SaveChanges();
        }

        public void FreezeMonth(MonthEntry monthEntry)
        {
            monthEntry.Frozen = true;
            _context.Update(monthEntry);
            _context.SaveChanges();
        }

        public List<ReportViewModel> GetMontlyReport(MonthEntry monthData)
        {
            List<ReportViewModel> monthlyReport = null;
            monthlyReport = monthData.Entries
                                .GroupBy(en => en.Activity.Code)
                                .Select(mr => new ReportViewModel {
                                    Code = mr.First().Activity.Code,
                                    TotalTime = mr.Sum(c => c.Time),
                                }).ToList();

            if (monthData.Accepted != null) {
                foreach(var m in monthlyReport) {
                    var acceptedFound = monthData.Accepted.Where(e => e.Activity.Code == m.Code).FirstOrDefault(); 
                    if (acceptedFound != null) {
                        m.AcceptedTime = acceptedFound.Time;
                    }
                }
            }
            
            return monthlyReport;
        }

        private MonthEntry _GetMonthDataForUser(DateTime dateTime, User user)
        {
            return _context.MonthEntries
                            .IncludeFilter(m => m.Entries.Where(e => e.User == user)
                            .Select(e => e.Activity))
                            // .ThenInclude(e => e.Activity)
                            .Where(m => m.Date.Month == dateTime.Month)
                            .Where(m => m.User == user)
                            .FirstOrDefault();
        }
    }
}