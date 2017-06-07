using System;
using FluentAssertions;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.parser
{
    [TestClass]
    public class ints
    {
        [TestMethod]
        public void zero()
        {
            "0".GetInt().Should().Be(0);
        }

        [TestMethod]
        public void positive()
        {
            "123".GetInt().Should().Be(123);
        }

        [TestMethod]
        public void positive_with_leading_zeros()
        {
            "000123".GetInt().Should().Be(123);
        }

        [TestMethod]
        public void negative()
        {
            "-123".GetInt().Should().Be(-123);
        }

        [TestMethod]
        public void negative_with_leading_zeros()
        {
            "-000123".GetInt().Should().Be(-123);
        }
    }
}