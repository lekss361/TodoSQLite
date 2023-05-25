using SQLite;

namespace TodoSQLite.Models;

public class TodoItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public TypeOrders? Type { get; set; }
    public int Phone { get; set; }
    public Statuses? Status { get; set; }
    [Indexed]
    public int UserId { get; set; }
}

public class UserItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Unique]
    public string Login { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

}

public enum TypeOrders
{
      Поверка,
      Образец,
      Индивидуальный
}

public enum Statuses
{
    Обрабатывается,
    Выполняется,
    Выполнен
}

public enum Role
{
    User,
    Manager,
    Admin
}
