using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NtrTrs.Models
{
    public class ManagerViewModel
    {
        public DateTime Date { get; set; }

        [DisplayName("Total time spent")]
        public int TotalTime { get; set; }

        [DisplayName("Total accept time")]
        [Required(ErrorMessage = "Please enter accepted time")]  
        public int AcceptedTime { get; set; }

    }
}
