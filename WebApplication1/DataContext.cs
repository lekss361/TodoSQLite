namespace ConsoleApp1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DataContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<UserItem> UserItems { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    public DbSet<T> DbSet<T>() where T : class
    {
        return Set<T>();
    }

    public new IQueryable<T> Query<T>() where T : class
    {
        return Set<T>();
    }
}
