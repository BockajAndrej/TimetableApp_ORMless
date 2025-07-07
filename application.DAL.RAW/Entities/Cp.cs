using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace application.DAL.RAW.Entities;

public partial class Cp : ObservableObject, IEntity<int>
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _idEmployee;

    [ObservableProperty]
    private int _idStartCity;

    [ObservableProperty]
    private int _idEndCity;

    [ObservableProperty]
    private DateTime _creationDate;

    [ObservableProperty]
    private DateTimeOffset _startTime;

    [ObservableProperty]
    private DateTimeOffset _endTime;

    [ObservableProperty]
    private string _cpState;

    //Not stored in DB
    [ObservableProperty]
    private bool _iSelectedFromFilter = false;
    [ObservableProperty]
    private bool _isClickedFromFilter = false;
    public bool IdIsUsed => Id == 0;
    public bool IdIsUsedInverseValue => !IdIsUsed;

    [ObservableProperty] private string _employeeName;
    [ObservableProperty] private string _startCityName;
    [ObservableProperty] private string _endCityName;
}
