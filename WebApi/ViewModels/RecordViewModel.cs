using GR.Records.Core.Models;

namespace GR.Records.WebApi.ViewModels
{
    /// <summary>
    /// Used to "shape" the data for the WebApi
    /// </summary>
    public class RecordViewModel
    {
        public string LastName { get; }
        public string FirstName { get; }
        public string Gender { get; }
        public string FavoriteColor { get; }
        public string BirthDate { get; }

        public RecordViewModel(Record record)
        {
            LastName = record.LastName;
            FirstName = record.FirstName;
            Gender = record.Gender.ToString();
            FavoriteColor = record.FavoriteColor;
            BirthDate = record.BirthDate?.ToString("M/d/yyyy") ?? "";
        }
    }
}
