﻿using System;
using NUnit.Framework;

namespace NSubstitute.Acceptance.Specs
{
    [TestFixture]
    public class SequenceChecking
    {
        private IFoo _foo;
        private IBar _bar;

        [Test]
        [Pending]
        public void Pass_when_checking_a_single_call_that_was_in_the_sequence()
        {
            _foo.Start();
            Received.InOrder(() => _foo.Start());
        }

        [Test]
        [Pending]
        public void Fail_when_checking_a_single_call_that_was_not_in_the_sequence()
        {
            _foo.Start();
            Assert.Throws<CallSequenceNotFound>(() =>
                Received.InOrder(() => _foo.Finish())
                );
        }

        [Test]
        [Pending]
        public void Pass_when_calls_match_exactly()
        {
            _foo.Start(2);
            _bar.Begin();
            _foo.Finish();
            _bar.End();

            Received.InOrder(() =>
            {
                _foo.Start(2);
                _bar.Begin();
                _foo.Finish();
                _bar.End();

            });
        }

        [Test]
        [Pending]
        public void Fail_when_call_arg_does_not_match()
        {
            _foo.Start(1);
            _bar.Begin();
            _foo.Finish();
            _bar.End();

            Assert.Throws<CallSequenceNotFound>(() =>
                Received.InOrder(() =>
                                     {
                                         _foo.Start(2);
                                         _bar.Begin();
                                         _foo.Finish();
                                         _bar.End();
                                     })
                );
        }

        [Test]
        [Pending]
        public void Use_arg_matcher()
        {
            _foo.Start(1);
            _bar.Begin();
            _foo.Finish();
            _bar.End();

            Received.InOrder(() =>
            {
                _foo.Start(Arg.Is<int>(x => x < 10));
                _bar.Begin();
                _foo.Finish();
                _bar.End();

            });
        }

        [Test]
        [Pending]
        public void Fail_when_calls_made_in_different_order()
        {
            _foo.Finish();
            _foo.Start();

            Assert.Throws<CallSequenceNotFound>(() =>
                Received.InOrder(() =>
                                 {
                                     _foo.Start();
                                     _foo.Finish();
                                 })
            );
        }

        [Test]
        [Pending]
        public void Fail_when_one_of_the_calls_in_the_sequence_was_not_received()
        {
            _foo.Start();
            _foo.Finish();

            Assert.Throws<CallSequenceNotFound>(() =>
                Received.InOrder(() =>
                {
                    _foo.Start();
                    _foo.FunkyStuff("hi");
                    _foo.Finish();
                })
            );
        }

        [Test]
        [Pending]
        public void Fail_when_additional_related_calls_at_start()
        {
            _foo.Start();
            _foo.Start();
            _bar.Begin();
            _foo.Finish();
            _bar.End();

            Assert.Throws<CallSequenceNotFound>(() =>
                Received.InOrder(() =>
                {
                    _foo.Start();
                    _bar.Begin();
                    _foo.Finish();
                    _bar.End();
                })
            );
        }

        [Test]
        [Pending]
        public void Fail_when_additional_related_calls_at_end()
        {
            _foo.Start();
            _bar.Begin();
            _foo.Finish();
            _bar.End();
            _foo.Start();

            Assert.Throws<CallSequenceNotFound>(() =>
                Received.InOrder(() =>
                {
                    _foo.Start();
                    _bar.Begin();
                    _foo.Finish();
                    _bar.End();
                })
            );
        }

        [Test]
        [Pending]
        public void Ignore_unrelated_calls()
        {
            _foo.Start();
            _foo.FunkyStuff("get funky!");
            _foo.Finish();

            Received.InOrder(() =>
                             {
                                 _foo.Start();
                                 _foo.Finish();
                             });
        }



        [SetUp]
        public void SetUp()
        {
            _foo = Substitute.For<IFoo>();
            _bar = Substitute.For<IBar>();
        }
    }

    public static class Received
    {
        public static void InOrder(Action action)
        {
        }
    }

    public class CallSequenceNotFound : Exception
    {
    }

    public interface IFoo
    {
        void Start();
        void Start(int i);
        void Finish();
        void FunkyStuff(string s);
    }

    public interface IBar
    {
        void Begin();
        void End();
    }
}