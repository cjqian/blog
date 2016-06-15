using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
    public class Entry
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int ID { get; set; }
        public string Content { get; set; }

        public Boolean IsPublic { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
