using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tests.Comparators;
using XMLBasic;
using XMLBasic.Entities;
using XMLBasic.Entities.Catalog;
using XMLBasic.Parsers;
using XMLBasic.Writers;

namespace Tests
{
    [TestFixture]
    public class XmlLibraryCatalogTests
    {
        private XmlLibraryCatalog _catalog;

        [SetUp]
        public void Init()
        {
            _catalog = new XmlLibraryCatalog();
            _catalog.AddParsers(new BookElementParser(), new NewsPaperElementParser(), new PatentElementParser());
            _catalog.AddWriters(new BookWriter(), new NewsPaperWriter(), new PatentWriter());
        }

        [Test]
        public void Test_Books_Read()
        {
            var sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                             "<catalog>" +
                                             GetBookXml() +
                                             "</catalog>");

            IEnumerable<ICatalogEntity> books = _catalog.ReadFrom(sr).ToList();

            CollectionAssert.AreEqual(books, new[] {CreateBook()}, new BooksComparer());
        }

        [Test]
        public void Test_Papers_Read()
        {
            TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                             "<catalog>" +
                                             GetNewspaperXml() +
                                             "</catalog>");

            var newspapers = _catalog.ReadFrom(sr);

            CollectionAssert.AreEqual(newspapers, new[] {CreateNewspaper()}, new NewsPaperComparer());
        }

        [Test]
        public void Test_Patents_Read()
        {
            TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                             "<catalog>" +
                                                GetPatentXml() +
                                             "</catalog>");

            var newspapers = _catalog.ReadFrom(sr);

            CollectionAssert.AreEqual(newspapers, new[] {CreatePatent()}, new PatentComparer());
        }

        [Test]
        public void Test_MixedEntities_Read()
        {
            TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                             "<catalog>" +
                                                GetPatentXml() +
                                                GetBookXml() +
                                                GetNewspaperXml() +
                                             "</catalog>");

            var entities = _catalog.ReadFrom(sr).ToList();

            Assert.IsTrue(new PatentComparer().Compare(entities[0], CreatePatent()) == 0);
            Assert.IsTrue(new BooksComparer().Compare(entities[1], CreateBook()) == 0);
            Assert.IsTrue(new NewsPaperComparer().Compare(entities[2], CreateNewspaper()) == 0);
        }

        [Test]
        public void Test_MixedEntities_Write()
        {
            var actualResult = new StringBuilder();
            var sw = new StringWriter(actualResult);
            var book = CreateBook();
            var newspaper = CreateNewspaper();
            var patent = CreatePatent();
            var entities = new ICatalogEntity[]
            {
                book,
                newspaper,
                patent
            };
            var expectedResult = @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                "<catalog>" +
                    GetBookXml() +
                    GetNewspaperXml() +
                    GetPatentXml() +
                "</catalog>";

            _catalog.WriteTo(sw, entities);

            Assert.AreEqual(expectedResult, actualResult.ToString());
        }

        private static NewsPaper CreateNewspaper()
        {
            return new NewsPaper
            {
                Name = "Times",
                PublicationCity = "London",
                PublisherName = "London typography",
                PublishYear = 1904,
                PagesCount = 14,
                Date = new DateTime(1905, 5, 16),
                IssnNumber = "0317-8471",
                Number = 14,
                Note = "The most popular London newspaper"
            };
        }

        private static Patent CreatePatent()
        {
            return new Patent
            {
                Name = "Airplane",
                Country = "USA",
                RegistrationNumber = "D0000126",
                FilingDate = new DateTime(1905, 12, 24),
                PublishDate = new DateTime(1906, 1, 20),
                PagesCount = 100,
                Note = "First airplane in the world",
                Creators = new List<Creator>
                {
                    new Creator {Name = "Orville", Surname = "Wright"},
                    new Creator {Name = "Wilbur", Surname = "Wright"}
                }
            };
        }

        private static Book CreateBook()
        {
            return new Book
            {
                Name = "A song of Ice and Fire",
                PublicationCity = "New-York",
                PublisherName = "Typography",
                PublishYear = 1999,
                PagesCount = 500,
                IsbnNumber = "978-1-56619-909-4",
                Note = "This book is about history of Seven Kingdom.",
                Authors = new List<Author>
                {
                    new Author {Name = "George", Surname = "Martin"}
                }
            };
        }

        private static string GetBookXml()
        {
            return @"<book name=""A song of Ice and Fire"" " +
                       @"publicationCity=""New-York"" " +
                       @"publisherName=""Typography"" " +
                       @"publishYear=""1999"" " +
                       @"pagesCount=""500"" " +
                       @"isbn=""978-1-56619-909-4"">" +
                       "<note>This book is about history of Seven Kingdom.</note>" +
                       "<authors>" +
                            @"<author name=""George"" surname=""Martin"" />" +
                       "</authors>" +
                   "</book>";
        }

        private static string GetNewspaperXml()
        {
            return "<newspaper name=\"Times\" " +
                       "publicationCity=\"London\" " +
                       "publisherName=\"London typography\" " +
                       "publishYear=\"1904\" " +
                       "pagesCount=\"14\" " +
                       "date=\"05/16/1905\" " +
                       "issn=\"0317-8471\" " +
                       "number=\"14\">" +
                       "<note>The most popular London newspaper</note>" +
                   "</newspaper>";
        }

        private static string GetPatentXml()
        {
            return "<patent name=\"Airplane\" " +
                       "country=\"USA\" " +
                       "registrationNumber=\"D0000126\" " +
                       "filingDate=\"12/24/1905\" " +
                       "publishDate=\"01/20/1906\" " +
                       "pagesCount=\"100\">" +
                       "<note>First airplane in the world</note>" +
                       "<creators>" +
                            "<creator name=\"Orville\" surname=\"Wright\" />" +
                            "<creator name=\"Wilbur\" surname=\"Wright\" />" +
                       "</creators>" +
                   "</patent>";
        }
    }
}
