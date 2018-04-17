using System;
using System.Collections.Generic;
using GR.Records.Core.Models;
using GR.Records.Core.Parser;
using GR.Records.Core.Sorter;

namespace GR.Records.ConsoleApp
{
    class Application
    {
        private readonly string[] _args;
        private readonly RecordParser _recordParser;
        private readonly RecordSorter _recordSorter;
        private SortCriteria _sortCriteria;
        private readonly string[] _fileNames = new string[3];

        public Application(string[] args, RecordParser recordParser, RecordSorter recordSorter)
        {
            _args = args;
            _recordParser = recordParser;
            _recordSorter = recordSorter;
        }

        public int Run()
        {
            if (!CheckArgs())
            {
                PrintUsage();
                return -1;
            }
            ProcessFile();
            Console.ReadKey();
            return 0;
        }

        private bool CheckArgs()
        {
            if (_args.Length == 3)
            {
                _sortCriteria = SortCriteria.GenderAscLastNameAsc;
                return true;
            }
            if (_args.Length == 4)
            {
                switch (_args[0])
                {
                    case "-1":
                        _sortCriteria = SortCriteria.GenderAscLastNameAsc;
                        break;
                    case "-2":
                        _sortCriteria = SortCriteria.BirthDateAsc;
                        break;
                    case "-3":
                        _sortCriteria = SortCriteria.LastNameDesc;
                        break;
                }
                for (var i = 1; i < 4; i++)
                    _fileNames[i-1] = _args[i];
                return true;
            }
            return false;
        }

        private void PrintUsage()
        {
            Console.WriteLine($"Usage: {_args[0]} [(-1 | -2 | -3)] <filename1> <filename2> <filename3>");
        }

        private IEnumerable<Record> GetSortedData()
        {
            var recordList = new List<Record>();
            foreach (var fileName in _fileNames)
                recordList.AddRange(_recordParser.ParseRecordFile(fileName));
            return _recordSorter.SortRecords(recordList, _sortCriteria);
        }

        private void ProcessFile()
        {
            foreach (var record in GetSortedData())
                Console.WriteLine($"{record.LastName}, {record.FirstName}, {record.Gender}, {record.FavoriteColor}, {record.BirthDate:M/d/yyy}");
        }
    }
}
