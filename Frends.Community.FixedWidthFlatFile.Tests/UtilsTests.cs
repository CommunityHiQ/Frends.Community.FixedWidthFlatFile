using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Frends.Community.FixedWidthFlatFile.Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void ParseDataRow_CreatesFieldNames()
        {
            var testData = "firstValuesecondValuethirdValueTrue12.1";
            var columns = new ColumnSpecification[]
            {
                new ColumnSpecification{Type=ColumnType.String, Length=10},
                new ColumnSpecification{Type=ColumnType.String, Length = 11},
                new ColumnSpecification{Type=ColumnType.String, Length = 10},
                new ColumnSpecification{Type = ColumnType.Boolean, Length=4},
                new ColumnSpecification{Type = ColumnType.Double, Length=4}
            };
            var result = Utils.ParseDataRow(testData, columns);

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("Field_1", result.Keys.First());
            Assert.AreEqual("Field_5", result.Keys.Last());
        }

        [TestMethod]
        public void ParseDataRow_TypeCasting_Test()
        {
            var testData = "truet1,51234567890201804249";
            var columns = new ColumnSpecification[] {
                new ColumnSpecification{Type = ColumnType.Boolean, Length = 4, Name = "Boolean"},
                new ColumnSpecification{Type = ColumnType.Char, Length = 1, Name = "Char"},
                new ColumnSpecification{Type = ColumnType.Double, Length = 3, Name = "Double"},
                new ColumnSpecification{Type = ColumnType.Long, Length = 10, Name = "Long"},
                new ColumnSpecification{Type = ColumnType.DateTime, DateTimeFormat = "yyyyMMdd", Length = 8, Name = "DateTime"},
                new ColumnSpecification{Type = ColumnType.Int, Length = 1, Name = "Int"}
            };
            var result = Utils.ParseDataRow(testData, columns);

            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(typeof(bool), result["Boolean"].GetType());
            Assert.IsTrue(result["Char"] is Char);
            Assert.AreEqual(typeof(double), result["Double"].GetType());
            Assert.AreEqual(typeof(long), result["Long"].GetType());
            Assert.AreEqual(typeof(DateTime), result["DateTime"].GetType());
            Assert.AreEqual(typeof(int), result["Int"].GetType());
        }
    }
}
