using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleCsv;
using Population;

namespace SimpleCsv.Tests_NUnit
{
    [TestFixture]
    public class CsvGeneratorSpecs
    {
        protected People people;
        protected CsvGenerator generator;

        [SetUp]
        public void Setup()
        {
            people = new People();
        }

        //[Test]
        //public void Generator_should_preserve_data_source()
        //{
        //    var source = people.QueryThePeople;
        //    Type t = source.GetType();
        //    generator = new CsvGenerator(source);
        //    Assert.AreEqual(generator.DataSource.GetType(), t);
        //}
    }
}
