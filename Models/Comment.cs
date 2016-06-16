using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
    public class Comment
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int EntryID { get; set; }
        public int ID { get; set; }
    }
}
