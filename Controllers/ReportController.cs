using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.ViewModels;
using NtrTrs.Services;

namespace NtrTrs.Controllers
{
    public class ReportController : Controller
    {
        private readonly UserService _userService;
        private readonly MonthEntryService _monthEntryService;
        private readonly EntryService _entryService;

        public ReportController(
                        UserService userService, 
                        MonthEntryService monthEntryService,
                        EntryService entryService) 
        {
            _userService = userService;
            _monthEntryService = monthEntryService;
            _entryService = entryService;
        }
        public IActionResult Index(string dateString = null) {
            DateTime dateTime;

            if (dateString == null) {
                dateTime = DateTime.Now;
            } else {
                try {
                    dateTime = _entryService.GetRequestedDateTime(dateString);
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
                    string cause = "Month is already accepted. Try again!";
                    ViewData["Cause"] = cause;
                    return View("BadRequest");
                }

            monthData.Frozen = true;
            ViewData["Frozen"] = monthData.Frozen;

            _monthEntryService.FreezeMonth(monthData);

            monthlyReport = _monthEntryService.GetMontlyReport(monthData);

            ViewData["DateTime"] = Date;
            ViewData["UserName"] = loggedUser.Name;

            return View("Index", monthlyReport);
        }
    }
}