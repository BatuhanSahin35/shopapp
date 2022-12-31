using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.entity
{
    public class List
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<ListItem> ListItems { get; set; }
    }
}