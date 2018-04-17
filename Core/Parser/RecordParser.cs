using System;
using System.Collections.Generic;
using System.IO;
using GR.Records.Core.Models;

namespace GR.Records.Core.Parser
{
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

        // IRecordParser implementation
        //
        public Record ParseRecord(string data)
        {
            var delimiter = ExtractDelimiter(data);
            if (!delimiter.HasValue)
                return null;
            return ParseRecord(data, delimiter.Value);
        }

        public IEnumerable<Record> ParseRecordFile(string fileName)
        {
            var streamReader = new StreamReader(fileName);
            var line = streamReader.ReadLine();
            var delimiter = ExtractDelimiter(line);

            // To-do
            //
            // Error handling: abort if no delimiter on first line?
            //
            if (delimiter.HasValue)
            {
                while (line != null)
                {
                    // To-do
                    // 
                    // Error handling: ignore lines with errors?
                    //
                    var record = ParseRecord(line, delimiter.Value);
                    if (record != null)
                        yield return record;
                    line = streamReader.ReadLine();
                }
            }
        }

        // Helper methods
        //
        private char? ExtractDelimiter(string data)
        {
            if (data == null)
                return null;

            foreach (var delimiter in Delimiters)
                if (data.IndexOf(delimiter) != -1)
                    return delimiter;

            return null;
        }

        private Record ParseRecord(string data, char delimiter)
        {
            var fields = data.Split(delimiter);
            if (fields.Length != 5)
                return null;

            // To-do
            //
            // Error handling: what to do when missing fields or encounter bad gender or date of birth values
            //
            var record = new Record
            {
                LastName = fields[LastNameIndex],
                FirstName = fields[FirstNameIndex],
                Gender = (Gender)Enum.Parse(typeof(Gender), fields[GenderIndex]),
                FavoriteColor = fields[FavoriteColorIndex],
                BirthDate = DateTime.Parse(fields[BirthDateIndex])
            };
            return record;
        }
    }
}
