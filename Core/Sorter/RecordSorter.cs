using System.Collections.Generic;
using System.Linq;
using GR.Records.Core.Models;

namespace GR.Records.Core.Sorter
{
    /// <summary>
    /// Class that contains the sorting logic. Uses an extremely simplistic approach. Can probably
    /// be improved. Could become unnecessary if database is ued.
    /// </summary>
    public class RecordSorter : IRecordSorter
    {
        /// <summary>
        /// Sorts the dataa records by the given sort conditions.
        /// </summary>
        /// <param name="records">The records to sort.</param>
        /// <param name="sortCriteria">Criteria to sort by</param>
        /// <returns>
        /// Returns enumerable of the sorted data.
        /// </returns>
        public IEnumerable<Record> SortRecords(IEnumerable<Record> records, SortCriteria sortCriteria)
        {
            switch (sortCriteria)
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
