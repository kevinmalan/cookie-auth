using DataLayer.EF.Contexts;

namespace DataLayer.EF.Repositories
{
    public class BaseRepository
    {
        protected readonly DataContext DataContext;

        public BaseRepository(DataContext dataContext)
        {
            DataContext = dataContext;
        }
    }
}