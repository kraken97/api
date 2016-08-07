using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Controllers
{
    public class NavLinksController : Controller
    {
        private readonly INavRepository _repo;
        private readonly IPageRepository _prepo;

        
        public NavLinksController(INavRepository repo,IPageRepository prepo)
        {
        
            _prepo=prepo;
            _repo=repo;
        }

        // GET: NavLinks
        public async Task<IActionResult> Index(string prop = "id", bool order = true, int take = 10, int skip = 0)
        {

            ViewBag.Order = !order;
            var query = _repo.GetAll();
            ViewBag.Count = query.Count();
            var res = await Task<dynamic>.Run(()=>Utils.Sort<NavLink>(query, Utils.GetKeyForNavLink(prop.ToLower()), order)
                               .TakeSkip(take, skip).ToList());
            return View(res);
        }

        // GET: NavLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navLink = await Task<NavLink>.Run(()=>_repo.Get(id.Value));
            if (navLink == null)
            {
                return NotFound();
            }

            return View(navLink);
        }

        // GET: NavLinks/Create
        public IActionResult Create()
        {
            ViewData["PageId"] = new SelectList(_prepo.GetAll(), "PageId", "PageId");

        ViewData["ParentLinks"] =   GetParentsLinks(null);
            return View();
        }

        // POST: NavLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NavLinkId,ParentLinkID,PageId,NavLinkId,Position,Title")] NavLink navLink)
        {
            if (ModelState.IsValid)
            {
               await Task.Run(()=> _repo.Add(navLink));
        
                return RedirectToAction("Index");
            }
            ViewData["PageId"] = new SelectList(_prepo.GetAll(), "PageId", "PageId", navLink.PageId);
            ViewData["ParentLinks"] =         ViewData["ParentLinks"] =  GetParentsLinks(navLink.PageId);
            return View(navLink);
        }

        // GET: NavLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navLink = await Task<NavLink>.Run(()=>_repo.Get(id.Value));
            if (navLink == null)
            {
                return NotFound();
            }
            ViewData["PageId"] = new SelectList(_prepo.GetAll(), "PageId", "PageId", navLink.PageId);
                     ViewData["ParentLinks"] = GetParentsLinks(navLink.PageId);
            return View(navLink);
        }

        // POST: NavLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NavLinkId,ParentLinkID,PageId,NavLinkId,Position,Title")] NavLink navLink)
        {
            if (id != navLink.NavLinkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await Task.Run(()=>_repo.Update(navLink));
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavLinkExists(navLink.NavLinkId))
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
            ViewData["PageId"] = new SelectList(_prepo.GetAll(), "PageId", "PageId", navLink.PageId);
            ViewData["ParentLinks"] =  GetParentsLinks(navLink.PageId);
            return View(navLink);
        }

        private IEnumerable<SelectListItem> GetParentsLinks(int? id ){
            return   new SelectList(_repo.GetAll().Where(r=>r.ParentLinkID==null), "NavLinkId", "NavLinkId",id)
            .Append(new SelectListItem(){Selected=true,Value="",Text="Without parent"});
        }
        // GET: NavLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navLink = await Task<NavLink>.Run(()=>_repo.Get(id.Value));
            if (navLink == null)
            {
                return NotFound();
            }

            return View(navLink);
        }

        // POST: NavLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           await Task.Run(()=>_repo.Remove(id));
           return RedirectToAction("Index");
        }

        private bool NavLinkExists(int id)
        {
            return _repo.GetAll().Any(e => e.NavLinkId == id);
        }

       
    }
}
