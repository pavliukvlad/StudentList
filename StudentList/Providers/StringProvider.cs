using Android.Content;
using StudentList.Providers.Interfaces;

namespace StudentList.Providers
{
    public class StringProvider : IStringProvider
    {
        private readonly Context context;

        public StringProvider(Context context)
        {
            this.context = context;
        }

        public string NameError => this.context.GetString(Resource.String.name_field_error);

        public string BirthdateError => this.context.GetString(Resource.String.birthdate_field_error);

        public string UniversityError => this.context.GetString(Resource.String.uni_field_error);

        public string GroupError => this.context.GetString(Resource.String.group_field_error);

        public string PhoneError => this.context.GetString(Resource.String.phone_field_error);

        public string GroupFilter => this.context.GetString(Resource.String.filter_group_txt);
    }
}
