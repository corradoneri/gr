using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GR.Records.Core.Models;
using GR.Records.Core.Parser;

namespace GR.Records.Core.Test
{
    [TestClass]
    public class RecordParserTests
    {
        [TestMethod]
        public void ParseRecord_FemalePipe_Valid()
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
            Assert.AreEqual(record.DateOfBirth, new DateTime(2002, 2, 12));
        }

        [TestMethod]
        public void ParseRecord_FemaleComma_Valid()
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
            Assert.AreEqual(record.DateOfBirth, new DateTime(2002, 2, 12));
        }

        [TestMethod]
        public void ParseRecord_FemaleSpace_Valid()
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
            Assert.AreEqual(record.DateOfBirth, new DateTime(2002, 2, 12));
        }

        [TestMethod]
        public void ParseRecord_MalePipe_Valid()
        {
            var data = "Doe|John|Male|Blue|2001-01-11";
            var separator = '|';
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
            Assert.AreEqual(record.DateOfBirth, new DateTime(2001, 1, 11));
        }


        [TestMethod]
        public void ParseRecord_NoData_Invalid()
        {
            var data = "";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            // Check that a Record instance was created and returned
            //
            Assert.IsNull(record);
        }

        [TestMethod]
        public void ParseRecord_MissingData_Pipe_Invalid()
        {
            var data = "Doe|John|Male|Blue";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            // Check that a Record instance was created and returned
            //
            Assert.IsNull(record);
        }

        [TestMethod]
        public void ParseRecord_TooManyFields_Pipe_Invalid()
        {
            var data = "Doe|John|Male|Blue|2001-01-01|Programmer";
            var recordParser = new RecordParser();
            var record = recordParser.ParseRecord(data);

            // Check that a Record instance was created and returned
            //
            Assert.IsNull(record);
        }

        [TestMethod]
        public void ParseFile_0Records_Valid()
        {
            var path = @"Data\0-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseRecordFile(path);

            Assert.AreEqual(recordList.Count, 0);
        }

        [TestMethod]
        public void ParseFile_2Records_Pipe_Valid()
        {
            var path = @"Data\2-Pipe-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseRecordFile(path);

            Assert.AreEqual(recordList.Count, 2);
        }

        [TestMethod]
        public void ParseFile_2Records_Comma_Valid()
        {
            var path = @"Data\2-Comma-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseRecordFile(path);

            Assert.AreEqual(recordList.Count, 2);
        }

        [TestMethod]
        public void ParseFile_2Records_Space_Valid()
        {
            var path = @"Data\2-Space-Valid.txt";
            var recordParser = new RecordParser();
            var recordList = recordParser.ParseRecordFile(path);

            Assert.AreEqual(recordList.Count, 2);
        }
    }
}
