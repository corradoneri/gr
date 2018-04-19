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
        // Fields
        //
        private object _lockHandle = new object();
        private readonly IRecordSorter _recordSorter;
        private readonly List<Record> _records = new List<Record>();

        /// <summary>
        /// Basic constructor taking a sorter as parameter
        /// </summary>
        /// <param name="recordSorter">Used to sort the data</param>
        public RecordRepository(IRecordSorter recordSorter)
        {
            _recordSorter = recordSorter;
        }

        /// <summary>
        /// Gets records from the "database" and sorts by the given criteria
        /// </summary>
        /// <param name="sortCriteria">Sort conditions</param>
        /// <returns>
        /// The sorted list of records
        /// </returns>
        public IEnumerable<Record> GetRecords(SortCriteria sortCriteria)
        {
            // Copy list so lock isn't held during the sort operation. 
            // Lock wouldn't be necessary if there was an actual database.
            //
            List<Record> copy;
            lock (_lockHandle)
            {
                copy = _records.ToList();
            }
            return _recordSorter.SortRecords(_records.ToList(), sortCriteria);
        }

        /// <summary>
        /// Adds record to the "database"
        /// </summary>
        /// <param name="record">The record to add</param>
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
