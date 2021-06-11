using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class Checklist
    {
        public int ChecklistId { get; set; }
        public string ChecklistName { get; set; }
        public string Description { get; set; }
        public List<Item> Items { get; set; }
    }
}
