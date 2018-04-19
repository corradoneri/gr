using System;

namespace GR.Records.Core.Models
{
    public class Record
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Gender Gender { get; set; }
        public string FavoriteColor { get; set; }
        public DateTime? BirthDate { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(LastName) && !string.IsNullOrWhiteSpace(FirstName);
    }
}
