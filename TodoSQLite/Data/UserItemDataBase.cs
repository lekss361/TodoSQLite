using SQLite;
using TodoSQLite.Models;

namespace TodoSQLite.Data
{
    public class BaseItem
    {
        public SQLiteAsyncConnection Database;

        public async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }
    }
    public class UserItemDataBase : BaseItem
    {
        public UserItemDataBase()
        {
        }


        public async Task<List<UserItem>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<UserItem>().ToListAsync();
        }

        public async Task<List<UserItem>> GetItemsNotDoneAsync()
        {
            await Init();
            return await Database.Table<UserItem>().Where(t => t.Role == Role.Manager).ToListAsync();

            // SQL queries are also possible
            //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public async Task<UserItem> GetUserAsync(string Login)
        {
            await Init();
            return await Database.Table<UserItem>().Where(i => i.Login == Login).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(UserItem item)
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

        public async Task<int> DeleteItemAsync(UserItem item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
