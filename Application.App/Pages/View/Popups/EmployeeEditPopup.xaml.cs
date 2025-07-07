using System.Collections.ObjectModel;
using System.ComponentModel;
using application.DAL.RAW.Entities;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace application.App.Pages.View.Popups;

public partial class EmployeeEditPopup : Popup<int>
{
    public Employee? EmployeeDetailMain;
    public Employee? EmployeeDetailTmp;

    public EmployeeEditPopup(Employee EmployeeDetail)
    {
        InitializeComponent();

        EmployeeDetailMain = EmployeeDetail;

        EmployeeDetailTmp = new Employee
        {
            Id = EmployeeDetailMain.Id,
            FirstName = EmployeeDetailMain.FirstName,
            LastName = EmployeeDetailMain.LastName,
            BirthDay = EmployeeDetailMain.BirthDay,
            BirthNumber = EmployeeDetailMain.BirthNumber,
        };

        BindingContext = EmployeeDetailTmp;
    }
    async void OnSaveClicked(object? sender, EventArgs e)
    {
        EmployeeDetailMain.Id = EmployeeDetailTmp.Id;
        EmployeeDetailMain.FirstName = EmployeeDetailTmp.FirstName;
        EmployeeDetailMain.LastName = EmployeeDetailTmp.LastName;
        EmployeeDetailMain.BirthDay = EmployeeDetailTmp.BirthDay;
        EmployeeDetailMain.BirthNumber = EmployeeDetailTmp.BirthNumber;

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