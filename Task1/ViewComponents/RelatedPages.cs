
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task1.Models;

namespace Task1.ViewComponents
{
    public class RelatedPagesViewComponent : ViewComponent
    {
        private readonly IRelPagesRepository db;

        public RelatedPagesViewComponent(IRelPagesRepository context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageID)
        {
            var items =  await GetItemsAsync(pageID);
            return View(items);
        }
        private Task<List<Page>> GetItemsAsync(int pageID)
        {
            return Task<List<Page>>.Run(()=> db.GetAll().Where(r=>r.Page1Id==pageID||r.Page2Id==pageID).Select(r=>r.Page1Id==pageID?r.Page2:r.Page1).ToList());
        }       
    }
}