using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NtrTrs.Services
{
    public class EntryService
    {
        private NtrTrsContext _context { get; set; }
        public EntryService(NtrTrsContext context) {
            _context = context;
        }

        public void CreateEntry(Entry entry)
        {
            _context.Entries.Add(entry);
            _context.SaveChanges();
        }

        // public GetEntryByDate(Date date)
        // {

        // }
    }
}