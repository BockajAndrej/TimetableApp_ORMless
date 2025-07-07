using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using application.DAL.RAW.Entities;

namespace application.App.Pages.View.Popups;

public partial class CityEditPopup : Popup<int>
{
    public City? modelMain;
    public City? modelTmp;
    public CityEditPopup(City model)
    {
        InitializeComponent();

        modelMain = model;

        modelTmp = new City
        {
            Id = model.Id,
            CityName = model.CityName,
            StateName = model.StateName,
            Latitude = model.Latitude,
            Longitude = model.Longitude
        };

        BindingContext = modelTmp;
    }

    public async void OnSaveClicked(object? sender, EventArgs e)
    {
        modelMain.Id = modelTmp.Id;
        modelMain.CityName = modelTmp.CityName;
        modelMain.StateName = modelTmp.StateName;
        modelMain.Latitude = modelTmp.Latitude;
        modelMain.Longitude = modelTmp.Longitude;

        await CloseAsync(1);
    }
    public async void OnCloseTapped(object? sender, EventArgs e)
    {
        await CloseAsync(0);
    }
    public async void OnDeleteTapped(object? sender, EventArgs e)
    {
        //You are not allowed to remove item with changed Id
        await CloseAsync(2);
    }
}