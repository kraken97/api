using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task2.Models;
using Task2;

namespace Task2.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;
        private readonly IPageRepository _repo;

        public PagesController(IPageRepository repo, ILogger<PagesController> logger)
        {
            _repo = repo;
            _logger = logger;

        }

        // GET: Pages
        public IActionResult Index(string url, string title, string prop = "id", bool order = true, int take = 5, int skip = 0)
        {
            ViewBag.Order = !order;

            _logger.LogInformation("index:" + ViewBag.Order as String);


            IEnumerable<Page> query = _repo.GetAll();

            if (url != null)
            {
                query = query.Where(page => page.UrlName.Contains(url));
            }
            if (title != null)
            {
                query = query.Where(page => page.Title.Contains(title));
            }
            ViewBag.Count = query.Count();

            var res = Utils.Sort<Page>(query, Utils.GetKeyForPageSorting(prop.ToLower()), order)
                               .TakeSkip(take, skip).ToList();


            return View(res);
        }


        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await Task<Page>.Run(()=>_repo.Get(id.Value));
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Pages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PageId,Title,AddedDate,Content,Description,UrlName")] Page page)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(page);
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Pages/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = _repo.Get(id.Value);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PageId,AddedDate,Title,Content,Description,UrlName")] Page page)
        {
            if (id != page.PageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(page);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.PageId))
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
            return View(page);
        }

        // GET: Pages/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = _repo.Get(id.Value);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var page = _repo.Get(id);
            _repo.Remove(page);
            return RedirectToAction("Index");
        }

        private bool PageExists(int id)
        {
            return _repo.Get(id) != null;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult Unique(string UrlName, string initialUrl)
        {
            _logger.LogInformation("Validation Uniquness for url " + UrlName);
            if (!UrlName.Equals(initialUrl))
            {
                var res = _repo.GetAll().Any(p => p.UrlName.Equals(UrlName));
                if (res)
                {
                    return Json(data: false);
                }
            }


            return Json(data: true);
        }
    }
}
