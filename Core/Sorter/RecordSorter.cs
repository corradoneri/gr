using System.Collections.Generic;
using System.Linq;
using GR.Records.Core.Models;

namespace GR.Records.Core.Sorter
{
    public class RecordSorter : IRecordSorter
    {
        public IEnumerable<Record> SortRecords(IEnumerable<Record> records, SortCriteria sortField)
        {
            switch (sortField)
            {
                case SortCriteria.BirthDateAsc:
                    return records.OrderBy(item => item.BirthDate);
                case SortCriteria.LastNameDesc:
                    return records.OrderByDescending(item => item.LastName);
                case SortCriteria.GenderAscLastNameAsc:
                default:
                    return records.OrderBy(item => item.Gender).ThenBy(item => item.LastName);
            }
        }
    }
}
