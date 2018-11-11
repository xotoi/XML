using System.Collections;
using System.Collections.Generic;
using XMLBasic.Entities.Catalog;

namespace Tests.Comparators
{
    internal class NewsPaperComparer : IComparer, IComparer<NewsPaper>
    {
        public int Compare(object x, object y)
        {
            return Compare(x as NewsPaper, y as NewsPaper);
        }

        public int Compare(NewsPaper x, NewsPaper y)
        {
            return x.Name == y.Name &&
                   x.PublicationCity == y.PublicationCity &&
                   x.PublisherName == y.PublisherName &&
                   x.PublishYear == y.PublishYear &&
                   x.PagesCount == y.PagesCount &&
                   x.Note == y.Note &&
                   x.Number == y.Number &&
                   x.Date == y.Date &&
                   x.IssnNumber == y.IssnNumber ? 0 : 1;
        }
    }
}
