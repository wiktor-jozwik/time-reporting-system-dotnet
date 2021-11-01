using System;
using System.ComponentModel.DataAnnotations;

namespace NtrTrs.Models
{
    public class EntryModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        [Required(ErrorMessage = "Please enter date")]  
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter code of project")]  
        public string Code { get; set; }
        public string Subcode { get; set; }

        [Required(ErrorMessage = "Please enter time")]  
        public int Time { get; set; }
        public string Description { get; set; }

    }
}
