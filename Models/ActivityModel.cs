using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NtrTrs.Models
{

    public class ActivityList {
        public List<ActivityModel> Activities { get; set;}
    }
    public struct Subactivity {
        public string Code { get; set; }
    }
    public class ActivityModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter code")]  
        public string Code { get; set; }
        public string Manager { get; set; }

        [Required(ErrorMessage = "Please enter budget")]  
        public int Budget { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }
        public List<Subactivity> Subactivities { get; set; }
    }
}
