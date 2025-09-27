using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly GymDbContext _context;
    private readonly DbSet<T> _dbSet;

    protected BaseRepository(GymDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public virtual T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public virtual bool Create(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();

        return true;
    }

    public virtual bool Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();

        return true;
    }

    public virtual bool Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();

        return true;
    }

    public virtual List<T> GetByCriteria(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression).ToList();
    }
}
