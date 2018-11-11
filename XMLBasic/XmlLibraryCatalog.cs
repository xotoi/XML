using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using XMLBasic.Entities;

namespace XMLBasic
{
    public class XmlLibraryCatalog
    {
        private static readonly string _catalogElementName = "catalog";

        private readonly IDictionary<string, IElementParser> _elementParsers;
        private readonly IDictionary<Type, IEntityWriter> _entityWriters;

        public XmlLibraryCatalog()
        {
            _elementParsers = new Dictionary<string, IElementParser>();
            _entityWriters = new Dictionary<Type, IEntityWriter>();
        }

        public void AddParsers(params IElementParser[] elementParsers)
        {
            foreach (var parser in elementParsers)
            {
                _elementParsers.Add(parser.ElementName, parser);
            }
        }

        public void AddWriters(params IEntityWriter[] writers)
        {
            foreach (var writer in writers)
            {
                _entityWriters.Add(writer.TypeToWrite, writer);
            }
        }

        public IEnumerable<ICatalogEntity> ReadFrom(TextReader input)
        {
            using (var xmlReader = XmlReader.Create(input, new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true
            }))
            {
                xmlReader.ReadToFollowing(_catalogElementName);
                xmlReader.ReadStartElement();

                do
                {
                    while (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        var node = XNode.ReadFrom(xmlReader) as XElement;
                        if (_elementParsers.TryGetValue(node.Name.LocalName, out var parser))
                        {
                            yield return parser.ParseElement(node);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Founded unknown element tag: {node.Name.LocalName}");
                        }
                    }
                } while (xmlReader.Read());
            }
        }

        public void WriteTo(TextWriter output, IEnumerable<ICatalogEntity> catalogEntities)
        {
            using (var xmlWriter = XmlWriter.Create(output, new XmlWriterSettings()))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(_catalogElementName);
                foreach (var catalogEntity in catalogEntities)
                {
                    if (_entityWriters.TryGetValue(catalogEntity.GetType(), out var writer))
                    {
                        writer.WriteEntity(xmlWriter, catalogEntity);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot find entity writer for type {catalogEntity.GetType().FullName}");
                    }
                }
                xmlWriter.WriteEndElement();
            }
        }
    }
}
