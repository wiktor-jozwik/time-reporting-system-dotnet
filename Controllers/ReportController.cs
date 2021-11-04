using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;
using System.Linq;

namespace NtrTrs.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index(string dateString = null) {


            DateTime dateTime;

            if (dateString == null) {
                dateTime = DateTime.Now;
            } else {
                try {
                    dateTime = EntryService.getRequestedDateTime(dateString);

                } catch (System.FormatException) {
                    return View("Error");
                }
            }

            List<ReportViewModel> monthlyReport = null;

            string userName = FileParser.getLoggedUser();
            string filePath = EntryService.getFileNameFromDate(userName.ToLower(), dateTime);
            try {
                MonthModel monthData = EntryService.getMonthData(filePath);

                monthlyReport = monthData.Entries
                                    .GroupBy(en => en.Code)
                                    .Select(mr => new ReportViewModel {
                                        Code = mr.First().Code,
                                        TotalTime = mr.Sum(c => c.Time),
                                    }).ToList();

            } catch (System.IO.FileNotFoundException) {
                monthlyReport = null;

            } catch (Exception) {
                return View("Error");
            }
            ViewData["DateTime"] = dateTime;
            ViewData["UserName"] = userName;
            return View(monthlyReport);
        }
    }
}