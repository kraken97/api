using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task1.Models;
using Task1;
using Task1.UtilsModels;
using System.Net.Http;
using JSerializer = Newtonsoft.Json.JsonConvert;
using Microsoft.AspNetCore.Authorization;

namespace Task1.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;
        private readonly IPageRepository _repo;
        private readonly IRelPagesRepository _related;
        private const string _domain = "localhost:5000";
        private static readonly string _apiUrl = $"http://{_domain}/api/page/";
        private readonly HttpClient _httpClient;


        public PagesController(IPageRepository repo, IRelPagesRepository related, ILogger<PagesController> logger)
        {
            _related = related;
            _repo = repo;
            _logger = logger;
            _httpClient = new HttpClient();


        }
        //maybe should create new instance of httpclient or clear headers
        private async Task<Page> GetPage(int id)
        {

            string query = PagesController._apiUrl + id;

            var res = await (await _httpClient.GetAsync(query)).Content.ReadAsStringAsync();
            if (res != null)
            {
                _logger.LogInformation("Page accepted from Rest api");
            }
            else
            {
                _logger.LogWarning("Failed to get page from Rest apo");
            }
            var page = JSerializer.DeserializeObject<Page>(res);
            return page;

        }

        // GET: Pages
        public async Task<IActionResult> Index(string title, string prop = "id", bool order = true, int take = 5, int skip = 0)
        {
            string param = $@"?order={order}&prop={prop}&take={take}&skip={skip}&title={title}";
            string query = PagesController._apiUrl + param;

            var res = await (await _httpClient.GetAsync(query)).Content.ReadAsStringAsync();
            if (res != null)
            {
                _logger.LogInformation("Page accepted from Rest api");
            }
            else
            {
                _logger.LogError("Failed to get page from Rest api");
            }
            var pages = JSerializer.DeserializeObject<List<Page>>(res);
            ViewBag.Order = !order;
            ViewBag.Take = 5;
            var countJson =await (await _httpClient.GetAsync(_apiUrl+"count")).Content.ReadAsStringAsync();
            _logger.LogInformation(countJson);
            ViewBag.Count= int.Parse(countJson);
            _logger.LogInformation(pages.Count() + "");

            return View(pages.ToList());
        }


        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await GetPage(id.Value);
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
            var related = _related.GetAll().Where(r => r.Page1Id == id || r.Page2Id == id).Select(r => r.Page1Id == id ? r.Page2Id : r.Page1Id).ToList();

            var res = _repo.GetAll()
                    .Where(r => r.PageId != id)
                        .Select(r => new RelPagesView()
                        {
                            RelPageId = r.PageId,
                            IsSelected = related.Contains(r.PageId),
                            Name = r.UrlName
                        })
                            .ToList();
            return View(res);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRelations(int id, List<RelPagesView> list)
        {

            if (ModelState.IsValid)
            {
                _logger.LogInformation(id.ToString());
                var relPages = _related.GetAll();
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    var relpage = relPages.FirstOrDefault(r => (r.Page1Id == id && r.Page2Id == item.RelPageId) || (r.Page1Id == item.RelPageId && r.Page2Id == id));

                    if (relpage == null && item.IsSelected)
                    {
                        _related.Add(new RelatedPages() { Page1Id = id, Page2Id = item.RelPageId });
                    }
                    else if (relpage != null && !item.IsSelected)
                    {
                        _related.Remove(relpage);
                    }
                }
                return RedirectToAction("Index");
            }
            var related = _related.GetAll().Where(r => r.Page1Id == id || r.Page2Id == id).Select(r => r.Page1Id == id ? r.Page2Id : r.Page1Id).ToList();

            var res = _repo.GetAll()
                    .Where(r => r.PageId != id)
                        .Select(r => new RelPagesView()
                        {
                            RelPageId = r.PageId,
                            IsSelected = related.Contains(r.PageId),
                            Name = r.UrlName
                        })
                            .ToList();

            return View(res);
        }
        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create([Bind("PageId,Title,AddedDate,Content,Description,UrlName")] Page page)
        {
            if (ModelState.IsValid)
            {
                var pageAsString = JSerializer.SerializeObject(page);
                StringContent theContent = new StringContent(pageAsString, System.Text.Encoding.UTF8, "application/json");
               await  _httpClient.PostAsync(_apiUrl, theContent);

                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await GetPage(id.Value);
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
        public async Task<IActionResult> Edit(int id, [Bind("PageId,AddedDate,Title,Content,Description,UrlName")] Page page)
        {
            if (id != page.PageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var query = _apiUrl + id;
                    var pageAsString = JSerializer.SerializeObject(page);
                    StringContent theContent = new StringContent(pageAsString, System.Text.Encoding.UTF8, "application/json");
                    await _httpClient.PutAsync(query, theContent);

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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await GetPage(id.Value);
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
                var pageAsString = JSerializer.SerializeObject(page);
                StringContent theContent = new StringContent(pageAsString, System.Text.Encoding.UTF8, "application/json");
                _httpClient.DeleteAsync(_apiUrl + id);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {

                return this.RedirectToAction("FkError");
            }

            return RedirectToAction("Index");
        }
        public IActionResult FkError()
        {
            return View();
        }
        private bool PageExists(int id)
        {
            return GetPage(id).Result != null;
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
