using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;
using NtrTrs.Services;

namespace NtrTrs.Controllers
{
    public class ReportController : Controller
    {
        private readonly UserService _userService;
        private readonly MonthEntryService _monthEntryService;

                public ReportController(
                        UserService userService, 
                        MonthEntryService monthEntryService) 
        {
            _userService = userService;
            _monthEntryService = monthEntryService;
        }
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

            User loggedUser = _userService.GetLoggedUser();
            if (loggedUser == null)
            {
                return View("Error");
            }
            ViewData["DateTime"] = dateTime;
            ViewData["UserName"] = loggedUser.Name;

            MonthEntry monthData = _monthEntryService.GetMonthDataForUser(dateTime, loggedUser);

            if (monthData != null)
            {
                ViewData["Frozen"] = monthData.Frozen;

                monthlyReport = _monthEntryService.GetMontlyReport(monthData);

                return View(monthlyReport);
            }

            return View();
        }

        public IActionResult Submit(DateTime Date) {
            User loggedUser = _userService.GetLoggedUser();
            if (loggedUser == null)
            {
                return View("Error");
            }


            List<ReportViewModel> monthlyReport = null;
            MonthEntry monthData = _monthEntryService.GetMonthDataForUser(Date, loggedUser);

            bool frozen = monthData.Frozen;
                if(frozen) {
                    return View("BadRequest");
                }

            monthData.Frozen = true;
            ViewData["Frozen"] = monthData.Frozen;

            _monthEntryService.FreezeMonth(monthData);

            try {
                monthlyReport = _monthEntryService.GetMontlyReport(monthData);

            } catch (System.IO.FileNotFoundException) {
                monthlyReport = null;

            } catch (Exception) {
                return View("Error");
            }
            ViewData["DateTime"] = Date;
            ViewData["UserName"] = loggedUser.Name;

            return View("Index", monthlyReport);
        }
    }
}