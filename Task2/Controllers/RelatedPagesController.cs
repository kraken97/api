using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task2.Models;

namespace Task2.Controllers
{
    public class RelatedPagesController : Controller
    {
        private readonly IRelPagesRepository _context;
        private readonly  ILogger<RelatedPages> _logger;

        public RelatedPagesController(IRelPagesRepository context,ILogger<RelatedPages> logger)
        {
            _logger=logger;
            _context = context;    
        }

        // GET: RelatedPages
        public IActionResult Index(string prop = "id", bool order = true, int take = 5, int skip = 0)
        {

               ViewBag.Order = !order;
            var query = _context.GetAll();
            ViewBag.Count = query.Count();
            var res = Utils.Sort<RelatedPages>(query, Utils.GetKeyForRelPages(prop), order)
                               .TakeSkip(take, skip).ToList();
            
            return View(res);
        }

       
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedPages =  _context.Get(id.Value);
            if (relatedPages == null)
            {
                return NotFound();
            }

            return View(relatedPages);
        }

        // POST: RelatedPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var relatedPages = this._context.Get(id);
            _context.Remove(relatedPages);
            return RedirectToAction("Index");
        }

     
    }
}
