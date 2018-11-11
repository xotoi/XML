using System.Collections;
using System.Collections.Generic;
using XMLBasic.Entities.Catalog;

namespace Tests.Comparators
{
    internal class BooksComparer : IComparer, IComparer<Book>
    {
        public int Compare(Book x, Book y)
        {
            return x.PagesCount == y.PagesCount
                   && x.Name == y.Name
                   && x.IsbnNumber == y.IsbnNumber
                   && x.Note == y.Note
                   && x.PagesCount == y.PagesCount
                   && x.PublishYear == y.PublishYear
                   && x.PublicationCity == y.PublicationCity
                   && x.PublisherName == y.PublisherName ? 0 : 1;
        }

        public int Compare(object x, object y)
        {
            return Compare(x as Book, y as Book);
        }
    }
}
