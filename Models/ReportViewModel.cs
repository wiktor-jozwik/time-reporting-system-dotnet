using System.ComponentModel;

namespace NtrTrs.Models
{
    public class ReportViewModel
    {
        public string Code { get; set; }

        [DisplayName("Total time spent")]
        public int TotalTime { get; set; }

    }
}
