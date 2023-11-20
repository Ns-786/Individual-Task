using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FortniteCompetition.Models
{
    public class CompetitionPost
    {
        public long Id { get; set; }

        public string _key;

        public string Key
        {
            get
            {
                if (_key == null)
                {
                    _key = Regex.Replace(Name.ToLower(), "[^a-z0-9]", "-");
                }
                return _key;
            }
            set { _key = value; }
        }

        [Display(Name = "Competition Name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "Name must be between 5 and 100 characters long")]
        public string Name { get; set; }

        [Display(Name = "Competition Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Competition Location")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5,
           ErrorMessage = "Location must be between 5 and 100 characters long")]
        public string Location { get; set; }

        [Required]
        [MinLength(100, ErrorMessage = "Blog posts must be at least 100 characters long")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

         public string Author { get; set; }
    }
}
