using System.Collections.Generic;
using System.ComponentModel;

namespace NtrTrs.ViewModels
{
    public class ReportViewModel
    {
        public string Code { get; set; }

        [DisplayName("Total time spent")]
        public int TotalTime { get; set; }

        [DisplayName("Total time accepted")]
        public int AcceptedTime { get; set; }

    }
}
