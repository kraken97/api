using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Task3.Models
{
    public class Page
    {
    
        public int PageId { get; set; }
        [Required]
        [Remote("Unique", "Pages",AdditionalFields="InitialUrl", ErrorMessage = "UrlName must be unique")]
        
        [MaxLengthAttribute(15)]
        public string UrlName { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
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
