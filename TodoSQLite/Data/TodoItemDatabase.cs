using SQLite;
using TodoSQLite.Models;

namespace TodoSQLite.Data;

public class TodoItemDatabase : BaseItem
{
    public TodoItemDatabase()
    {
    }

    public async Task<List<TodoItem>> GetItemsAsync(int id)
    {
        await Init();
        return await Database.Table<TodoItem>().Where(x=>x.UserId==id).ToListAsync();
    }

    public async Task<List<TodoItem>> GetItemsNotDoneAsync()
    {
        await Init();
        return await Database.Table<TodoItem>().Where(t => t.Status == Statuses.Выполнен).ToListAsync();

        // SQL queries are also possible
        //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    }

    public async Task<TodoItem> GetItemAsync(int id)
    {
        await Init();
        return await Database.Table<TodoItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(TodoItem item)
    {
        await Init();
        if (item.Id != 0)
        {
            return await Database.UpdateAsync(item);
        }
        else
        {
            return await Database.InsertAsync(item);
        }
    }

    public async Task<int> DeleteItemAsync(TodoItem item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }
}