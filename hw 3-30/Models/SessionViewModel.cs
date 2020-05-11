using hw_3_30b.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hw_3_30.Models
{
    public class SessionViewModel
    {
        public List<Post> Posts { get; set; }
        public List<int> Ids = new List<int>();
    }
}
