using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Profile
    {
        public IEnumerable<Entry> PublicEntries { get; set; }
        public IEnumerable<Entry> PrivateEntries { get; set; }
        public string Author { get; set; }
    }
}
