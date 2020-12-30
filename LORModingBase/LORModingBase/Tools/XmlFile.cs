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

        /// <summary>
        /// Add new node to rootNode with innerText
        /// </summary>
        /// <param name="rootNode">Root node to use</param>
        /// <param name="nodeName">Node name will be used</param>
        /// <param name="innerText">Inner text content</param>
        /// <returns>Created node</returns>
        public static XmlNode AddNewNodeWithInnerText(XmlNode rootNode, string nodeName, string innerText)
        {
            XmlElement createdElement = rootNode.OwnerDocument.CreateElement(nodeName);
            createdElement.InnerText = innerText;
            rootNode.AppendChild(createdElement);
            return createdElement;
        }
    }
}
