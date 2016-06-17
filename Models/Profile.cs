using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
    public class Profile
    {
        public IEnumerable<Entry> entries;
        public string author;
    }
}
