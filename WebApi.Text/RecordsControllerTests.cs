using System;
using System.Collections.Generic;
using System.Linq;
using GR.Records.Core.DataAccess;
using GR.Records.Core.Models;
using GR.Records.Core.Parser;
using GR.Records.Core.Sorter;
using GR.Records.WebApi.Controllers;
using GR.Records.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApi.Text
{
    [TestClass]
    public class RecordsControllerTests
    {
        private RecordsController _recordsController;

        [TestInitialize]
        public void Setup()
        {
            // Would normally create some mock objects. In this case
            // the record repository pretty much already is a mock 
            // implementation of a repository anyway, i.e., 
            // it doesn't really access a database.
            //
            var recordParser = new RecordParser();
            var recordSorter = new RecordSorter();
            var recordRepository = new RecordRepository(recordSorter);
            _recordsController = new RecordsController(recordParser, recordRepository);
        }

        [TestMethod]
        public void Post_MalePipe()
        {
            var data = "Doe|John|Male|Blue|1990-01-01";
            var result = _recordsController.Post(data) as CreatedResult;

            Assert.IsNotNull(result);

            var recordViewModel = result.Value as RecordViewModel;

            Assert.IsNotNull(recordViewModel);
            Assert.AreEqual(recordViewModel.LastName, "Doe");
            Assert.AreEqual(recordViewModel.FirstName, "John");
            Assert.AreEqual(recordViewModel.Gender, "Male");
            Assert.AreEqual(recordViewModel.FavoriteColor, "Blue");
            Assert.AreEqual(recordViewModel.BirthDate, "1/1/1990");
        }
        
        [TestMethod]
        public void Post_NoData()
        {
            var data = "";
            var result = _recordsController.Post(data);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Post_OnlyWhiteSpace()
        {
            var data = "   ";
            var result = _recordsController.Post(data);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Post_BadDelimiter()
        {
            var data = "Doe.John.Male.Blue.2001-01-11";
            var result = _recordsController.Post(data);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetGender_3Records()
        {
            _recordsController.Post("Doe|John|Male|Blue|1995-01-01");
            _recordsController.Post("Sanders|Jane|Female|Blue|1994-02-02");
            _recordsController.Post("Smith|Jane|Female|Brown|2002-02-13");

            var records = _recordsController.GetGender() as IEnumerable<RecordViewModel>;

            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count(), 3);

            var list = records.ToList();
            Assert.AreEqual(list[0].LastName, "Sanders");
            Assert.AreEqual(list[1].LastName, "Smith");
            Assert.AreEqual(list[2].LastName, "Doe");
        }

        [TestMethod]
        public void GetBirthdate_3Records()
        {
            _recordsController.Post("Doe|John|Male|Blue|1995-01-01");
            _recordsController.Post("Sanders|Jane|Female|Blue|1994-02-02");
            _recordsController.Post("Smith|Jane|Female|Brown|2002-02-13");

            var records = _recordsController.GetBirthDate() as IEnumerable<RecordViewModel>;

            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count(), 3);

            var list = records.ToList();
            Assert.AreEqual(list[0].LastName, "Sanders");
            Assert.AreEqual(list[1].LastName, "Doe");
            Assert.AreEqual(list[2].LastName, "Smith");
        }


        [TestMethod]
        public void GetName_3Records()
        {
            _recordsController.Post("Doe|John|Male|Blue|1995-01-01");
            _recordsController.Post("Sanders|Jane|Female|Blue|1994-02-02");
            _recordsController.Post("Smith|Jane|Female|Brown|2002-02-13");

            var records = _recordsController.GetName() as IEnumerable<RecordViewModel>;

            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count(), 3);

            var list = records.ToList();
            Assert.AreEqual(list[0].LastName, "Smith");
            Assert.AreEqual(list[1].LastName, "Sanders");
            Assert.AreEqual(list[2].LastName, "Doe");
        }

        [TestMethod]
        public void Get_0Records()
        {
            var records = _recordsController.GetGender() as IEnumerable<RecordViewModel>;

            Assert.IsNotNull(records);
            Assert.AreEqual(records.Count(), 0);
        }
    }
}
