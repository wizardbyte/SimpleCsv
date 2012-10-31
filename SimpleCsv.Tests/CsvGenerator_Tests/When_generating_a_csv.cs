using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCsv;
using Population;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;

namespace SimpleCsv.Tests
{
    [TestClass]
    public class When_generating_a_csv
    {
        protected People people;
        protected CsvGenerator generator;
        protected IQueryable<Person> source;
        protected int personPropertyCount;
        protected string fileSavePath = "testCsvFile.csv";
        
        [TestInitialize]
        public void Setup()
        {
            people = new People();
            generator = new CsvGenerator();
            source = people.QueryThePeople;
            personPropertyCount = GetPropertiesCount(source.First());
        }

        [TestCleanup]
        public void TearDown()
        {
            if (File.Exists(fileSavePath))
            {
                File.Delete(fileSavePath);
            }
        }

        [TestMethod]
        public void file_should_not_be_empty()
        {
            generator.CollectionToCsvFile(source, fileSavePath);
            string[] lines = File.ReadAllLines(fileSavePath);
            Assert.AreNotEqual(0, lines.Count(), "File line count should not be 0.");
            
        }

        [TestMethod]
        public void file_should_exist()
        {
            //Need to add a new method that will return a file or filestream or somethign for large data sets
            //in order to avoid out of memory errors.
            generator.CollectionToCsvFile(source, fileSavePath);
            Assert.IsTrue(File.Exists(fileSavePath));
        }

        [TestMethod]
        public void passing_collection_to_ObjectToCsv_should_return_empty_string()
        {
            string csv = generator.ObjectToCsv(source);
            Assert.AreEqual("", csv, "IQueryable as input produced non-empty string.");
        }

        [TestMethod]
        public void passing_null_object_should_throw_exception()
        {
            try
            { 
                generator.ObjectToCsv(null);
                Assert.Fail("An ArgumentNullException must be thrown when null object is passed in.");
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException), "Incorrect exception type is being thrown. ArgumentNullException was expected.");
            }
        }

        [TestMethod]
        public void converting_single_object_should_return_correct_values()
        {
            Person guy = source.First();
            string csv = generator.ObjectToCsv(guy);
            string[] results = csv.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            bool isCorrect = guy.firstName.Equals(results[0].TrimStart(new char[] {'"'}))
                && guy.middleName.Equals(results[1])
                && guy.lastName.Equals(results[2])
                && guy.birthdate.ToString().Equals(results[3])
                && guy.favouriteColour.ToString().Equals(results[4])
                && guy.quote.Equals(results[5].Trim().TrimEnd(new char[] {'"'}));

            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void converting_single_object_should_return_correct_number_of_columns_when_field_contains_commas()
        {
            Person withCommas = source.Where(p => p.quote.Contains(',')).First();
            string csv = generator.ObjectToCsv(withCommas);
            string[] results = csv.Split(new string[] {"\",\""}, StringSplitOptions.None);

            Assert.AreEqual(personPropertyCount, results.Length);
        }

        [TestMethod]
        public void should_return_correct_number_of_columns()
        {
            string csv = generator.ObjectToCsv(source.First());
            string[] results = csv.Split(new char[] {','});

            Assert.AreEqual(personPropertyCount, results.Length);            
        }

        [TestMethod]
        public void should_return_comma_delimited_values()
        {
            string csv = generator.CollectionToCsv(source);
            string firstRow = csv.Substring(0, csv.IndexOf("\r\n"));
            Assert.IsTrue(firstRow.Contains(','));
        }

        [TestMethod]
        public void multiple_objects_should_return_same_amount_of_rows()
        {
            string csv = generator.CollectionToCsv(source);
            string[] result = csv.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(source.Count(), result.Length, "result size does not equal input size");
        }

        [TestMethod]
        public void collection_with_single_object_should_return_with_a_single_row()
        {
            //Pass a collection containing a single object to the generator
            var local_source = source.Take(1);
            string csv = generator.CollectionToCsv(local_source);
            string[] result = csv.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(local_source.Count(), result.Length, "When passed a collection of size 1 it should return only a single row");
        }

        [TestMethod]
        public void should_not_return_null()
        {
            string csv = generator.CollectionToCsv(source);
            Assert.IsNotNull(csv, "generator should not return null");
        }

        [TestMethod]
        public void should_not_return_empty_string()
        {
            string csv = generator.CollectionToCsv(source);
            Assert.IsTrue(csv.Length > 0, "Generator should not return an empty string");
        }

        [TestMethod]
        public void passing_null_collection_should_return_null()
        {
            Assert.IsNull(generator.CollectionToCsv(null));
        }

        private int GetPropertiesCount(object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] pinfo = t.GetProperties();
            int expectedColumns = pinfo.Count();
            return expectedColumns;
        }
    }
}
