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
        // Constants and statics
        //
        private const int LastNameIndex = 0;
        private const int FirstNameIndex = 1;
        private const int GenderIndex = 2;
        private const int FavoriteColorIndex = 3;
        private const int BirthDateIndex = 4;
        private const int FieldCount = 5;

        private static char[] Delimiters = { ',', '|', ' ' };

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
            if (string.IsNullOrWhiteSpace(data))
                return null;

            var delimiter = ExtractDelimiter(data);
            if (!delimiter.HasValue)
                throw new RecordException("Could not find delimiter in record");

            var record = ParseRecord(data, delimiter.Value);
            if (record == null || !record.IsValid)
                return null;
            return record;
        }

        /// <summary>
        /// Parses all fo the files given in the fileNames array.
        /// </summary>
        /// <param name="fileName">The name of the file to parse.</param>
        /// <returns>
        /// Will return enumerable containing the data.
        /// </returns>
        /// <exception>
        /// Throws RecordFileError if any files are missing or invalid or if any general error occurs.
        /// </exception>
        public IEnumerable<Record> ParseFiles(IEnumerable<string> fileNames)
        {
            var recordFileException = new RecordFileException();
            var records = new List<Record>();

            foreach (var fileName in fileNames)
                records.AddRange(ParseFile(fileName, recordFileException));

            if (recordFileException.HasErrors)
                throw recordFileException;
            return records;
        }

        /// <summary>
        /// Attempts to find the delimiter in the data returning the first one it finds
        /// </summary>
        /// <param name="data">The data that will be searched.</param>
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
        /// Checks to see if a file contains nothing but whitespace
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns></returns>
        private bool IsFileEmpty(string fileName)
        {
            using (var streamReader = new StreamReader(fileName))
            {
                var line = streamReader.ReadLine();
                while (line != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        return false;
                    line = streamReader.ReadLine();
                }
            }
            return true;
        }

        /// <summary>
        /// Goes through as much of the file as necessary to find the delimiter used in the file. 
        /// In a correctly formatted file it should be in the first line. Assumes no file contains
        /// more than one delimiter - not even in the data.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
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
                    line = streamReader.ReadLine();
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

            try
            {
                var record = new Record
                {
                    LastName = fields[LastNameIndex],
                    FirstName = fields[FirstNameIndex],
                    Gender = string.IsNullOrWhiteSpace(fields[GenderIndex]) ? Gender.Unknown : (Gender)Enum.Parse(typeof(Gender), fields[GenderIndex], true),
                    FavoriteColor = fields[FavoriteColorIndex],
                    BirthDate = string.IsNullOrWhiteSpace(fields[BirthDateIndex]) ? (DateTime?)null : DateTime.Parse(fields[BirthDateIndex])
                };
                return record;
            }
            catch (Exception)
            {
                // Ignoring specifics of error for now and just returning an empty 
                // record indicating that an error occurred
                //
            }
            return null;
        }

        /// <summary>
        /// Parses the entire file.
        /// </summary>
        /// <param name="fileName">The name of the file to parse.</param>
        /// <returns>
        /// Will return enumerable containing the data.
        /// </returns>
        /// <exception>
        /// Throws RecordFileError if file is missing or invalid or if any general error occurs.
        /// </exception>
        public IEnumerable<Record> ParseFile(string fileName, RecordFileException recordFileException)
        {
            var records = new List<Record>();

            // If file doesn't exist record an error and return right away
            //
            if (!File.Exists(fileName))
            {
                recordFileException.AddError(fileName, "File not found");
                return records;
            }

            // Do not attempt to parse empty files
            //
            if (IsFileEmpty(fileName))
                return records;

            try
            {
                // Try to determine the delimiter used in the file by inspecting the file for each
                // possible delimiter until one is found. 
                //
                // Asumption is that the first delimiter found is used through the entire file. 
                //
                var delimiter = ExtractDelimiterFromFile(fileName);
                if (!delimiter.HasValue)
                {
                    recordFileException.AddError(fileName, "Could not find a valid delimiter");
                    return records;
                }

                // Open file and parse each line recording errors along the way
                //
                using (var streamReader = new StreamReader(fileName))
                {
                    var line = streamReader.ReadLine();
                    var lineNo = 0UL;
                    while (line != null)
                    {
                        // Ignoring empty lines
                        //
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            line = streamReader.ReadLine();
                            continue;
                        }

                        var record = ParseRecord(line, delimiter.Value);
                        if (record == null || !record.IsValid)
                            recordFileException.AddError(fileName, lineNo, "Invalid record");
                        else
                            records.Add(record);

                        line = streamReader.ReadLine();
                        ++lineNo;
                    }
                }
            }
            catch (Exception exception)
            {
                recordFileException.AddError(fileName, exception.Message);
            }
            return records;
        }
    }
}
