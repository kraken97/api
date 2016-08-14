using System.Collections.Generic;
using Task1.Models;

namespace Task1
{
    public interface IRepo<T>
    {
        void Add(T page);
        IEnumerable<T> GetAll();
        T Get(int key);
        void Remove(int key);
        void Remove(T key);
        void Update(T page);
    }
    public interface IPageRepository : IRepo<Page>
    {
        IEnumerable<Page> SortAndTake(string titleFilter, string propOder, bool order, int take, int skip);
    }

    public interface INavRepository : IRepo<NavLink> { }

    public interface IRelPagesRepository : IRepo<RelatedPages> { }


}


