using System.Xml.Linq;

namespace XMLBasic.Entities
{
    public interface IElementParser
    {
        string ElementName { get; }
        ICatalogEntity ParseElement(XElement element);
    }
}
