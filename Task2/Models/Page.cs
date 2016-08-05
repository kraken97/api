using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication16.Models
{
    public class Page
    {
    
        public int PageId { get; set; }
        [Required]
        [Url]
        [MaxLength(500)]
        [Unique]
        public string UrlName { get; set; }
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }
        public override string ToString()
        {
            return $"PageID: {PageId}  UrlName: {UrlName} Description: {Description}  Content:{Content}  AddedDate: {AddedDate} ";
        }
    }
}
