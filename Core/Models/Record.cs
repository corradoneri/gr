using System;

namespace GR.Records.Core.Models
{
    public class Record
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Gender Gender { get; set; }
        public string FavoriateColor { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
