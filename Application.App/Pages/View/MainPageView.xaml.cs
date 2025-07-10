using application.App.Pages.View.Popups;
using application.App.Pages.ViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using Application.App.Pages.View.Popups;
using application.DAL.RAW.Entities;

namespace application.App.Pages.View;

public partial class MainPageView : ContentPage
{

    private MainPageViewModel _viewModel;
    private PopupOptions popupOptions;
    public MainPageView(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;

        popupOptions = new PopupOptions
        {
            Shape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(2),
                Opacity = 0.8f,
                Stroke = Colors.Black,
                BackgroundColor = Colors.Black,
                StrokeThickness = 2
            },
            CanBeDismissedByTappingOutsideOfPopup = false
        };
    }

    private async void EditEmployeeButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.IsClickedEmployee == null)
        {
            _viewModel.IsClickedEmployee = new Employee
            {
                Id = "",
                FirstName = "",
                LastName = "",
                BirthDay = default,
                BirthNumber = "",
            };
        }

        var popup = new EmployeeEditPopup(_viewModel.IsClickedEmployee);

        IPopupResult<int> popupResult = await this.ShowPopupAsync<int>(popup, popupOptions);

        if (popupResult.WasDismissedByTappingOutsideOfPopup)
            return;

        //Yes was clicked
        if (popupResult.Result == 1)
        {
            await _viewModel.SaveEmployeeAsync(_viewModel.IsClickedEmployee);
            await _viewModel.LoadData();
        }
        else if (popupResult.Result == 2)
        {
            await _viewModel.RoveEmployeeAsync(_viewModel.IsClickedEmployee);
            await _viewModel.LoadData();
        }

        _viewModel.IsClickedEmployee = null;
    }

    private async void EditCityButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.IsClickedCity == null)
        {
            _viewModel.IsClickedCity = new City
            {
                Id = 0,
                CityName = "",
                StateName = "",
                Latitude = 0,
                Longitude = 0
            };
        }
        var popup = new CityEditPopup(_viewModel.IsClickedCity);

        IPopupResult<int> popupResult = await this.ShowPopupAsync<int>(popup, popupOptions);

        if (popupResult.WasDismissedByTappingOutsideOfPopup)
            return;

        //Yes was clicked
        if (popupResult.Result == 1)
        {
            await _viewModel.SaveCityAsync(_viewModel.IsClickedCity);
            await _viewModel.LoadData();
        }
        else if (popupResult.Result == 2)
        {
            await _viewModel.RoveCityAsync(_viewModel.IsClickedCity);
            await _viewModel.LoadData();
        }

        _viewModel.IsClickedCity = null;
    }

    private async void EditVehicleButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.IsClickedVehicle == null)
        {
            _viewModel.IsClickedVehicle = new Vehicle
            {
                Id = 0,
                VehicleName = ""
            };
        }
        var popup = new VehicleEditPopup(_viewModel.IsClickedVehicle);

        IPopupResult<int> popupResult = await this.ShowPopupAsync<int>(popup, popupOptions);

        if (popupResult.WasDismissedByTappingOutsideOfPopup)
            return;

        //Yes was clicked
        if (popupResult.Result == 1)
        {
            await _viewModel.SaveVehicleAsync(_viewModel.IsClickedVehicle);
            await _viewModel.LoadData();
        }
        else if (popupResult.Result == 2)
        {
            await _viewModel.RoveVehicleAsync(_viewModel.IsClickedVehicle);
            await _viewModel.LoadData();
        }

        _viewModel.IsClickedVehicle = null;
    }

    private async void EditCpButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.IsClickedCp == null)
        {
            _viewModel.IsClickedCp = new Cp
            {
                Id = 0,
                IdEmployee = string.Empty,
                IdStartCity = 0,
                IdEndCity = 0,
                CreationDate = default,
                StartTime = default,
                EndTime = default,
                CpState = string.Empty
            };
        }
        var popup = new CpEditPopup(_viewModel.IsClickedCp, _viewModel);

        IPopupResult<int> popupResult = await this.ShowPopupAsync<int>(popup, popupOptions);

        if (popupResult.WasDismissedByTappingOutsideOfPopup)
            return;

        //Yes was clicked
        if (popupResult.Result == 1)
        {
            await _viewModel.LoadData();
        }
        else if (popupResult.Result == 2)
        {
            await _viewModel.RemoveCpAsync(_viewModel.IsClickedCp);
            await _viewModel.LoadData();
        }

        _viewModel.IsClickedCp = null;
    }


    private void OnExpanderIsExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        //Expanded
        if (e.IsExpanded)
        {
            _viewModel.GetByFilterAsync();
        }
        //Collapsed
        else
        {
        }
    }
}