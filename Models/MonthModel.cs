using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NtrTrs.Models
{
    public class MonthModel
    {
        [DefaultValue(false)]
        public bool Frozen { get; set; }

        public List<EntryModel> Entries { get; set; }

        public List<AcceptedEntryModel> Accepted { get; set; }
    }
}
