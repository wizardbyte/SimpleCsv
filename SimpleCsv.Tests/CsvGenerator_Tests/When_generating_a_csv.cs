using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCsv;
using Population;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SimpleCsv.Tests
{
    [TestClass]
    public class When_generating_a_csv
    {
        protected People people;
        protected CsvGenerator generator;
        protected IQueryable<Person> source;
        protected int personPropertyCount;
        
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
            
        }

        [TestMethod]
        public void should_return_correct_values()
        {
            var local_source = source.Take(1);
            string csv = generator.ObjectToCsvString(local_source);
            string[] results = csv.Split(new string[] { "\",\"" }, StringSplitOptions.None);
            Person guy = local_source.First();
            //bool isCorrect = guy.firstName.Equals(results[0].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries).First())
            //    && guy.middleName.Equals(results[1].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries).First())
            //    && guy.lastName.Equals(results[2].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries).First())
            //    && guy.birthdate.ToString().Equals(results[3].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries).First())
            //    && guy.favouriteColour.ToString().Equals(results[4].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries).First())
            //    && guy.quote.Equals(results[5].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries).First());
            bool isCorrect = guy.firstName.Equals(results[0].TrimStart(new char[] {'"'}))
                && guy.middleName.Equals(results[1])
                && guy.lastName.Equals(results[2])
                && guy.birthdate.ToString().Equals(results[3])
                && guy.favouriteColour.ToString().Equals(results[4])
                && guy.quote.Equals(results[5].Trim().TrimEnd(new char[] {'"'}));

            Assert.IsTrue(isCorrect);

        }

        [TestMethod]
        public void should_return_correct_number_of_columns_when_field_contains_commas()
        {
            var withCommas = source.Where(p => p.quote.Contains(',')).Take(1);
            string csv = generator.ObjectToCsvString(withCommas);
            string[] results = csv.Split(new string[] {"\",\""}, StringSplitOptions.None);

            Assert.AreEqual(personPropertyCount, results.Length);
        }

        [TestMethod]
        public void should_return_correct_number_of_columns()
        {
            string csv = generator.ObjectToCsvString(source.Take(1));
            string[] results = csv.Split(new char[] {','});

            Assert.AreEqual(personPropertyCount, results.Length);            
        }

        private int GetPropertiesCount(object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] pinfo = t.GetProperties();
            int expectedColumns = pinfo.Count();
            return expectedColumns;
        }

        [TestMethod]
        public void should_return_comma_delimited_values()
        {
            string csv = generator.ObjectToCsvString(source);
            string firstRow = csv.Substring(0, csv.IndexOf("\r\n"));
            Assert.IsTrue(firstRow.Contains(','));
        }

        [TestMethod]
        public void multiple_objects_should_return_same_amount_of_rows()
        {
            string csv = generator.ObjectToCsvString(source);
            string[] result = csv.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(source.Count(), result.Length, "result size does not equal input size");
        }

        [TestMethod]
        public void single_object_should_return_string_with_a_single_row()
        {
            //Pass a collection containing a single object to the generator
            var local_source = source.Take(1);
            string csv = generator.ObjectToCsvString(local_source);
            string[] result = csv.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(local_source.Count(), result.Length, "When passed a collection of size 1 it should return only a single row");
        }

        [TestMethod]
        public void should_not_return_null()
        {
            string csv = generator.ObjectToCsvString(source);
            Assert.IsNotNull(csv, "generator should not return null");
        }

        [TestMethod]
        public void should_not_return_empty_string()
        {
            string csv = generator.ObjectToCsvString(source);
            Assert.IsTrue(csv.Length > 0, "Generator should not return an empty string");
        }

        [TestMethod]
        public void should_return_null_if_passed_null()
        {
            Assert.IsNull(generator.ObjectToCsvString(null));
        }
    }
}
