using System;
using System.Collections.Generic;
using System.Text;
using GR.Records.Core.Exceptions;
using GR.Records.Core.Models;
using GR.Records.Core.Parser;
using GR.Records.Core.Sorter;

namespace GR.Records.ConsoleApp
{
    /// <summary>
    /// Contains all of the high level application logic delegating the actual data intensive work
    /// to various services that are injected into the constructor.
    /// </summary>
    class Application
    {
        // Constants
        //
        private const int FileCount = 3;

        // Fields
        //
        private readonly string[] _args;
        private readonly RecordParser _recordParser;
        private readonly RecordSorter _recordSorter;
        private SortCriteria _sortCriteria = SortCriteria.GenderAscLastNameAsc;
        private readonly string[] _fileNames = new string[FileCount];

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="args">Arguments to the program containing three filenames and optional sort criteria</param>
        /// <param name="recordParser">Service that is used to parse data in files</param>
        /// <param name="recordSorter">Service that is used to sort data</param>
        public Application(string[] args, RecordParser recordParser, RecordSorter recordSorter)
        {
            _args = args;
            _recordParser = recordParser;
            _recordSorter = recordSorter;
        }

        /// <summary>
        /// Checks command line arguments and then executes the heart of the program, which
        /// gets the data from the files, sorts it and outputs it.
        /// </summary>
        /// <returns>
        /// If application runs properly returns true. Otherwise, returns false.
        /// </returns>
        public int Run()
        {
            try
            {
                if (!CheckArgs())
                {
                    PrintUsage();
                    return -1;
                }
                ProcessFiles();
                return 0;
            }
            catch (RecordFileException fileRecorddException)
            {
                Console.WriteLine();
                Console.WriteLine(fileRecorddException.FullMessage);
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine($"An unknown error occured: {exception.Message}");
            }
            return -1;
        }

        /// <summary>
        /// Checks to make sure there are either 3 or 4 arguments to the command line.
        /// The first three arguments are the file names in any order while the optional 
        /// fourth argument determines the sort criteria.
        /// </summary>
        /// <returns>
        /// If number of arguments is three or four and the optional fourth argument
        /// contains one of the expected values, returns true. Otherwise, returns false.
        /// </returns>
        private bool CheckArgs()
        {
            if (_args.Length < FileCount || _args.Length > FileCount + 1)
                return false;

            for (var i = 0; i < FileCount; i++)
                _fileNames[i] = _args[i];
            _sortCriteria = SortCriteria.GenderAscLastNameAsc;

            if (_args.Length == FileCount + 1)
            {
                switch (_args[_args.Length - 1])
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
                    default:
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Prints the usage information.
        /// </summary>
        private void PrintUsage()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Usage: dotnet {AppDomain.CurrentDomain.FriendlyName}.dll <filename1> <filename2> <filename3> [(-1 | -2 | -3)]");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Description:");
            stringBuilder.AppendLine("Extracts data from the three given files each using one of the three delimiter and outputs results sorted by the optional sort criteria");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Notes:");
            stringBuilder.AppendLine("Files can be given in any order but must be the first three parameters");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Options:");
            stringBuilder.AppendLine("-1\t(Default) Sort by gender (ascending) and then by last name (ascending)");
            stringBuilder.AppendLine("-2\tSort by birth date (ascending)");
            stringBuilder.AppendLine("-3\tSort by last name (descending)");
            stringBuilder.AppendLine();

            Console.WriteLine(stringBuilder.ToString());
        }

        /// <summary>
        /// Loops through all data from files, sorts them and prints them to the console.
        /// </summary>
        private void ProcessFiles()
        {
            var records = _recordParser.ParseFiles(_fileNames);
            foreach (var record in _recordSorter.SortRecords(records, _sortCriteria))
                Console.WriteLine($"{record.LastName}, {record.FirstName}, {record.Gender}, {record.FavoriteColor}, {record.BirthDate:M/d/yyy}");
        }
    }
}
