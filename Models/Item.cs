using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public int ChecklistId { get; set; }
        public Checklist Checklist { get; set; }
    }
}
