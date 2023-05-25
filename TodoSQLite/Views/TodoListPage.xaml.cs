using System;
using System.Collections.ObjectModel;
using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

public partial class TodoListPage : ContentPage
{
    TodoItemDatabase database;
    UserItemDataBase userItemDataBase;
    public ObservableCollection<TodoItem> Items { get; set; } = new();
    UserItem userItem = new UserItem();
    public TodoListPage(TodoItemDatabase todoItemDatabase, UserItemDataBase userItemDataBase)
    {
        InitializeComponent();
        database = todoItemDatabase;
        BindingContext = this;
        this.userItemDataBase = userItemDataBase;
    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var items = await database.GetItemsAsync(userItem.Id);
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
            ["User"]=userItem
        });


    }
    async void AddUser(object sender, EventArgs e)
    {
        UserItem item = new UserItem() { Login = TextLogin.Text, Password = TextPassword.Text, Role = 0 };
        int x = await userItemDataBase.SaveItemAsync(item);
    }
    async void CheckUser(object sender, EventArgs e)
    {
        UserItem item = new UserItem() { Login = TextLogin.Text, Password = TextPassword.Text };
        userItem = await userItemDataBase.GetUserAsync(TextLogin.Text);

        if (userItem == null || userItem.Password != TextPassword.Text)
        {
            await DisplayAlert("Incorect login data", "Please enter a login or password.", "OK");
            return;
        }
        else
        {
            var items = await database.GetItemsAsync(userItem.Id);
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
