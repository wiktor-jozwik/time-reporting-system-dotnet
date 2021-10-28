using System;
using System.Collections.Generic;

namespace NtrTrs.Models
{
    public class MonthModel
    {
        public string frozen { get; set; }

        public List<EntryModel> entries { get; set; }

        public List<AcceptedEntryModel> accepted { get; set; }
    }
}
