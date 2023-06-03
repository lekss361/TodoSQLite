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
    private int UserId;
    public int id;
    private bool Init { get; set; }
    private TodoItem _todoItem = new TodoItem();
    private UserItem _userItem = new();
    private IDbRepository dbRepository;

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
            id=_todoItem.Id;
            UserId=_todoItem.UserId;
            if (Item.Type != null)
            {
                TypeRequests = (int)Item.Type;
                Status = (int)Item.Status;
            }
            if (_todoItem.Type!=null)
            {
            TypeRequests= (int)_todoItem.Type;

            }
            NameCompanyEntry = Item.Name;
            this.NameCompanyEntr.Text = Item.Name;
            this.Number.Text = _todoItem.Phone.ToString();
            Init = false;



        }
    }

    public TodoItemPage(IDbRepository dbRepository)
    {
        InitializeComponent();
        BindingContext = this;
        this.dbRepository = dbRepository;
        Init =true;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(this.NameCompanyEntr.Text))
        {
            await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
            return;
        }
        if (TypeRequest.SelectedIndex == -1)
        {
            _todoItem.Status = Statuses.Обрабатывается;

        }

        if (User.Role==0)
        {
            _todoItem.UserId = User.Id;

        }
        else
        {
            _todoItem.UserId = UserId;
        }
        _todoItem.Id = id;
        _todoItem.Status = (Statuses)this.Status;
        _todoItem.Phone = Convert.ToInt32(this.Number.Text);
        _todoItem.Name = this.NameCompanyEntr.Text;
        _todoItem.Type = (TypeOrders)this.TypeRequesz.SelectedIndex;
        if (Init)
        {
        dbRepository.Add<TodoItem>(_todoItem);

        }
        else
        {
            dbRepository.Update<TodoItem>(_todoItem);

        }
        dbRepository.SaveChangesAsync();

        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_todoItem.Id == 0)
            return;
        dbRepository.Remove(_todoItem);
        dbRepository.SaveChangesAsync();
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