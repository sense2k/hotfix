﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using HotFix.Transport;

namespace HotFix.Specification
{
    public class Harness : ITransport
    {
        public VirtualClock Clock { get; }
        public List<string> Instructions { get; }

        public int Step { get; private set; }

        public Harness(List<string> instructions)
        {
            Instructions = instructions;
            Engine.Clock = Clock = new VirtualClock();

            var instruction = Instructions[Step++];

            if (instruction[0] != '!') throw new Exception("The first step should always be a time-set (!) instruction");

            Clock.Time = DateTime.ParseExact(instruction.Substring(2, instruction.Length - 2), "yyyyMMdd-HH:mm:ss.fff", null);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            // If there are no more instructions, we have completed the scenario
            if (Instructions.Count == Step) throw new DelightfullySuccessfulException();

            var instruction = Instructions[Step++];

            try
            {
                switch (instruction[0])
                {
                    case '!':
                        Clock.Time = DateTime.ParseExact(instruction.Substring(2, instruction.Length - 2), "yyyyMMdd-HH:mm:ss.fff", null);
                        return 0;
                    case '<':
                        System.Text.Encoding.UTF8.GetBytes(instruction, 2, instruction.Length - 2, buffer, offset);
                        return instruction.Length - 2;
                    case '>':
                        throw new Exception($"Outbound message expected but not received: {instruction}");
                    default:
                        throw new Exception($"Unrecognised instruction found in scenario: {instruction}");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error on step {Step} '{instruction}'", e);
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            var instruction = Instructions[Step++];

            try
            {
                var outbound = System.Text.Encoding.UTF8.GetString(buffer, offset, count);

                if (instruction[0] != '>') throw new Exception($"Unexpected outbound message received from engine: {outbound}");

                var expected = instruction.Substring(2, instruction.Length - 2);

                outbound.Should().Be(expected);
            }
            catch (Exception e)
            {
                throw new Exception($"Error on step {Step} '{instruction}'", e);
            }
        }

        public void Dispose()
        {

        }
    }
}