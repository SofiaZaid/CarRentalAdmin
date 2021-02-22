using System.Linq;

namespace CarRentalAdministration.DAL
{
    public class GenericRepository
    {

        private readonly CarRentalDBContext _context;

        public GenericRepository(CarRentalDBContext context)
        {
            _context = context;
        }

        public void Create<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }
        
        public void Edit<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }

        public IQueryable<T> DataSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        //public ICollection<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class
        //{
        //    return _context.Set<T>().Where(predicate).ToList<T>();
        //}

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
