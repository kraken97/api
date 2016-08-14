using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task3;
using Task3.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Task3.Controllers
{
    [Route("/View")]
    [Authorize]
    public class ViewController : Controller
    {

        private readonly IPageRepository _repo;
      public ViewController(IPageRepository repo){
        
            this._repo=repo;

      }


        [HttpGet("{urlName}")]
        public async Task<IActionResult> Index(string urlName)
        {
           Page page =  await Task<Page>.Run(()=>_repo.GetAll().SingleOrDefault(r=>r.UrlName.Equals(urlName)));

           if(page==null){
               return  this.NotFound("file not found");
           }
            return View(page);
        }
    }
}
