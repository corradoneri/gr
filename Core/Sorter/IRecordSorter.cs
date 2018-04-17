using System.Collections.Generic;
using GR.Records.Core.Models;

namespace GR.Records.Core.Sorter
{
    public interface IRecordSorter
    {
        IEnumerable<Record> SortRecords(IEnumerable<Record> records, SortCriteria sortField);
    }
}
