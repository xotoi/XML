using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Entities.Catalog;

namespace XMLBasic.Writers
{
    public class BookWriter : BaseXmlEntityWriter
    {
        public override Type TypeToWrite => typeof(Book);

        public override void WriteEntity(XmlWriter xmlWriter, ICatalogEntity entity)
        {
            if (!(entity is Book book))
            {
                throw new ArgumentException($"provided {nameof(entity)} is null or not of type {nameof(Book)}");
            }

            var element = new XElement("book");
            AddAttribute(element, "name", book.Name);
            AddAttribute(element, "publicationCity", book.PublicationCity);
            AddAttribute(element, "publisherName", book.PublisherName);
            AddAttribute(element, "publishYear", book.PublishYear.ToString());
            AddAttribute(element, "pagesCount", book.PagesCount.ToString());
            AddAttribute(element, "isbn", book.IsbnNumber);
            AddElement(element, "note", book.Note);
            AddElement(element, "authors",
                book.Authors?.Select(a => new XElement("author",
                    new XAttribute("name", a.Name),
                    new XAttribute("surname", a.Surname)
                ))
            );

            element.WriteTo(xmlWriter);
        }
    }
}
