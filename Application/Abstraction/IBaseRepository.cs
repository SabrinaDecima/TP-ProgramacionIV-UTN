using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstraction;

public interface IBaseRepository<T> where T : BaseEntity 
{
    List<T> GetAll();
    T? GetById(int id);
    bool Create(T entity);
    bool Update(T entity);
    bool Delete(T entity);
    List<T> GetByCriteria(Expression<Func<T, bool>> expression);
}