
namespace ConsoleApp1;

public class Base : IBase
{
    public int Id { get; set; }
}
public class TodoItem :Base
{
    public string Name { get; set; }
    public string? Notes { get; set; }
    public TypeOrders? Type { get; set; }
    public int Phone { get; set; }
    public Statuses? Status { get; set; }
    public UserItem User { get; set; }
}

public class UserItem: Base
{
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
