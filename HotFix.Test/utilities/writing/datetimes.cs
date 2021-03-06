﻿using System;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.writing
{
    [TestClass]
    public class datetimes
    {
        private byte[] _buffer;

        [TestInitialize]
        public void Setup()
        {
            _buffer = new byte[23];
        }

        [TestMethod]
        public void timestamp()
        {
            var written = _buffer.WriteDateTime(1, "2017/03/27 15:45:13.000".AsDateTime());

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "20170327-15:45:13.000" + "\0");
            written.Should().Be(21);
        }

        [TestMethod]
        public void timestamp_with_milliseconds()
        {
            var written = _buffer.WriteDateTime(1, "2017/03/27 15:45:13.123".AsDateTime());

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "20170327-15:45:13.123" + "\0");
            written.Should().Be(21);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void out_of_bounds()
        {
            _buffer.WriteDateTime(7, "2017/03/27 15:45:13.123".AsDateTime());
        }
    }
}
