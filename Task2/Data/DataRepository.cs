using System;
using System.Collections.Generic;
using WebApplication16.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApplication16{
   public interface IPageRepository{
         void Add(Page page);
        IEnumerable<Page> GetAll();
        Page Get(int key);
        void Remove(int key);
        void Remove(Page key);
        void Update(Page page);
        IEnumerable<Page> SortBy(string prop,bool order);
    }

     public interface INavRepository{
         void Add(NavLink page);
        IEnumerable<NavLink> GetAll();
        NavLink Get(int key);
        void Remove(int key);
        void Remove(NavLink key);
        void Update(NavLink page);
    }



    public class NavLinksRepository : INavRepository
    {
        private readonly SqliteContext _context;
        public NavLinksRepository(SqliteContext context){
            this._context=context;
        }
        public void Add(NavLink page)
        {
            _context.Add(page);
            _context.SaveChanges();
        }

        public NavLink Get(int key)
        {
            return _context.NavLinks.SingleOrDefault(r=>r.NavLinkId==key);
            
        }

        public IEnumerable<NavLink> GetAll()
        {
            return _context.NavLinks.Include(n => n.Page).Include(n => n.ParentLink);;
        }

        public void Remove(NavLink key)
        {
            _context.Remove(key);
            _context.SaveChanges();
        }

        public void Remove(int key)
        {
         _context.NavLinks.Remove(Get(key)); 
         _context.SaveChanges();  
        }

        public void Update(NavLink page)
        {
            _context.Update(page);
            _context.SaveChanges();
        }
    }

    public class PageRepository :IPageRepository{
        private readonly SqliteContext _context;
        public PageRepository(SqliteContext context){
            _context=context;
        }

        public void Add(Page page)
        {
            _context.Pages.Add(page);
            _context.SaveChanges();
        }

        public Page Get(int key)
        {
          return  _context.Pages.SingleOrDefault(r=>r.PageId==key);
        }

        public IEnumerable<Page> GetAll()
        {
            return _context.Pages;
        }

        public void Remove(Page key)
        {
            Remove(key.PageId);
            
        }

        public void Remove(int key)
        {
           _context.Pages.Remove(Get(key));
           _context.SaveChanges();
        }

        public IEnumerable<Page> SortBy(string prop,bool order)
        {
         return Utils.Sort<Page>(_context.Pages,Utils.GetKeyForPageSorting(prop.ToLower()),order)  ; 
        }

        public void Update(Page page)
        {
            _context.Update(page);
            _context.SaveChanges();
        }
    }



}