using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Frends.Community.FixedWidthFlatFile.Tests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void SplitToList_WithDelimiter_Test()
        {
            var input = "h1;h2;h3;h4,h5";
            var result = input.SplitToList(';');

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("h4,h5", result.Last());
        }

        [TestMethod]
        public void SplitToList_FixedWidth_Test()
        {
            var input = "hodor22tenchars10";
            var columnSpecification = new ColumnSpecification[3];
            columnSpecification[0] = new ColumnSpecification { Name = "h1", Length = 5, Type = ColumnType.String };
            columnSpecification[1] = new ColumnSpecification { Name = "h2", Length = 2, Type = ColumnType.Int };
            columnSpecification[2] = new ColumnSpecification { Name = "h3", Length = 10, Type = ColumnType.String };

            var result = input.SplitToList(columnSpecification);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("hodor", result.First());
            Assert.AreEqual("tenchars10", result.Last());
        }
    }
}
