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
                    dateTime = EntrysService.getRequestedDateTime(dateString);

                } catch (System.FormatException) {
                    return View("Error");
                }
            }
            List<ReportViewModel> monthlyReport = null;

            string userName = FileParser.getLoggedUser();
            string filePath = EntrysService.getFileNameFromDate(userName.ToLower(), dateTime);
            try {
                MonthModel monthData = EntrysService.getMonthData(filePath);
                ViewData["Frozen"] = monthData.Frozen;

                monthlyReport = this.getMontlyReport(monthData);


            } catch (System.IO.FileNotFoundException) {
                monthlyReport = null;

            } catch (Exception e) {
                System.Console.WriteLine(e.Message);
                return View("Error");
            }
            ViewData["DateTime"] = dateTime;
            ViewData["UserName"] = userName;
            return View(monthlyReport);
        }

        public IActionResult Submit(DateTime Date) {
            string userName = FileParser.getLoggedUser();
            string filePath = EntrysService.getFileNameFromDate(userName, Date);


            List<ReportViewModel> monthlyReport = null;
            MonthModel monthData = EntrysService.getMonthData(filePath);

            bool frozen = monthData.Frozen;
                if(frozen) {
                    return View("BadRequest");
                }

            monthData.Frozen = true;
            ViewData["Frozen"] = monthData.Frozen;

            FileParser.writeMonth(monthData, filePath);

            try {
                monthlyReport = this.getMontlyReport(monthData);

            } catch (System.IO.FileNotFoundException) {
                monthlyReport = null;

            } catch (Exception) {
                return View("Error");
            }
            ViewData["DateTime"] = Date;
            ViewData["UserName"] = userName;

            return View("Index", monthlyReport);
        }

        private List<ReportViewModel> getMontlyReport(MonthModel monthData) {
            List<ReportViewModel> monthlyReport = null;
            monthlyReport = monthData.Entries
                                .GroupBy(en => en.Code)
                                .Select(mr => new ReportViewModel {
                                    Code = mr.First().Code,
                                    TotalTime = mr.Sum(c => c.Time),
                                }).ToList();

            if (monthData.Accepted != null) {
                foreach(var m in monthlyReport) {
                    var acceptedFound = monthData.Accepted.Where(e => e.Code == m.Code).FirstOrDefault(); 
                    if (acceptedFound != null) {
                        m.AcceptedTime = acceptedFound.Time;
                    }
                }
            }
            
            return monthlyReport;
        }
    }
}