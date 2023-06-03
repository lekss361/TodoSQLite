namespace TodoSQLite.Data;

using System;
using System.Linq;
using System.Linq.Expressions;
using TodoSQLite.Models;

public interface IDbRepository
{
    int Add<T>(T newEntity) where T : class, IBase;
    IQueryable<T> Get<T>() where T : class, IBase;
    IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IBase;
    IQueryable<T> GetAll<T>() where T : class, IBase;
    void Remove<T>(T entity) where T : class, IBase;
    int SaveChangesAsync();
    void Update<T>(T entity) where T : class, IBase;
}