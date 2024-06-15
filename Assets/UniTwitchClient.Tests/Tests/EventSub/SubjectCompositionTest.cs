using Codice.CM.Common;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniTwitchClient.EventSub;
using UnityEngine;

namespace UniTwitchClient.Tests.EventSub
{
    public class SubjectCompositionTest
    {
        private SubjectComposition _subjectComposition;
        private CompositeDisposable _compositeDisposable;

        [SetUp]
        public void SetUp() 
        {
            _subjectComposition = new SubjectComposition();
            _compositeDisposable = new CompositeDisposable();
        }

        [TearDown]
        public void TearDown() 
        {
            _subjectComposition?.Dispose();
            _subjectComposition = null;
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        [Test]
        public void OnNextTest() 
        {
            object result = null;

            _subjectComposition.CreateSubject<TestDTOOne>(TestEnum.TestOne.ToString()).Subscribe(x => result = x).AddTo(_compositeDisposable);
            _subjectComposition.CreateSubject<TestDTOTwo>(TestEnum.TestTwo.ToString()).Subscribe(x => result = x).AddTo(_compositeDisposable);
            _subjectComposition.CreateSubject<TestDTOThree>(TestEnum.TestThree.ToString()).Subscribe(x => result = x).AddTo(_compositeDisposable);

            TestDTOOne sourceOne = new TestDTOOne() { Name = "TestDTO", Value = "TestDTOValue" };
            _subjectComposition.OnNext(TestEnum.TestOne.ToString(), sourceOne);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, sourceOne);
            Assert.AreEqual(((TestDTOOne)result).Name, sourceOne.Name);
            Assert.AreEqual(((TestDTOOne)result).Value, sourceOne.Value);

            TestDTOTwo sourceTwo = new TestDTOTwo() { Name = "TestDTO", Value = "TestDTOValue" };
            _subjectComposition.OnNext(TestEnum.TestTwo.ToString(), sourceTwo);
            Assert.AreEqual(result, sourceTwo);
            Assert.AreEqual(((TestDTOTwo)result).Name, sourceTwo.Name);
            Assert.AreEqual(((TestDTOTwo)result).Value, sourceTwo.Value);

            TestDTOThree sourceThree = new TestDTOThree() { Name = "TestDTO", Value = "TestDTOValue" };
            _subjectComposition.OnNext(TestEnum.TestThree.ToString(), sourceThree);
            Assert.AreEqual(result, sourceThree);
            Assert.AreEqual(((TestDTOThree)result).Name, sourceThree.Name);
            Assert.AreEqual(((TestDTOThree)result).Value, sourceThree.Value);
        }

        private enum TestEnum 
        {
            TestOne,
            TestTwo,
            TestThree,
        }

        private class TestDTOOne 
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private class TestDTOTwo
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private class TestDTOThree
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}