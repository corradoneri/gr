using System.Collections.Generic;
using GR.Records.Core.Models;
using GR.Records.Core.Sorter;

namespace GR.Records.Core.DataAccess
{
    public interface IRecordRepository
    {
        IEnumerable<Record> GetRecords(SortCriteria sortCriteria);

        void AddRecord(Record record);
    }
}
