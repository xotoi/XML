﻿using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using XMLBasic.Entities;
using XMLBasic.Entities.Catalog;

namespace XMLBasic.Writers
{
    public class PatentWriter : BaseXmlEntityWriter
    {
        public override Type TypeToWrite => typeof(Patent);

        public override void WriteEntity(XmlWriter xmlWriter, ICatalogEntity entity)
        {
            if (!(entity is Patent patent))
            {
                throw new ArgumentException($"provided {nameof(entity)} is null or not of type {nameof(Patent)}");
            }

            var element = new XElement("patent");
            AddAttribute(element, "name", patent.Name);
            AddAttribute(element, "country", patent.Country);
            AddAttribute(element, "registrationNumber", patent.RegistrationNumber);
            AddAttribute(element, "filingDate", GetInvariantShortDateString(patent.FilingDate.Date));
            AddAttribute(element, "publishDate", GetInvariantShortDateString(patent.PublishDate.Date));
            AddAttribute(element, "pagesCount", patent.PagesCount.ToString());
            AddElement(element, "note", patent.Note);
            AddElement(element, "creators", patent.Creators.Select(
                c => new XElement("creator",
                    new XAttribute("name", c.Name),
                    new XAttribute("surname", c.Surname))));
            element.WriteTo(xmlWriter);
        }
    }
}
