using System;
using System.Collections.Generic;

namespace NtrTrs.Models
{
    public struct Subactivity {
        public string code { get; }
    }
    public class ActivityModel
    {
        public string code { get; set; }
        public string manager { get; set; }
        public int budget { get; set; }
        public bool active { get; set; }
        public List<Subactivity> subactivities { get; set; }
    }
}
