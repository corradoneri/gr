using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GR.Records.Core.Models;
using GR.Records.Core.Exceptions;

namespace GR.Records.Core.Parser
{
    /// <summary>
    /// Contains the file parsing logic.
    /// </summary>
    public class RecordParser : IRecordParser
    {
        // Constants
        //
        private const int LastNameIndex = 0;
        private const int FirstNameIndex = 1;
        private const int GenderIndex = 2;
        private const int FavoriteColorIndex = 3;
        private const int BirthDateIndex = 4;
        private const int FieldCount = 5;

        // Fields
        //
        private char[] Delimiters = { ',', '|', ' ' };

        /// <summary>
        /// Attempts to find the delimiter and then parses the one record.
        /// </summary>
        /// <param name="data">The record to parse.</param>
        /// <returns>
        /// If all is good will return the parsed record. Otherwise, returns null.
        /// </returns>
        /// <exception>
        /// Throws exception if delimiter could not be founde.
        /// </exception>
        public Record ParseRecord(string data)
        {
            var delimiter = ExtractDelimiter(data);
            if (!delimiter.HasValue)
                throw new RecordException("Could not find delimiter in record");
            return ParseRecord(data, delimiter.Value);
        }

        /// <summary>
        /// Parses the entire file.
        /// </summary>
        /// <param name="fileName">The name of the file to parse.</param>
        /// <returns>
        /// Will return enumerable containing the data.
        /// </returns>
        /// <exception>
        /// Throws RecordFileError if file is invalid.
        /// </exception>
        public IEnumerable<Record> ParseRecordFile(string fileName)
        {
            var delimiter = ExtractDelimiterFromFile(fileName);
            if (!delimiter.HasValue)
                throw new RecordFileException(fileName, "Could not find delimiter in record file");

            var records = new List<Record>();
            var recordFileError = new RecordFileException(fileName);
            using (var streamReader = new StreamReader(fileName))
            {
                var line = streamReader.ReadLine();
                var lineNo = 1;
                while (line != null)
                {
                    var record = ParseRecord(line, delimiter.Value);
                    if (record == null || !record.IsValid)
                        recordFileError.AddError($"Line {lineNo}: Invalid record");
                    else
                        records.Add(record);

                    line = streamReader.ReadLine();
                    ++lineNo;
                }
            }

            if (recordFileError.ErrorCount > 0)
                throw recordFileError;
            return records;
        }

        /// <summary>
        /// Attempts to find the delimiter in the data returning the first 
        /// one it finds
        /// </summary>
        /// <param name="data">Data that will be searched</param>
        /// <returns>
        /// If a delimiter is found then it returns it. Otherwise, null is returned.
        /// </returns>
        private char? ExtractDelimiter(string data)
        {
            if (data == null)
                return null;

            foreach (var delimiter in Delimiters)
                if (data.IndexOf(delimiter) != -1)
                    return delimiter;

            return null;
        }

        /// <summary>
        /// Goes through as much of the file as necessary to find the delimiter used in the file. 
        /// In a correctly formatted file it should be in the first line. Assumes no file contains
        /// more than one delimiter - not even in the data.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>
        /// The delimiter or null if it could not be found.
        /// </returns>
        private char? ExtractDelimiterFromFile(string fileName)
        {
            using (var streamReader = new StreamReader(fileName))
            {
                var line = streamReader.ReadLine();
                while (line != null)
                {
                    var delimiter = ExtractDelimiter(line);
                    if (delimiter != null)
                        return delimiter;
                }
            }
            return null;
        }

        /// <summary>
        /// Helper used by both public parse methods to parse one record.
        /// </summary>
        /// <param name="data">The data to parse.</param>
        /// <param name="delimiter">The delimiter in the data.</param>
        /// <returns>
        /// Returns the record if successful. Otherwise, returns null.
        /// </returns>
        private Record ParseRecord(string data, char delimiter)
        {
            var fields = data.Split(delimiter);
            if (fields.Length != FieldCount)
                return null;

            // To-do
            //
            // Error handling: what to do when missing fields or encounter bad gender or date of birth values
            //
            var record = new Record
            {
                LastName = fields[LastNameIndex],
                FirstName = fields[FirstNameIndex],
                Gender = string.IsNullOrWhiteSpace(fields[GenderIndex]) ? Gender.Unknown : (Gender)Enum.Parse(typeof(Gender), fields[GenderIndex]),
                FavoriteColor = fields[FavoriteColorIndex],
                BirthDate = string.IsNullOrWhiteSpace(fields[BirthDateIndex]) ? (DateTime?)null : DateTime.Parse(fields[BirthDateIndex])
            };
            return record;
        }
    }
}
