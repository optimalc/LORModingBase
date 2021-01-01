using System;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// 해당 노드에 값이 정말로 존재하는지 확인합니다.
        /// </summary>
        public static bool IsValueExists(XmlNode nodeXml, string node)
        {
            if (nodeXml == null) return false;
            if (nodeXml[node] != null)
                return !string.IsNullOrEmpty(nodeXml[node].InnerText);
            else
                return false;
        }

        /// <summary>
        /// Xml 노드로 부터 안전하게 값을 얻어오기 위해서 사용된다.
        /// </summary>
        public class GetXmlNodeSafe
        {
            /// <summary>
            /// String 형으로 얻어온다.
            /// </summary>
            public static string ToString(XmlNode xmlNode, string XPath, string defaultValue = "")
            {
                if (IsValueExists(xmlNode, XPath))
                    return xmlNode[XPath].InnerText;
                else
                    return defaultValue;
            }

            /// <summary>
            /// Double 형으로 얻어온다. 숫자형식이 아니면 1.0을 반환한다.
            /// </summary>
            public static double ToDouble(XmlNode xmlNode, string XPath, double defaultValue = 0)
            {
                string gettedText = ToString(xmlNode, XPath, defaultValue.ToString());
                if (Regex.IsMatch(gettedText, "^[0-9.]+$"))
                    return Convert.ToDouble(gettedText);
                else
                    return defaultValue;
            }

            /// <summary>
            /// Int 형으로 얻어온다. 숫자형식이 아니면 1을 반환한다.
            /// </summary>
            public static int ToInt(XmlNode xmlNode, string XPath, int defaultValue = 0)
            {
                string gettedText = ToString(xmlNode, XPath, defaultValue.ToString());
                if (Regex.IsMatch(gettedText, "^[0-9]+$"))
                    return Convert.ToInt32(gettedText);
                else
                    return defaultValue;
            }
        }
    }
}
