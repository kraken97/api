using System;
using System.Collections.Generic;
using Task1.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Task1;

public class RelPagesRepository : IRelPagesRepository
    {

        private readonly SqliteContext _context;

        public RelPagesRepository(SqliteContext context){
            this._context=context;

        }
        public void Add(RelatedPages page)
        {
            _context.Add(page);
            _context.SaveChanges();
        }

        public RelatedPages Get(int key)
        {
            return _context.RelatedPages.SingleOrDefault(a=>a.ID==key);
        }

        public IEnumerable<RelatedPages> GetAll()
        {
            return _context.RelatedPages;
        }

        public void Remove(RelatedPages key)
        {
            _context.Remove(key);
            _context.SaveChanges();
        }

        public void Remove(int key)
        {

            _context.Remove(Get(key));
            _context.SaveChanges();
        }

        public void Update(RelatedPages page)
        {
            _context.Update(page);
            _context.SaveChanges();
        }
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
            return _context.NavLinks.Include(n => n.Page);
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
          var res= _context.Pages.SingleOrDefault(r=>r.PageId==key);
          _context.SaveChanges();
          return res;
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

    
        //filter data by title then sort and then  skip   and take ;
         public  IEnumerable<Page> SortAndTake(string titleFilter,string propOder,bool order,int take,int skip)
        {
            var filteredData= string.IsNullOrEmpty(titleFilter)?_context.Pages:this._context.Pages.Where(r=>r.Title.Contains(titleFilter));
         return Utils.Sort<Page>(filteredData,Utils.GetKeyForPageSorting(propOder.ToLower()),order).Skip(skip).Take(take);
        }

        public void Update(Page page)
        {
            _context.Pages.Update(page);
            _context.SaveChanges();
        }

    }
