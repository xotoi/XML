using System.Collections;
using System.Collections.Generic;
using XMLBasic.Entities.Catalog;

namespace Tests.Comparators
{
    internal class PatentComparer : IComparer, IComparer<Patent>
    {
        public int Compare(object x, object y)
        {
            return Compare(x as Patent, y as Patent);
        }

        public int Compare(Patent x, Patent y)
        {
            return x.Name == y.Name &&
                   x.Country == y.Country &&
                   x.RegistrationNumber == y.RegistrationNumber &&
                   x.FilingDate == y.FilingDate &&
                   x.PublishDate == y.PublishDate &&
                   x.PagesCount == y.PagesCount &&
                   x.Note == y.Note ? 0 : 1;
        }
    }
}
