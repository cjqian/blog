using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Entry
    {
        [Required]
        [StringLength(100, ErrorMessage = "The title must have at least (1) character and must not exceed (100) characters.", MinimumLength = 1)]
        public string Title { get; set; }
        public string Author { get; set; }
        public int ID { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The content must have at least (4) characters and must not exceed (10000) characters.", MinimumLength = 4)]
        public string Content { get; set; }

        [Display(Name = "Make post public?")]
        public Boolean IsPublic { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
