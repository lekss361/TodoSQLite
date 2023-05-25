using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

[QueryProperty("Item", "Item")]
[QueryProperty("User", "User")]
public partial class TodoItemPage : ContentPage
{
    public UserItem User { get; set; }
    public TodoItem Item { get; set; }
    TodoItemDatabase database;
    
    public TodoItemPage(TodoItemDatabase todoItemDatabase)
    {
        InitializeComponent();
        
        database = todoItemDatabase;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
            return;
        }
         if (!Item.Type.HasValue)
        {
            await DisplayAlert("Type Required", "Please enter a Type for the todo item.", "OK");
            return;
        }
        if (TypeRequest.SelectedIndex==null)
        {
        Item.Status = Statuses.Обрабатывается;

        }
        
        Item.UserId = User.Id;
        await database.SaveItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Item.Id == 0)
            return;
        await database.DeleteItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    async void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        Item = new TodoItem() { Type = (TypeOrders)picker.SelectedIndex, Name= Name.Text,  
            Phone= Convert.ToInt32(Number.Text), Status= (Statuses)TypeRequest.SelectedIndex };
        if (Item.Type.ToString() == picker.Items[2])
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
        Item.Status = (Statuses)picker.SelectedIndex;
    }
}