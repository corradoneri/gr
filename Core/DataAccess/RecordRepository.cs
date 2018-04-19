using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GR.Records.Core.Models;
using GR.Records.Core.Sorter;

namespace GR.Records.Core.DataAccess
{
    public class RecordRepository : IRecordRepository
    {
        private object _lockHandle = new object();
        private readonly IRecordSorter _recordSorter;
        private readonly List<Record> _records = new List<Record>();

        public RecordRepository(IRecordSorter recordSorter)
        {
            _recordSorter = recordSorter;
        }

        public IEnumerable<Record> GetRecords(SortCriteria sortCriteria)
        {
            // Copy list so lock isn't held during the sort operation. 
            // Wouldn't really be a concern if there was an actual database.
            //
            List<Record> copy;
            lock (_lockHandle)
            {
                copy = _records.ToList();
            }
            return _recordSorter.SortRecords(_records.ToList(), sortCriteria);
        }

        public void AddRecord(Record record)
        {
            // Assuming duplicates are okay
            //
            lock (_lockHandle)
            {
                _records.Add(record);
            }
        }

    }
}
