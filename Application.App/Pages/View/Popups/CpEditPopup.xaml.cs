using CommunityToolkit.Maui.Views;

namespace application.App.Pages.View.Popups;

public partial class CpEditPopup : Popup<int>
{
    public CpEditPopup()
    {
        InitializeComponent();
    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        //EmployeeDetailMain.Id = EmployeeDetailTmp.Id;
        //EmployeeDetailMain.FirstName = EmployeeDetailTmp.FirstName;
        //EmployeeDetailMain.LastName = EmployeeDetailTmp.LastName;
        //EmployeeDetailMain.BirthDay = EmployeeDetailTmp.BirthDay;
        //EmployeeDetailMain.BirthNumber = EmployeeDetailTmp.BirthNumber;

        await CloseAsync(1);
    }

    async void OnCloseTapped(object? sender, EventArgs e)
    {
        await CloseAsync(0);
    }

    async void OnDeleteTapped(object? sender, EventArgs e)
    {
        await CloseAsync(2);
    }
}