using Microsoft.Maui.Controls.Internals;
using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

[QueryProperty("Item", "Item")]
[QueryProperty("User", "User")]
public partial class TodoItemPage : ContentPage
{
    public int TypeRequests { get; set; }
    public int Status { get; set; }
    public string NameCompanyEntry { get; set; }
    public int Role { get; set; }
    private TodoItem _todoItem = new TodoItem();
    private UserItem _userItem = new();

    public UserItem User
    {
        get { return _userItem; }
        set
        {
            _userItem = value;
            Role = (int)User.Role;
            if (Role == (int)Models.Role.Manager)
            {
                this.NameCompanyEntr.IsEnabled = false;
                this.TypeRequesz.IsEnabled = false;
                this.TitelNumber.IsEnabled = false;
                this.DeleteButton.IsEnabled = false;
                this.Number.IsEnabled = false;
            }
            else if (Role == (int)Models.Role.User)
            {
                this.TypeRequest.IsEnabled = false;

            }
        }
    }
    public TodoItem Item
    {
        get { return _todoItem; }
        set
        {
            _todoItem = value;
            if (Item.Type != null)
            {
                TypeRequests = (int)Item.Type;
            Status = (int)Item.Status;
            }
            NameCompanyEntry = Item.Name;
            this.NameCompanyEntr.Text = Item.Name;
            this.Number.Text = _todoItem.Phone.ToString();



        }
    }
    TodoItemDatabase database;

    public TodoItemPage(TodoItemDatabase todoItemDatabase)
    {
        InitializeComponent();
        database = todoItemDatabase;
        BindingContext = this;

    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(this.NameCompanyEntr.Text))
        {
            await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
            return;
        }
        if (TypeRequest.SelectedIndex == null)
        {
            _todoItem.Status = Statuses.Обрабатывается;

        }

        _todoItem.UserId = User.Id;
        _todoItem.Status = (Statuses)this.Status;
        _todoItem.Phone = Convert.ToInt32(this.Number.Text);
        _todoItem.Name = this.NameCompanyEntr.Text;
        _todoItem.Type = (TypeOrders)this.TypeRequesz.SelectedIndex;
        await database.SaveItemAsync(_todoItem);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_todoItem.Id == 0)
            return;
        await database.DeleteItemAsync(_todoItem);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    async void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        _todoItem = new TodoItem()
        {
            Type = (TypeOrders)picker.SelectedIndex,
            Name = NameCompanyEntry,
            Phone = Convert.ToInt32(Number.Text),
            Status = (Statuses)TypeRequest.SelectedIndex
        };
        if (_todoItem.Type.ToString() == picker.Items[2])
        {
            TitelNumber.IsVisible = true;
            Number.IsVisible = true;
        }
        else
        {
            TitelNumber.IsVisible = false;
            Number.IsVisible = false;
        }
    }
    async void StatusPicker(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        _todoItem.Status = (Statuses)picker.SelectedIndex;
    }

    async void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {

        if (!string.IsNullOrWhiteSpace(args.NewTextValue))
        {
            bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x) || x.Equals('.'));

            ((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
        }
    }
}