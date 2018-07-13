namespace StudentList.Providers.Interfaces
{
    public interface IStringProvider
    {
        // Error messages
        string NameError { get; }

        string BirthdateError { get; }

        string UniversityError { get; }

        string GroupError { get; }

        string PhoneError { get; }

        // Filters
        string GroupFilter { get; }
    }
}
