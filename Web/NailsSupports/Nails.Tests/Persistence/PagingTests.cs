using System.Collections.Generic;
using System.Linq;
using NailsFramework.Persistence;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    [TestFixture]
    public class PagingTests
    {
        private static IQueryable<TestClass> GetItems(int count)
        {
            return GetItems<TestClass>(count);
        }

        private static IQueryable<T> GetItems<T>(int count) where T : new()
        {
            var list = new List<T>();
            for (var i = 0; i < count; i++)
                list.Add(new T());

            return list.AsQueryable();
        }

        public class TestClass
        {
        }

        [PageSize(10)]
        private class TestClassWithOverridenPageSize
        {
        }

        [Test]
        public void CollectionWith3Pages()
        {
            var collection = GetItems(60);
            var page1 = collection.Page(1);
            Assert.AreEqual(25, page1.Count());
            Assert.AreEqual(1,page1.PageNumber);
            var page2 = collection.Page(2);
            Assert.AreEqual(25, page2.Count());
            Assert.AreEqual(2, page2.PageNumber);
            var page3 = collection.Page(3);
            Assert.AreEqual(10, page3.Count());
            Assert.AreEqual(3, page3.PageNumber);
        }

        [Test]
        public void CollectionWithLessElementsThanPageSize()
        {
            var collection = GetItems(23);
            var page1 = collection.Page(1);
            Assert.AreEqual(1, page1.TotalPages);
            Assert.AreEqual(25, page1.PageSize);
            Assert.AreEqual(23, page1.Count());
            var page2 = collection.Page(2);
            Assert.AreEqual(0, page2.Count());
        }

        [Test]
        public void CollectionWithMoreElementsThanPageSize()
        {
            var collection = GetItems(27);
            var page1 = collection.Page(1);
            Assert.AreEqual(25, page1.PageSize);
            Assert.AreEqual(2, page1.TotalPages);
            Assert.AreEqual(25, page1.Count());
            var page2 = collection.Page(2);
            Assert.AreEqual(2, page2.Count());
        }

        [Test]
        public void CollectionWithSameElementsThanPageSize()
        {
            var collection = GetItems(25);
            var page1 = collection.Page(1);
            Assert.AreEqual(25, page1.PageSize);
            Assert.AreEqual(1, page1.TotalPages);
            Assert.AreEqual(25, page1.Count());
            var page2 = collection.Page(2);
            Assert.AreEqual(0, page2.Count());
        }

        [Test]
        public void ElementsWerePaged()
        {
            var list = new List<int>();
            for (var i = 0; i < 100; i++)
                list.Add(i);

            var queryable = list.AsQueryable();

            var page = queryable.Page(1);
            var firstElementInPage = page.First();
            var expected = list.ToList()[0];
            Assert.AreEqual(expected, firstElementInPage);

            var lastElementInPage = page.Last();
            expected = list.ToList()[24];
            Assert.AreEqual(expected, lastElementInPage);

            page = queryable.Page(3);
            firstElementInPage = page.First();
            expected = list.ToList()[50];
            Assert.AreEqual(expected, firstElementInPage);

            lastElementInPage = page.Last();
            expected = list.ToList()[74];
            Assert.AreEqual(expected, lastElementInPage);
        }

        [Test]
        public void EmptyCollection()
        {
            var collection = GetItems(0);
            var page = collection.Page(1);
            Assert.AreEqual(25, page.PageSize);
            Assert.AreEqual(1, page.TotalPages);
            Assert.AreEqual(0, page.Count());
        }

        [Test]
        public void OverridenPageSizeShouldIgnoreAttribute()
        {
            var collection = GetItems<TestClassWithOverridenPageSize>(60);
            var page1 = collection.Page(1, pageSize: 15);
            Assert.AreEqual(4, page1.TotalPages);
            Assert.AreEqual(15, page1.PageSize);
            Assert.AreEqual(15, page1.Count());
        }

        [Test]
        public void PageSizeOverriden()
        {
            var collection = GetItems<TestClass>(60);
            var page1 = collection.Page(1, pageSize: 10);
            Assert.AreEqual(6, page1.TotalPages);
            Assert.AreEqual(10, page1.PageSize);
            Assert.AreEqual(10, page1.Count());
        }

        [Test]
        public void PageSizeOverridenByAttribute()
        {
            var collection = GetItems<TestClassWithOverridenPageSize>(60);
            var page1 = collection.Page(1);
            Assert.AreEqual(6, page1.TotalPages);
            Assert.AreEqual(10, page1.PageSize);
            Assert.AreEqual(10, page1.Count());
        }
    }
}