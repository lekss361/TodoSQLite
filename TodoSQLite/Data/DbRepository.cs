namespace TodoSQLite.Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoSQLite.Models;

public class DbRepository : IDbRepository
{
    private readonly DataContext _context;

    public DbRepository(DataContext context)
    {
        _context = context;
    }

    public IQueryable<T> Get<T>() where T : class, IBase
    {
        return _context.Set<T>().AsQueryable();
    }

    public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IBase
    {
        return _context.Set<T>().Where(selector).AsQueryable();
    }

    public int Add<T>(T newEntity) where T : class, IBase
    {
        var c = _context.Set<T>().Add(newEntity);
        return c.Entity.Id;
    }

    public void Remove<T>(T entity) where T : class, IBase
    {
        _context.Set<T>().Remove(entity);
    }

    public void Update<T>(T entity) where T : class, IBase
    {
        _context.Set<T>().Update(entity);
    }
    public int SaveChangesAsync()
    {
        return _context.SaveChanges();
    }

    public IQueryable<T> GetAll<T>() where T : class, IBase
    {
        return _context.Set<T>().AsQueryable();
    }

}

