using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.webui.Models
{
    public class ListModel
    {
        public int ListId { get; set; }
        public List<ListItemModel> ListItems { get; set; }
    }

    public class ListItemModel
    {
        public int ListItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}