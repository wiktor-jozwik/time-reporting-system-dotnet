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
            return _GetAllMonthsData();
        }

        public MonthEntry GetMonthDataForUser(DateTime datetime, User user)
        {
            return _GetMonthDataForUser(datetime, user);
        }

        public List<ManagerViewModel> GetListForManager(string code, string userName) 
        {
            List<MonthEntry> monthData = _GetAllMonthsData();

            // Accepted month
            monthData = monthData.Where(m => m.Frozen == true && m.Entries.Any(a => a.User.Name == userName)).ToList();

            List<ManagerViewModel> managerList = new List<ManagerViewModel>();

            foreach(var month in monthData) {
                managerList.AddRange(month.Entries
                    .Where(en => en.Activity.Code == code)
                    .GroupBy(en => en.Activity.Code)
                    .Select(ml => new ManagerViewModel {
                        TotalTime = ml.Sum(c => c.Time), 
                        Date = ml.Select(a => a.Date).FirstOrDefault()}));

                if (month.Accepted != null) {
                    var accepted = month.Accepted.Where(a => a.Activity.Code == code).FirstOrDefault();

                    if(accepted != null ){
                        foreach(var m in managerList) {
                            int acceptedTime = accepted.Time;
                            m.AcceptedTime = acceptedTime;
                        }
                    }
                }
            }

            return managerList;
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
        
        public void CreateAcceptedEntry(Activity activity, DateTime date, User user, int time)
        {
            MonthEntry monthData = GetMonthDataForUser(date, user);

            // AcceptedEntry acceptedEntry = new AcceptedEntry {Activity = activity, Time = time};
            
            // if (monthData.Accepted != null && monthData.Accepted.Count != 0) {
                // monthData.Accepted = acceptedEntry;

                AcceptedEntry acc = new AcceptedEntry(){Activity = activity, Time = time};

                _context.AcceptedEntries.Add(acc);

                // AcceptedEntry acc = _context.AcceptedEntries.Add(new AcceptedEntry(){Activity = activity, Time = time})


                if (monthData.Accepted == null)
                {
                    monthData.Accepted = new List<AcceptedEntry>();
                }
                // if (monthData.Accepted == null)
                // {
                monthData.Accepted.Add(acc);
                // }
                // monthData.Accepted.Add(acceptedEntry);

                _context.Update(monthData);
                _context.SaveChanges();

                // int index = monthAccepted.FindIndex(x => x.Activity.Code == activity.Code);
                // if(index > 0) {
                    // monthAccepted[index] = acceptedModel;
                // } else {
                    // monthData.Accepted = new List<AcceptedEntryModel>();
                    // monthData.Accepted.Add(acceptedModel);
                // }
            // } 

            // FileParser.writeMonth(monthData, filePath);

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

        private List<MonthEntry> _GetAllMonthsData()
        {
            return _context.MonthEntries
                    .Include(m => m.Entries)
                        .ThenInclude(e => e.User)
                    .Include(m => m.Entries)
                        .ThenInclude(e => e.Activity)
                    .Include(m => m.Accepted)
                    .ToList();
        }

        private MonthEntry _GetMonthDataForUser(DateTime dateTime, User user)
        {
            return _context.MonthEntries
            .Include(m => m.Accepted)
                            .IncludeFilter(m => m.Entries.Where(e => e.User == user)
                            .Select(e => e.Activity))
                            .Where(m => m.Date.Month == dateTime.Month)
                            .Where(m => m.User == user)
                            .FirstOrDefault();
        }
    }
}