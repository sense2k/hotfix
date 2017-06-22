﻿using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.core.messagewriter
{
    [TestClass]
    public class building_a_message
    {
        [TestMethod]
        public void should_work()
        {
            var message = new MessageWriter(1000, "FIX.4.2");

            message.Prepare("A");
            message.Set(49, "SERVER");
            message.Set(56, "CLIENT");
            message.Set(34, 177);
            message.Set(52, DateTime.Parse("2009/01/07 18:15:16"));
            message.Set(98, 0);
            message.Set(108, 30);
            message.Build();

            var str = message.ToString();

            str.Should().Be("8=FIX.4.2|9=00069|35=A|49=SERVER|56=CLIENT|34=177|52=20090107-18:15:16.000|98=0|108=30|10=144|".Replace("|", "\u0001"));
        }

        [TestMethod]
        public void allows_the_message_to_be_reused()
        {
            var message = new MessageWriter(1000, "FIX.4.2");

            // Prepare a message
            message.Prepare("A");
            message.Set(49, "SERVER");
            message.Set(56, "CLIENT");
            message.Set(34, 177);
            message.Set(52, DateTime.Parse("2009/01/07 18:15:16"));
            message.Set(98, 0);
            message.Set(108, 30);
            message.Build();

            // Overwrite the previous message with a new message
            message.Prepare("0");
            message.Set(34, 8059);
            message.Set(52, DateTime.Parse("2017/05/31 08:18:01.767"));
            message.Set(49, "SENDER....");
            message.Set(56, "RECEIVER.....");
            message.Build();

            var str = message.ToString();

            str.Should().Be("8=FIX.4.2|9=00069|35=0|34=8059|52=20170531-08:18:01.767|49=SENDER....|56=RECEIVER.....|10=203|".Replace("|", "\u0001"));
        }
    }
}