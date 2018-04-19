using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GR.Records.Core.Models;
using GR.Records.Core.Parser;
using System.Linq;
using System.Collections.Generic;
using GR.Records.Core.Sorter;

namespace GR.Records.Core.Test
{
    [TestClass]
    public class RecordSorterTests
    {
        private List<Record> _records;

        [TestInitialize]
        public void Setup()
        {
            _records = new List<Record>
            {
                new Record
                {
                    LastName = "Sanders",
                    FirstName = "Jane",
                    Gender = Gender.Female,
                    FavoriteColor = "Green",
                    BirthDate = new DateTime(2001, 1, 13)
                },

                new Record
                {
                    LastName = "Murphy",
                    FirstName = "Bobbi",
                    Gender = Gender.Female,
                    FavoriteColor = "Brown",
                    BirthDate = new DateTime(2012, 2, 1)
                },

                new Record
                {
                    LastName = "Doe",
                    FirstName = "John",
                    Gender = Gender.Male,
                    FavoriteColor = "Blue",
                    BirthDate = new DateTime(1995, 12, 23)
                }
            };
        }

        [TestMethod]
        public void SortRecords_GenderAscLastNameAsc()
        {
            var recordSorter = new RecordSorter();
            var sortedRecords = recordSorter.SortRecords(_records, SortCriteria.GenderAscLastNameAsc).ToList<Record>();

            // Check the sort order
            //
            Assert.AreEqual(sortedRecords[0].LastName, "Murphy");
            Assert.AreEqual(sortedRecords[1].LastName, "Sanders");
            Assert.AreEqual(sortedRecords[2].LastName, "Doe");
        }

        [TestMethod]
        public void SortRecords_BirthDateAsc()
        {
            var recordSorter = new RecordSorter();
            var sortedRecords = recordSorter.SortRecords(_records, SortCriteria.BirthDateAsc).ToList<Record>();

            // Check the sort order
            //
            Assert.AreEqual(sortedRecords[0].LastName, "Doe");
            Assert.AreEqual(sortedRecords[1].LastName, "Sanders");
            Assert.AreEqual(sortedRecords[2].LastName, "Murphy");
        }

        [TestMethod]
        public void SortRecords_LastNameDesc()
        {
            var recordSorter = new RecordSorter();
            var sortedRecords = recordSorter.SortRecords(_records, SortCriteria.LastNameDesc).ToList<Record>();

            // Check the sort order
            //
            Assert.AreEqual(sortedRecords[0].LastName, "Sanders");
            Assert.AreEqual(sortedRecords[1].LastName, "Murphy");
            Assert.AreEqual(sortedRecords[2].LastName, "Doe");
        }

        [TestMethod]
        public void SortRecords_NoData()
        {
            var recordSorter = new RecordSorter();
            var emptyList = new List<Record>();
            var sortedRecords = recordSorter.SortRecords(emptyList, SortCriteria.GenderAscLastNameAsc).ToList<Record>();

            // Checking to make sure exception is not thrown and that sorted list is empty
            Assert.IsTrue(sortedRecords.Count == 0);
        }
    }
}
