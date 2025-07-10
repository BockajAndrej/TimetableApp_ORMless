using application.App.Pages.ViewModel;
using application.BL.RAW.Facades;
using application.DAL.RAW.Entities;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace application.App.Pages.View.Popups;

public partial class CpEditPopup
{
    MainPageViewModel _vm;
    public Cp? CpMain;

    public List<string> CpStateOptions { get; set; } = new()
    {
        "Vytvorený",
        "Schválený",
        "Vyúctovaný",
        "Zrušený"
    };

    private ObservableCollection<Employee?> _employeeOptions { get; set; }
    public ObservableCollection<Employee?> EmployeeOptions
    {
        get => _employeeOptions;
        set
        {
            if (_employeeOptions != value)
            {
                _employeeOptions = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<City?> _cityOptions { get; set; }

    public ObservableCollection<City?> CityOptions
    {
        get => _cityOptions;
        set
        {
            if (_cityOptions != value)
            {
                _cityOptions = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<Vehicle?> _vehicleOptions { get; set; }

    public ObservableCollection<Vehicle?> VehicleOptions
    {
        get => _vehicleOptions;
        set
        {
            if (_vehicleOptions != value)
            {
                _vehicleOptions = value;
                OnPropertyChanged();
            }
        }
    }


    //Selected item in picker
    private string _cpStateSelected;
    public string CpStateSelected
    {
        get => _cpStateSelected;
        set
        {
            if (_cpStateSelected != value)
            {
                _cpStateSelected = value;
                OnPropertyChanged();
            }
        }
    }


    private Employee? _selectedEmployee;
    public Employee? SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            if (_selectedEmployee != value)
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }
    }

    private City _selectedStartCity;
    public City SelectedStartCity
    {
        get => _selectedStartCity;
        set
        {
            if (_selectedStartCity != value)
            {
                _selectedStartCity = value;
                OnPropertyChanged();
            }
        }
    }

    private City _selectedEndCity;
    public City SelectedEndCity
    {
        get => _selectedEndCity;
        set
        {
            if (_selectedEndCity != value)
            {
                _selectedEndCity = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime _startDate = DateTime.Today;

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (_startDate != value)
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
    }

    private TimeSpan _startTime = DateTime.Now.TimeOfDay;

    public TimeSpan StartTime
    {
        get => _startTime;
        set
        {
            if (_startTime != value)
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime _endDate = DateTime.Today;
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (_endDate != value)
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
    }
    private TimeSpan _endTime = DateTime.Now.TimeOfDay;

    public TimeSpan EndTime
    {
        get => _endTime;
        set
        {
            if (_endTime != value)
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }
    }


    public CpEditPopup(Cp cpMain, MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = this;
        _vm = vm;

        CpMain = cpMain;

        LoadData();
    }

    private async Task LoadData()
    {
        var emps = await _vm.EmployeeFacade.GetAllAsync();
        EmployeeOptions = new ObservableCollection<Employee?>(emps);
        var cities = await _vm.CityFacade.GetAllAsync();
        CityOptions = new ObservableCollection<City>(cities);
        var vehicles = await _vm.VehicleFacade.GetAllAsync();
        VehicleOptions = new ObservableCollection<Vehicle>(vehicles);


        if (CpMain.IdEmployee != string.Empty)
            SelectedEmployee = EmployeeOptions.SingleOrDefault(l => l.Id == CpMain.IdEmployee);
        if (CpMain.IdStartCity != 0)
            SelectedStartCity = CityOptions.SingleOrDefault(l => l.Id == CpMain.IdStartCity);
        if (CpMain.IdEndCity != 0)
            SelectedEndCity = CityOptions.SingleOrDefault(l => l.Id == CpMain.IdEndCity);
        if (!string.IsNullOrEmpty(CpMain.CpState))
            CpStateSelected = CpStateOptions.FirstOrDefault(s => s == CpMain.CpState);
        if (CpMain.StartTime != default)
        {
            StartDate = CpMain.StartTime.Date;
            StartTime = CpMain.StartTime.TimeOfDay;
        }

        if (CpMain.EndTime != default)
        {
            EndDate = CpMain.EndTime.Date;
            EndTime = CpMain.EndTime.TimeOfDay;
        }

        var selectedVehicles = _vm.VehicleFacade.GetByFilterAsync(new List<Employee?>(), new List<City>(),
            new List<Vehicle>(), new List<Cp> { CpMain });

        foreach (var vehicle in selectedVehicles.Result)
        {
            var vehicleToFind = VehicleOptions.FirstOrDefault(v => v.Id == vehicle.Id);
            var index = VehicleOptions.IndexOf(vehicleToFind);
            if (index < 0)
                throw new ArgumentNullException();
            VehicleOptions[index]!.ISelectedFromFilter = true;
        }

    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        CpMain.IdEmployee = SelectedEmployee.Id;
        CpMain.IdStartCity = SelectedStartCity.Id;
        CpMain.IdEndCity = SelectedEndCity.Id;
        CpMain.CreationDate = DateTime.Now;
        CpMain.StartTime = StartDate + StartTime;
        CpMain.EndTime = EndDate + EndTime;
        CpMain.CpState = CpStateSelected;

        var savedCp = _vm.CpFacade.SaveAsync(CpMain);

        var transportsToBeRemoved = _vm.TransportFacade.GetByFilterAsync(new List<Employee?>(), new List<City>(),
            new List<Vehicle>(), new List<Cp> { CpMain });

        foreach (var transport in transportsToBeRemoved.Result)
        {
            await _vm.TransportFacade.DeleteAsync(transport.Id);
        }

        foreach (var vehicle in VehicleOptions)
        {
            if (vehicle is { ISelectedFromFilter: true })
            {
                Transport t = new Transport
                {
                    IdCp = savedCp.Result.Id,
                    IdVehicle = vehicle.Id
                };
                await _vm.TransportFacade.SaveAsync(t);
            }
        }

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