using CommunityToolkit.Mvvm.ComponentModel;

namespace application.DAL.RAW.Entities;

public partial class Employee : ObservableObject, IEntity<string>
{
    //Stored in DB
    [ObservableProperty] private string _id;
    [ObservableProperty] private string _firstName;
    [ObservableProperty] private string _lastName;
    [ObservableProperty] private string _birthNumber;
    [ObservableProperty] private DateOnly _birthDay;

    //Not stored in DB
    [ObservableProperty]
    private bool _iSelectedFromFilter = false;
    [ObservableProperty]
    private bool _isClickedFromFilter = false;
    public bool IdIsUsed => Id == string.Empty;
    public bool IdIsUsedInverseValue => !IdIsUsed;

    //Specific properties
    public string FullName => $"{FirstName} {LastName}";
    public DateTime BirthDayDateTime
    {
        get { return BirthDay.ToDateTime(TimeOnly.MinValue); }
        set
        {
            if (BirthDay != DateOnly.FromDateTime(value))
            {
                BirthDay = DateOnly.FromDateTime(value);
                OnPropertyChanged(nameof(BirthDayDateTime));
            }
        }
    }
}
