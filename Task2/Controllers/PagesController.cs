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
using Task2.UtilsModels;

namespace Task2.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;
        private readonly IPageRepository _repo;
        private readonly IRelPagesRepository _related;

        public PagesController(IPageRepository repo,IRelPagesRepository related, ILogger<PagesController> logger)
        {
            _related=related;
            _repo = repo;
            _logger = logger;

        }

        // GET: Pages
        public IActionResult Index(string url, string title, string prop = "id", bool order = true, int take = 5, int skip = 0)
        {
            ViewBag.Order = !order;

            ViewBag.Take=5;
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

            var page = await Task<Page>.Run(() => _repo.Get(id.Value));
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

        public IActionResult AddRelations(int id)
        {
            var related=_related.GetAll().Where(r=>r.Page1Id==id||r.Page2Id==id).Select(r=>r.Page1Id==id?r.Page2Id:r.Page1Id).ToList();

            var res = _repo.GetAll()
                    .Where(r=>r.PageId!=id)
                        .Select(r => new RelPagesView() {
                                 RelPageId = r.PageId,
                                 IsSelected = related.Contains(r.PageId),
                                 Name = r.UrlName })
                            .ToList();
            return View(res);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRelations(int id,List<RelPagesView> list)
        {

            if(ModelState.IsValid){
                _logger.LogInformation(id.ToString());
                var relPages=_related.GetAll();
                for (int i = 0; i < list.Count; i++)
                {
                    _logger.LogInformation(i+"__________________");
                    var item = list[i];
                  var relpage=  relPages.FirstOrDefault(r=>(r.Page1Id==id&&r.Page2Id==item.RelPageId)||(r.Page1Id==item.RelPageId&&r.Page2Id==id));

                 if(relpage==null&&item.IsSelected){
                     _related.Add(new RelatedPages(){Page1Id=id,Page2Id=item.RelPageId});
                 }
                 else if(relpage!=null&&!item.IsSelected){
                     _related.Remove(relpage);
                 }
                }
                 return RedirectToAction("Index");
            }
            var related=_related.GetAll().Where(r=>r.Page1Id==id||r.Page2Id==id).Select(r=>r.Page1Id==id?r.Page2Id:r.Page1Id).ToList();

            var res = _repo.GetAll()
                    .Where(r=>r.PageId!=id)
                        .Select(r => new RelPagesView() {
                                 RelPageId = r.PageId,
                                 IsSelected = related.Contains(r.PageId),
                                 Name = r.UrlName })
                            .ToList();
            _logger.LogInformation((res==null)+"--------------");
            return View(res);
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
            try
            {
                     _repo.Remove(page);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                
                return  this.RedirectToAction("FkError");
            }
            _repo.Remove(page);
            return RedirectToAction("Index");
        }
        public IActionResult FkError(){
            return View();
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
