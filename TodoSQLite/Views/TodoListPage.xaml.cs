using System;
using System.Collections.ObjectModel;
using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

public partial class TodoListPage : ContentPage
{
    public ObservableCollection<TodoItem> Items { get; set; } = new();
    UserItem userItem = new UserItem();
    IDbRepository dbRepository;
    public TodoListPage( IDbRepository dbRepository)
    {
        InitializeComponent();
        BindingContext = this;
        this.dbRepository = dbRepository;
    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        List<TodoItem> items = new List<TodoItem>();
        if (userItem.Role == Role.User)
            items = dbRepository.Get<TodoItem>(x => x.UserId == userItem.Id).ToList();
        else
            items = dbRepository.GetAll<TodoItem>().ToList();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);

        });
    }
    async void OnItemAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TodoItemPage), true, new Dictionary<string, object>
        {
            ["Item"] = new TodoItem(),
            ["User"] = userItem

        });
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not TodoItem item)
            return;

        await Shell.Current.GoToAsync(nameof(TodoItemPage), true, new Dictionary<string, object>
        {
            ["Item"] = item,
            ["User"] = userItem
        });


    }
    async void AddUser(object sender, EventArgs e)
    {
        UserItem item = new UserItem() { Login = TextLogin.Text, Password = TextPassword.Text, Role = Role.User };
        int x = dbRepository.Add(item);
        dbRepository.SaveChangesAsync();
    }
    async void CheckUser(object sender, EventArgs e)
    {
        UserItem item = new UserItem() { Login = TextLogin.Text, Password = TextPassword.Text };
        userItem = dbRepository.Get<UserItem>(x => x.Login == TextLogin.Text).FirstOrDefault();

        if (userItem == null || userItem.Password != TextPassword.Text)
        {
            await DisplayAlert("Incorect login data", "Please enter a login or password.", "OK");
            return;
        }
        else
        {
            List<TodoItem> items = new List<TodoItem>();
            if (userItem.Role == Role.User)
                items = dbRepository.Get<TodoItem>(x => x.UserId == userItem.Id).ToList();
            else
                items = dbRepository.GetAll<TodoItem>().ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);

            });
            AddRequest.IsEnabled = true;
            DateTable.IsVisible = true;
            DateTable.IsEnabled = true;
            ButtonLogin.IsVisible = false;
            ButtonRegistation.IsVisible = false;
            TextRole.Text = userItem.Role.ToString();
        }
    }
    async void OffVisible(object sender, EventArgs e)
    {
        AddRequest.IsEnabled = false;
        DateTable.IsVisible = false;
        DateTable.IsEnabled = false;
        ButtonLogin.IsVisible = true;
        ButtonRegistation.IsVisible = true;
        userItem = new UserItem();
    }
}
