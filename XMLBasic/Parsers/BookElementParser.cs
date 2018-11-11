﻿using System;
using System.Linq;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Entities.Catalog;

namespace XMLBasic.Parsers
{
    public class BookElementParser : BaseXmlElementParser
    {
        public override string ElementName => "book";

        public override ICatalogEntity ParseElement(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException($"{nameof(element)} is null");
            }

            return new Book
            {
                Name = GetAttributeValue(element, "name"),
                PublicationCity = GetAttributeValue(element, "publicationCity"),
                Authors = GetElement(element, "authors").Elements("author").Select(e => new Author
                {
                    Name = GetAttributeValue(e, "name"),
                    Surname = GetAttributeValue(e, "surname")
                }).ToList(),
                PagesCount = int.Parse(GetAttributeValue(element, "pagesCount")),
                Note = GetElement(element, "note").Value,
                PublisherName = GetAttributeValue(element, "publisherName"),
                PublishYear = int.Parse(GetAttributeValue(element, "publishYear")),
                IsbnNumber = GetAttributeValue(element, "isbn")
            };
        }
    }
}
