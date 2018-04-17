using System;
using System.IO;
using GR.Records.Core.Models;

namespace GR.Records.Core.Parser
{
    public class RecordParser
    {
        private const int LastNameIndex = 0;
        private const int FirstNameIndex = 1;
        private const int GenderIndex = 2;
        private const int FavoriteColorIndex = 3;
        private const int DateOfBirthIndex = 4;
        private const int FieldCount = 5;

        private char[] Delimiters = { ',', '|', ' ' };

        public Record ParseRecord(string data)
        {
            var delimiter = ExtractDelimiter(data);
            if (!delimiter.HasValue)
                return null;
            return ParseRecord(data, delimiter.Value);
        }

        public RecordList ParseRecordFile(string path)
        {
            var streamReader = new StreamReader(path);
            var recordList = new RecordList();
            var line = streamReader.ReadLine();
            var delimiter = ExtractDelimiter(line);

            // To-do
            //
            // Error handling: abort if no delimiter on first line?
            //
            if (!delimiter.HasValue)
                return recordList;

            while (line != null)
            {
                // To-do
                // 
                // Error handling: ignore lines with errors?
                //
                var record = ParseRecord(line, delimiter.Value);
                if (record != null)
                    recordList.Add(record);
                line = streamReader.ReadLine();
            }
            return recordList;
        }

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
                DateOfBirth = DateTime.Parse(fields[DateOfBirthIndex])
            };
            return record;
        }
    }
}
