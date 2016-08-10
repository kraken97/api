using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models
{
    public class NavLink
    {
        public int NavLinkId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        
        public int? ParentLinkID { get; set; }
        public int? PageId { get; set; }
        [ForeignKey("PageId")]
        public Page Page { get; set; }
        public int? Position { get; set; }
        public override string ToString()
        {
            return $"NavLink: {NavLinkId} Title: {Title} ParentLinkID: {ParentLinkID}  PageID: {PageId}  Position: {Position}";
        }

    }
}
