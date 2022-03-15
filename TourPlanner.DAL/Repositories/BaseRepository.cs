namespace TourPlanner.DAL.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    internal readonly DbContext Context;
    
    protected BaseRepository(DbContext context)
    {
        Context = context;
    }

    public virtual bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<T> Get()
    {
        throw new NotImplementedException();
    }

    public virtual T GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual bool Insert(T tour)
    {
        throw new NotImplementedException();
    }

    public virtual bool Update(T entity)
    {
        throw new NotImplementedException();
    }
}