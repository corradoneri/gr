using System;
using System.Collections.Generic;
using System.Linq;
using GR.Records.Core.DataAccess;
using GR.Records.Core.Parser;
using GR.Records.Core.Sorter;
using GR.Records.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GR.Records.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("records")]
    public class RecordsController : Controller
    {
        private IRecordParser _recordParser;
        private IRecordRepository _recordRepository;

        public RecordsController(IRecordParser recordParser, IRecordRepository recordRepository)
        {
            _recordParser = recordParser;
            _recordRepository = recordRepository;
        }

        // Note - would use async/await pattern if there was a database
        //
        [HttpGet("gender")]
        public IEnumerable<RecordViewModel> GetGender()
        {
            return _recordRepository.GetRecords(SortCriteria.GenderAscLastNameAsc).Select(Record => new RecordViewModel(Record));
        }

        [HttpGet("birthdate")]
        public IEnumerable<RecordViewModel> GetBirthDate()
        {
            return _recordRepository.GetRecords(SortCriteria.BirthDateAsc).Select(Record => new RecordViewModel(Record));
        }

        [HttpGet("name")]
        public IEnumerable<RecordViewModel> GetName()
        {
            return _recordRepository.GetRecords(SortCriteria.LastNameDesc).Select(Record => new RecordViewModel(Record));
        }

        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            try
            {
                var record = _recordParser.ParseRecord(value);
                _recordRepository.AddRecord(record);
                return Created("GetSortedByGenderAscLastNameAsc", new RecordViewModel(record));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
