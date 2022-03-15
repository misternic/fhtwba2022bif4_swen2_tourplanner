namespace TourPlanner.DAL.Repositories;

public interface IRepository<T> where T :class
{
    bool Delete(Guid id);
    IEnumerable<T> Get();
    T GetById(Guid id);
    bool Insert(T tour);
    bool Update(T entity);
}