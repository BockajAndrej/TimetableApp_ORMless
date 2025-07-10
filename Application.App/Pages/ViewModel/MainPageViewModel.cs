using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using application.BL.RAW.Facades;
using application.DAL.RAW.Entities;

namespace application.App.Pages.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        //Facades
        public readonly EmployeeFacade EmployeeFacade;
        public readonly CpFacade CpFacade;
        public readonly CityFacade CityFacade;
        public readonly VehicleFacade VehicleFacade;

        //Collections
        private ObservableCollection<Employee?> _Employees;
        public ObservableCollection<Employee?> Employees
        {
            get => _Employees;
            set
            {
                if (_Employees != null)
                {
                    _Employees.CollectionChanged -= Employees_CollectionChanged;
                    foreach (var employee in _Employees)
                        employee.PropertyChanged -= Employee_PropertyChanged;
                }

                SetProperty(ref _Employees, value);
                if (_Employees != null)
                {
                    _Employees.CollectionChanged += Employees_CollectionChanged;
                    foreach (var employee in _Employees)
                        employee.PropertyChanged += Employee_PropertyChanged;
                }
            }
        }
        [ObservableProperty]
        public ObservableCollection<Cp> _Cps;

        public ObservableCollection<City> _Cities;
        public ObservableCollection<City> Cities
        {
            get => _Cities;
            set
            {
                if (_Cities != null)
                {
                    _Cities.CollectionChanged -= Cities_CollectionChanged;
                    foreach (var item in _Cities)
                        item.PropertyChanged -= City_PropertyChanged;
                }

                SetProperty(ref _Cities, value);
                if (_Cities != null)
                {
                    _Cities.CollectionChanged += Cities_CollectionChanged;
                    foreach (var item in _Cities)
                        item.PropertyChanged += City_PropertyChanged;
                }
            }
        }
        public ObservableCollection<Vehicle> _Vehicles;
        public ObservableCollection<Vehicle> Vehicles
        {
            get => _Vehicles;
            set
            {
                if (_Vehicles != null)
                {
                    _Vehicles.CollectionChanged -= Vehicles_CollectionChanged;
                    foreach (var item in _Vehicles)
                        item.PropertyChanged -= Vehicle_PropertyChanged;
                }

                SetProperty(ref _Vehicles, value);
                if (_Vehicles != null)
                {
                    _Vehicles.CollectionChanged += Vehicles_CollectionChanged;
                    foreach (var item in _Vehicles)
                        item.PropertyChanged += Vehicle_PropertyChanged;
                }
            }
        }


        //SearchBars
        [ObservableProperty]
        private string _searchbarEmployee = string.Empty;
        [ObservableProperty]
        private string _searchbarCp = string.Empty;
        [ObservableProperty]
        private string _searchbarVehicle = string.Empty;
        [ObservableProperty]
        private string _searchbarCity = string.Empty;

        //List of selected Items
        public List<Employee?> SelectedEmployees = new List<Employee?>();
        public List<City> SelectedCities = new List<City>();
        public List<Vehicle> SelectedVehicles = new List<Vehicle>();

        //Show pop up variables
        public Employee? IsClickedEmployee = null;
        public City? IsClickedCity = null;
        public Vehicle? IsClickedVehicle = null;
        public Cp? IsClickedCp = null;

        //To expand
        public Cp? IsClickedCpToExpand = null;

        public MainPageViewModel(EmployeeFacade empfacade, CpFacade cpf, CityFacade cityf, VehicleFacade vehf)
        {
            EmployeeFacade = empfacade;
            CityFacade = cityf;
            CpFacade = cpf;
            VehicleFacade = vehf;

            LoadData();
        }

        public async Task LoadData()
        {
            var emps = await EmployeeFacade.GetAllAsync();
            Employees = new ObservableCollection<Employee?>(emps);
            var cps = await CpFacade.GetAllAsync();
            Cps = new ObservableCollection<Cp>(cps);
            var cities = await CityFacade.GetAllAsync();
            Cities = new ObservableCollection<City>(cities);
            var vehicles = await VehicleFacade.GetAllAsync();
            Vehicles = new ObservableCollection<Vehicle>(vehicles);
        }
        private async Task LoadDataCpQuery()
        {
            Cps.Clear();
            if (SearchbarCp == string.Empty)
            {
                var result = await CpFacade.GetByFilterAsync(SelectedEmployees, SelectedCities, SelectedVehicles, new List<Cp>());
                Cps = new ObservableCollection<Cp>(result);
            }
            else
            {
                var result = await CpFacade.GetByNameAsync(SearchbarCp);
                Cps = new ObservableCollection<Cp>(result);
            }
        }

        async partial void OnSearchbarEmployeeChanged(string? value)
        {
            Employees.Clear();

            string lowerSearchTerm = SearchbarEmployee.Trim().ToLower();
            if (SearchbarEmployee != string.Empty)
            {
                var result = await EmployeeFacade.GetByNameAsync(SearchbarEmployee);
                Employees = new ObservableCollection<Employee?>(result);
            }
            else
            {
                var result = await EmployeeFacade.GetAllAsync();
                Employees = new ObservableCollection<Employee?>(result);
            }
        }
        async partial void OnSearchbarCityChanged(string? value)
        {
            Cities.Clear();

            if (SearchbarCity != string.Empty)
            {
                var result = await CityFacade.GetByNameAsync(SearchbarCity.Trim());
                Cities = new ObservableCollection<City>(result);
            }
            else
            {
                var result = await CityFacade.GetAllAsync();
                Cities = new ObservableCollection<City>(result);
            }
        }
        async partial void OnSearchbarVehicleChanged(string? value)
        {
            Vehicles.Clear();
            string lowerSearchTerm = SearchbarVehicle.Trim().ToLower();
            Expression<Func<Vehicle, bool>> predicate = lamb => lamb.VehicleName.ToLower().Contains(lowerSearchTerm);

            if (SearchbarEmployee != string.Empty)
            {
                var result = await VehicleFacade.GetAllAsync();
                Vehicles = new ObservableCollection<Vehicle>(result);
            }
            else
            {
                var result = await VehicleFacade.GetAllAsync();
                Vehicles = new ObservableCollection<Vehicle>(result);
            }
        }
        async partial void OnSearchbarCpChanged(string? value)
        {
            await LoadDataCpQuery();
        }

        //MainPageView aux methods
        public async Task<bool> SaveEmployeeAsync(Employee model)
        {
            var result = await EmployeeFacade.SaveAsync(model);
            if (result.Id == String.Empty)
                return false;
            return true;
        }
        public async Task<bool> SaveCityAsync(City model)
        {
            var result = await CityFacade.SaveAsync(model);
            if (result == null)
                return false;
            return true;
        }
        public async Task<bool> SaveVehicleAsync(Vehicle model)
        {
            var result = await VehicleFacade.SaveAsync(model);
            if (result == null)
                return false;
            return true;
        }
        public async Task<bool> SaveCpAsync(Cp model)
        {
            var result = await CpFacade.SaveAsync(model);
            if (result == null)
                return false;
            return true;
        }

        public async Task RoveEmployeeAsync(Employee model)
        {
            await EmployeeFacade.DeleteAsync(model.Id);
        }
        public async Task RoveCityAsync(City model)
        {
            await CityFacade.DeleteAsync(model.Id);
        }
        public async Task RoveVehicleAsync(Vehicle model)
        {
            await VehicleFacade.DeleteAsync(model.Id);
        }
        public async Task RemoveCpAsync(Cp model)
        {
            await CpFacade.DeleteAsync(model.Id);
        }

        public async Task GetByFilterAsync()
        {
            var selectedCp = new List<Cp>();
            selectedCp.Add(IsClickedCpToExpand);
            var result = await VehicleFacade.GetByFilterAsync(new List<Employee?>(), new List<City>(), new List<Vehicle>(), selectedCp);
            foreach (var cp in Cps)
            {
                if (cp == IsClickedCpToExpand)
                {
                    cp.Vehicles.Clear();
                    foreach (var vehicle in result)
                    {
                        cp.Vehicles.Add(vehicle);
                    }
                }
            }
        }

        //Collection changed
        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Added Items
            if (e.NewItems != null)
            {
                foreach (Employee newEmployee in e.NewItems)
                    newEmployee.PropertyChanged += Employee_PropertyChanged;
            }
            //Removed Items
            if (e.OldItems != null)
            {
                foreach (Employee oldEmployee in e.OldItems)
                    oldEmployee.PropertyChanged -= Employee_PropertyChanged;
            }
        }
        private void Cities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Added Items
            if (e.NewItems != null)
            {
                foreach (City newEmployee in e.NewItems)
                    newEmployee.PropertyChanged += Employee_PropertyChanged;
            }
            //Removed Items
            if (e.OldItems != null)
            {
                foreach (City oldEmployee in e.OldItems)
                    oldEmployee.PropertyChanged -= Employee_PropertyChanged;
            }
        }
        private void Vehicles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Added Items
            if (e.NewItems != null)
            {
                foreach (Vehicle newEmployee in e.NewItems)
                    newEmployee.PropertyChanged += Employee_PropertyChanged;
            }
            //Removed Items
            if (e.OldItems != null)
            {
                foreach (Vehicle oldEmployee in e.OldItems)
                    oldEmployee.PropertyChanged -= Employee_PropertyChanged;
            }
        }

        //Property changed
        private void Employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Employee.ISelectedFromFilter))
            {
                SelectedEmployees.Clear();
                foreach (var employee in Employees)
                {
                    if (employee.ISelectedFromFilter)
                        SelectedEmployees.Add(employee);
                }
                LoadDataCpQuery();
            }
        }
        private void City_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(City.ISelectedFromFilter))
            {
                SelectedCities.Clear();
                foreach (var city in Cities)
                {
                    if (city.ISelectedFromFilter)
                        SelectedCities.Add(city);
                }
                LoadDataCpQuery();
            }
        }
        private void Vehicle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vehicle.ISelectedFromFilter))
            {
                SelectedVehicles.Clear();
                foreach (var vehicle in Vehicles)
                {
                    if (vehicle.ISelectedFromFilter)
                        SelectedVehicles.Add(vehicle);
                }
                LoadDataCpQuery();
            }
        }


        //Popup
        [RelayCommand]
        public void BtnClickedEmployeeFromFilter(Employee item)
        {
            IsClickedEmployee = item;
        }
        [RelayCommand]
        public void BtnClickedCityFromFilter(City item)
        {
            IsClickedCity = item;
        }
        [RelayCommand]
        public void BtnClickedVehicleFromFilter(Vehicle item)
        {
            IsClickedVehicle = item;
        }

        [RelayCommand]
        public void BtnClickedCpFromExpander(Cp item)
        {
            IsClickedCpToExpand = item;
        }
        [RelayCommand]
        public void BtnClickedCpToShowDetail(Cp item)
        {
            IsClickedCp = item;
        }
    }
}