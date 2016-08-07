
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task2.Models;

namespace Task2.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly INavRepository db;

        public NavBarViewComponent(INavRepository context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var res =await  Task<dynamic>.Run(()=>db.GetAll().Where(r => r.ParentLinkID == null)
                .Select(r => new Tuple<NavLink, List<NavLink>>(r, db.GetAll()
                    .Where(data => data.ParentLinkID == r.NavLinkId).ToList())));
            return View(res);
        }

    }


}