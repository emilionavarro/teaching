using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed.Classes
{
    class Counter
    {
        public static Dictionary<string, int> count { get; set; } = new Dictionary<string, int>();
    }

    class Cache
    {
        public static Dictionary<string, object> instance { get; set; } = new Dictionary<string, object>();
    }
}
