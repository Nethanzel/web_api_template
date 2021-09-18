/* docs */
/* to init a new instace of this clase use "private IGenericRepository<Entity> repository = null;" */

/* for more details go to https://dotnettutorials.net/lesson/generic-repository-pattern-csharp-mvc/ */

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace dataCoreService.Data
{
    public class DBManager<T> : IRepositoryInterface<T> where T : class
    {

        private DBContext _context = null;
        private DbSet<T> DataSet = null;

        public DBManager(DBContext _context)
        {
            this._context = _context;
            DataSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return DataSet.ToList();
        }
        public T GetById(object id)
        {
            return DataSet.Find(id);
        }
        public void Insert(T obj)
        {
            DataSet.Add(obj);
        }
        public void Update(T obj)
        {
            DataSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            /* Remenber that here you can make your modifications as you want */
        }
        public void Delete(object id)
        {
            T existing = DataSet.Find(id);
            DataSet.Remove(existing);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
