using CommunityToolkit.Mvvm.ComponentModel;

namespace application.DAL.RAW.Entities;

public partial class Vehicle : ObservableObject, IEntity<int>
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _vehicleName;

    //Not stored in DB
    [ObservableProperty]
    private bool _iSelectedFromFilter = false;
    [ObservableProperty]
    private bool _isClickedFromFilter = false;
    public bool IdIsUsed => Id == 0;
    public bool IdIsUsedInverseValue => !IdIsUsed;
}
