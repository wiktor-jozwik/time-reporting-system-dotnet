using System;
using System.Collections.Generic;

namespace NtrTrs.Models
{
    public class MonthModel
    {
        public string Frozen { get; set; }

        public List<EntryModel> Entries { get; set; }

        public List<AcceptedEntryModel> Accepted { get; set; }
    }
}
