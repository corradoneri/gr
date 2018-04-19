using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GR.Records.Core.Models;
using GR.Records.Core.Parser;
using System.Linq;
using GR.Records.Core.Exceptions;

namespace GR.Records.Core.Test
{
    [TestClass]
    public class RecordParserTests
    {
        // **********************************************
        // Test cases for RecordParser.ParseRecord method
        // **********************************************

        [TestMethod]
        public void ParseRecord_FemalePipe()
        {
            var data = "Smith|Jane|Female|Blue|2002-02-12";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            // Check that a Record instance was created and returned
            //
            Assert.IsNotNull(record);
            Assert.IsInstanceOfType(record, typeof(Record));

            // Check that values were correctly set
            //
            Assert.AreEqual(record.LastName, "Smith");
            Assert.AreEqual(record.FirstName, "Jane");
            Assert.AreEqual(record.Gender, Gender.Female);
            Assert.AreEqual(record.FavoriteColor, "Blue");
            Assert.AreEqual(record.BirthDate, new DateTime(2002, 2, 12));
        }

        [TestMethod]
        public void ParseRecord_FemaleComma()
        {
            var data = "Smith,Jane,Female,Blue,2002-02-12";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            // Check that a Record instance was created and returned
            //
            Assert.IsNotNull(record);
            Assert.IsInstanceOfType(record, typeof(Record));

            // Check that values were correctly set
            //
            Assert.AreEqual(record.LastName, "Smith");
            Assert.AreEqual(record.FirstName, "Jane");
            Assert.AreEqual(record.Gender, Gender.Female);
            Assert.AreEqual(record.FavoriteColor, "Blue");
            Assert.AreEqual(record.BirthDate, new DateTime(2002, 2, 12));
        }

        [TestMethod]
        public void ParseRecord_FemaleSpace()
        {
            var data = "Smith Jane Female Blue 2002-02-12";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            // Check that a Record instance was created and returned
            //
            Assert.IsNotNull(record);
            Assert.IsInstanceOfType(record, typeof(Record));

            // Check that values were correctly set
            //
            Assert.AreEqual(record.LastName, "Smith");
            Assert.AreEqual(record.FirstName, "Jane");
            Assert.AreEqual(record.Gender, Gender.Female);
            Assert.AreEqual(record.FavoriteColor, "Blue");
            Assert.AreEqual(record.BirthDate, new DateTime(2002, 2, 12));
        }

        [TestMethod]
        public void ParseRecord_MalePipe()
        {
            var data = "Doe|John|Male|Blue|2001-01-11";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            // Check that a Record instance was created and returned
            //
            Assert.IsNotNull(record);
            Assert.IsInstanceOfType(record, typeof(Record));

            // Check that values were correctly set
            //
            Assert.AreEqual(record.LastName, "Doe");
            Assert.AreEqual(record.FirstName, "John");
            Assert.AreEqual(record.Gender, Gender.Male);
            Assert.AreEqual(record.FavoriteColor, "Blue");
            Assert.AreEqual(record.BirthDate, new DateTime(2001, 1, 11));
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_NoData()
        {
            var data = "";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_OnlyWhiteSpace()
        {
            var data = "   ";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_BadDelimiter()
        {
            var data = "Doe.John.Male.Blue.2001-01-11";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            Assert.IsNull(record);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_MissingData_Pipe()
        {
            var data = "Doe|John|Male|Blue";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_TooManyFields_Pipe()
        {
            var data = "Doe|John|Male|Blue|2001-01-01|Programmer";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_MissingFirstName()
        {
            var data = "Doe||Male|Blue|2001-01-01";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_MissingLastName()
        {
            var data = "|John|Male|Blue|2001-01-01";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        [TestMethod]
        public void ParseRecord_MissingGender()
        {
            var data = "Doe|John||Blue|2001-01-01";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            Assert.IsNotNull(record);
        }

        [TestMethod]
        public void ParseRecord_MixedCaseGender()
        {
            var data = "Doe|John|mAlE|Blue|2001-01-01";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            Assert.IsNotNull(record);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_BadGender()
        {
            var data = "Doe|John|Mail|Blue|2001-01-01";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        [TestMethod]
        public void ParseRecord_MissingColor()
        {
            var data = "Doe|John|Male||2001-01-01";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            Assert.IsNotNull(record);
        }

        [TestMethod]
        public void ParseRecord_MissingBirthDate()
        {
            var data = "Doe|John|Male|Blue|";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            Assert.IsNotNull(record);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordException))]
        public void ParseRecord_BadBirthDate()
        {
            var data = "Doe|John|Male|Blue|2001-a1-01";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);
        }

        // *********************************************
        // Test cases for RecordParser.ParseFiles method
        // *********************************************

        [TestMethod]
        public void ParseFiles_0Records()
        {
            var path = @"Data\0-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new [] { path } );

            Assert.AreEqual(recordList.Count(), 0);
        }

        [TestMethod]
        public void ParseFiles_2Records_Pipe()
        {
            var path = @"Data\2-Pipe-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });

            Assert.AreEqual(recordList.Count(), 2);
        }

        [TestMethod]
        public void ParseFiles_2Records_Comma()
        {
            var path = @"Data\2-Comma-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });

            Assert.AreEqual(recordList.Count(), 2);
        }

        [TestMethod]
        public void ParseFiles_2Records_Space()
        {
            var path = @"Data\2-Space-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });

            Assert.AreEqual(recordList.Count(), 2);
        }

        [TestMethod]
        public void ParseFiles_3Files_6Records()
        {
            var paths = new string[]
            {
                @"Data\2-Pipe-Valid.txt",
                @"Data\2-Comma-Valid.txt",
                @"Data\2-Space-Valid.txt"
            };
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(paths);

            Assert.AreEqual(recordList.Count(), 6);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordFileException))]
        public void ParseFiles_BadDelimiter()
        {
            var path = @"Data\2-BadDelimiter-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });
        }

        [TestMethod]
        [ExpectedException(typeof(RecordFileException))]
        public void ParseFiles_MissingFirstName()
        {
            var path = @"Data\2-MissingFirstName-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });
        }

        [TestMethod]
        [ExpectedException(typeof(RecordFileException))]
        public void ParseFiles_MissingLastName()
        {
            var path = @"Data\2-MissingLastName-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });
        }

        [TestMethod]
        public void ParseFiles_MissingGender()
        {
            var path = @"Data\2-MissingGender-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });

            Assert.AreEqual(recordList.Count(), 2);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordFileException))]
        public void ParseFiles_BadGender()
        {
            var path = @"Data\2-BadGender-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });
        }

        [TestMethod]
        public void ParseFiles_MissingColor()
        {
            var path = @"Data\2-MissingColor-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });

            Assert.AreEqual(recordList.Count(), 2);
        }

        [TestMethod]
        public void ParseFiles_MissingBirthDate()
        {
            var path = @"Data\2-MissingBirthDate-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });

            Assert.AreEqual(recordList.Count(), 2);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordFileException))]
        public void ParseFiles_BadBirthDate()
        {
            var path = @"Data\2-BadBirthDate-Invalid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseFiles(new[] { path });
        }
    }
}
