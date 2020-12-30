using System.Xml;
using System.Xml.Linq;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Tools for xml file
    /// </summary>
    class XmlFile
    {
        /// <summary>
        /// Get xml node lists from given xml file path using XPath query
        /// </summary>
        /// <param name="xmlFilePath">XML file path</param>
        /// <param name="xPath">XPath query</param>
        /// <returns></returns>
        public static XmlNodeList SelectNodeLists(string xmlFilePath, string xPath)
        {
            XmlDocument XML_DOC = new XmlDocument();
            XML_DOC.Load(xmlFilePath);
            XmlNode root = XML_DOC.DocumentElement;

            return root.SelectNodes(xPath);
        }

        /// <summary>
        /// Get xml node lists from given xml file path using XPath query
        /// </summary>
        /// <param name="xmlFilePath">XML file path</param>
        /// <param name="xPath">XPath query</param>
        /// <returns></returns>
        public static XmlNode SelectSingleNode(string xmlFilePath, string xPath)
        {
            XmlDocument XML_DOC = new XmlDocument();
            XML_DOC.Load(xmlFilePath);
            XmlNode root = XML_DOC.DocumentElement;

            return root.SelectSingleNode(xPath);
        }
    }
}
