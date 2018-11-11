using System;
using System.Xml;

namespace XMLBasic.Entities
{
    public interface IEntityWriter
    {
        Type TypeToWrite { get; }

        void WriteEntity(XmlWriter xmlWriter, ICatalogEntity entity);
    }
}
