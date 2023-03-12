using DataLayer.EF.Contexts;

namespace DataLayer.EF.Repositories
{
    public class BaseRepository
    {
        protected readonly DataContext _dataContext;

        public BaseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}