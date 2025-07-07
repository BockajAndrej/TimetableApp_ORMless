using CommunityToolkit.Mvvm.ComponentModel;

namespace application.DAL.RAW.Entities;

public partial class Transport : ObservableObject, IEntity<int>
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _idCp;

    [ObservableProperty]
    private int _idVehicle;
}
