using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models
{
    public class RelatedPages
    {
        public int ID { get; set; }
        public int? Page1Id { get; set; }
        [ForeignKey("Page1Id")]
        public Page Page1 { get; set; }
        public int? Page2Id { get; set; }
        [ForeignKeyAttribute("Page2Id")]
        public Page Page2 { get; set; }
        public override string ToString()
        {
            return $"Page1ID: { Page1Id} Page2Id: {Page2Id}";
        }
    }
}
