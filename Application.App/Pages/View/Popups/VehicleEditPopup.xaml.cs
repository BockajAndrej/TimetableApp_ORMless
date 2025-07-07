using application.DAL.RAW.Entities;
using CommunityToolkit.Maui.Views;

namespace Application.App.Pages.View.Popups;

public partial class VehicleEditPopup : Popup<int>
{
    public Vehicle? VehicleMain;
    public Vehicle? VehicleTmp;

    public VehicleEditPopup(Vehicle VehicleDetail)
    {
        InitializeComponent();

        VehicleMain = VehicleDetail;

        VehicleTmp = new Vehicle
        {
            Id = VehicleMain.Id,
            VehicleName = VehicleMain.VehicleName
        };

        BindingContext = VehicleTmp;
    }
    async void OnSaveClicked(object? sender, EventArgs e)
    {
        VehicleMain.Id = VehicleTmp.Id;
        VehicleMain.VehicleName = VehicleTmp.VehicleName;

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