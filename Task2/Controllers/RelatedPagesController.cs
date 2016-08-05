using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication16.Models;

namespace WebApplication16.Controllers
{
    public class RelatedPagesController : Controller
    {
        private readonly SqliteContext _context;
        private readonly  ILogger<RelatedPages> _logger;

        public RelatedPagesController(SqliteContext context,ILogger<RelatedPages> logger)
        {
            _logger=logger;
            _context = context;    
        }

        // GET: RelatedPages
        public async Task<IActionResult> Index()
        {
            var sqliteContext = _context.RelatedPages.Include(r => r.Page1).Include(r => r.Page2);
            return View(await sqliteContext.ToListAsync());
        }

        // GET: RelatedPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.ID == id);
            if (relatedPages == null)
            {
                return NotFound();
            }

            return View(relatedPages);
        }

        // GET: RelatedPages/Create
        public IActionResult Create(){

                       ViewData["Pages"] = new SelectList(_context.Pages, "PageId", "UrlName");

             ViewData["PagesCount"] = _context.Pages.Count();

            return View();
        }

        // POST: RelatedPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Page1Id,Page2Id")] RelatedPages relatedPages)
        {
            _logger.LogInformation(relatedPages.ToString());
            
            if (ModelState.IsValid)
            {
                _context.Add(relatedPages);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["Page1Id"] = new SelectList(_context.Pages, "PageId", "UrlName", relatedPages.Page1Id);
            ViewData["Page2Id"] = new SelectList(_context.Pages, "PageId", "UrlName", relatedPages.Page2Id);
            return View(relatedPages);
        }

        // GET: RelatedPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.ID == id);
            if (relatedPages == null)
            {
                return NotFound();
            }
            ViewData["Page1Id"] = new SelectList(_context.Pages, "PageId", "UrlName", relatedPages.Page1Id);
            ViewData["Page2Id"] = new SelectList(_context.Pages, "PageId", "UrlName", relatedPages.Page2Id);
            return View(relatedPages);
        }

        // POST: RelatedPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Page1Id,Page2Id")] RelatedPages relatedPages)
        {
            if (id != relatedPages.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relatedPages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelatedPagesExists(relatedPages.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["Page1Id"] = new SelectList(_context.Pages, "PageId", "UrlName", relatedPages.Page1Id);
            ViewData["Page2Id"] = new SelectList(_context.Pages, "PageId", "Url", relatedPages.Page2Id);
            return View(relatedPages);
        }

        // GET: RelatedPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.ID == id);
            if (relatedPages == null)
            {
                return NotFound();
            }

            return View(relatedPages);
        }

        // POST: RelatedPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.ID == id);
            _context.RelatedPages.Remove(relatedPages);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RelatedPagesExists(int id)
        {
            return _context.RelatedPages.Any(e => e.ID == id);
        }
    }
}
