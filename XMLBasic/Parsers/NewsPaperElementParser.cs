using System;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Entities.Catalog;

namespace XMLBasic.Parsers
{
    public class NewsPaperElementParser : BaseXmlElementParser
    {
        public override string ElementName => "newspaper";

        public override ICatalogEntity ParseElement(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException($"{nameof(element)} is null");
            }

            return new NewsPaper
            {
                Name = GetAttributeValue(element, "name", true),
                PublicationCity = GetAttributeValue(element, "publicationCity"),
                PagesCount = int.Parse(GetAttributeValue(element, "pagesCount") ?? default(int).ToString()),
                Note = GetElement(element, "note")?.Value,
                PublisherName = GetAttributeValue(element, "publisherName"),
                PublishYear = int.Parse(GetAttributeValue(element, "publishYear") ?? default(int).ToString()),
                IssnNumber = GetAttributeValue(element, "issn", true),
                Number = int.Parse(GetAttributeValue(element, "number") ?? default(int).ToString()),
                Date = GetDate(GetAttributeValue(element, "date")),
            };
        }
    }
}
