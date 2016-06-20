using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ScriptManager : HashSet<String>
    {

        public ScriptManager()
        {
            JsFiles = new HashSet<string>();
        }

        public HashSet<string> JsFiles { get; }

    }
}
