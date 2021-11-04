using System;
using System.Collections.Generic;

namespace NtrTrs.Models
{

    public class AcitvityList {
        public List<ActivityModel> Activities { get; set;}
    }
    public struct Subactivity {
        public string Code { get; set; }
    }
    public class ActivityModel
    {
        public string Code { get; set; }
        public string Manager { get; set; }
        public int Budget { get; set; }
        public string Active { get; set; }
        public List<Subactivity> Subactivities { get; set; }
    }
}
