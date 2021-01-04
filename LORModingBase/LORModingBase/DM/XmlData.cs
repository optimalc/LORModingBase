using LORModingBase.CustomExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace LORModingBase.DM
{
    /// <summary>
    /// Xml data management class
    /// </summary>
    class XmlData
    {
        /// <summary>
        /// Current loaded xml file path
        /// </summary>
        public List<string> currentXmlFilePaths = new List<string>();
        /// <summary>
        /// Root xml data node
        /// </summary>
        public XmlDataNode rootDataNode = new XmlDataNode();



        /// <summary>
        /// Load xmlData from xml file or directory
        /// </summary>
        /// <param name="xmlPath">xml file path to load</param>
        public XmlData(string xmlPath)
        {
            if(xmlPath.Contains(".txt") || xmlPath.Contains(".xml"))
            {
                this.currentXmlFilePaths.Add(xmlPath);

                XmlDocument XML_DOC = new XmlDocument();
                XML_DOC.Load(xmlPath);
                rootDataNode = LoadNodeData(XML_DOC.DocumentElement);
            }
            else
                LoadFromXmlFilePaths(Directory.GetFiles(xmlPath).ToList());
        }

        /// <summary>
        /// Load xmlData from multiple xml file
        /// </summary>
        /// <param name="xmlFilePaths">Multiple xml file path list</param>
        public XmlData(List<string> xmlFilePaths)
        {
            LoadFromXmlFilePaths(xmlFilePaths);
        }

        /// <summary>
        /// Load xmlData from multiple xml file
        /// </summary>
        /// <param name="xmlFilePaths">Multiple xml file path list</param>
        private void LoadFromXmlFilePaths(List<string> xmlFilePaths)
        {
            this.currentXmlFilePaths = xmlFilePaths;
            if (xmlFilePaths.Count > 0)
            {
                XmlDocument XML_DOC = new XmlDocument();
                XML_DOC.Load(xmlFilePaths[0]);
                rootDataNode = LoadNodeData(XML_DOC.DocumentElement);

                xmlFilePaths.Skip(1).ForEachSafe((string xmlPath) =>
                {
                    XmlDocument XML_DOC_SUB = new XmlDocument();
                    XML_DOC_SUB.Load(xmlPath);
                    rootDataNode.subNodes.AddRange(LoadNodeData(XML_DOC_SUB.DocumentElement).subNodes);
                });
            }
        }


        /// <summary>
        /// Load xmlData from other XmlData base
        /// </summary>
        /// <param name="xmlFilePath">xml file path to load</param>
        public XmlData(XmlData xmlDataBase)
        {
            this.currentXmlFilePaths = xmlDataBase.currentXmlFilePaths;
            rootDataNode.name = xmlDataBase.rootDataNode.name;
        }



        /// <summary>
        /// Load xml datas
        /// </summary>
        private XmlDataNode LoadNodeData(XmlNode nodeToLoad)
        {
            XmlDataNode loadedXmlDataNode = new XmlDataNode();
            loadedXmlDataNode.name = nodeToLoad.Name;

            if(nodeToLoad.Attributes != null)
                foreach (XmlAttribute attribute in nodeToLoad.Attributes)
                    loadedXmlDataNode.attribute[attribute.Name] = attribute.Value;

            if(nodeToLoad.ChildNodes != null)
                foreach(XmlNode childNode in nodeToLoad.ChildNodes)
                {
                    if(childNode.Name == "#text")
                        loadedXmlDataNode.innerText = nodeToLoad.InnerText;
                    else if(!childNode.Name.Contains("#"))
                        loadedXmlDataNode.subNodes.Add(LoadNodeData(childNode));
                }

            return loadedXmlDataNode;
        }
    

        /// <summary>
        /// Save xml datas
        /// </summary>
        /// <param name="dataFilePath">path to save data</param>
        public void SaveNodeData(string dataFilePath)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(dataFilePath, xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(rootDataNode.name);
                writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");

                rootDataNode.subNodes.ForEachSafe((XmlDataNode xmlDataNode) =>
                {
                    MakeEachNodeData(writer, xmlDataNode);
                });

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }

        /// <summary>
        /// Make each node data from writer
        /// </summary>
        /// <param name="writer">Writer to use</param>
        /// <param name="xmlDataNodeToUse">XmlDataNode that converted to string by writer</param>
        private void MakeEachNodeData(XmlWriter writer, XmlDataNode xmlDataNodeToUse)
        {
            writer.WriteStartElement(xmlDataNodeToUse.name);
            if(!string.IsNullOrEmpty(xmlDataNodeToUse.innerText))
                writer.WriteString(xmlDataNodeToUse.innerText);

            foreach (KeyValuePair<string, string> attributeKeyPair in xmlDataNodeToUse.attribute)
                writer.WriteAttributeString(attributeKeyPair.Key, attributeKeyPair.Value);

            xmlDataNodeToUse.subNodes.ForEachSafe((XmlDataNode xmlDataNode) =>
            {
                MakeEachNodeData(writer, xmlDataNode);
            });

            writer.WriteEndElement();
        }
    }

    /// <summary>
    /// Each xml node in XmlData
    /// </summary>
    public class XmlDataNode
    {
        /// <summary>
        /// Node name
        /// </summary>
        public string name = "";
        /// <summary>
        /// Inner text of Xml Data Node
        /// </summary>
        public string innerText = "";

        /// <summary>
        /// Attributes in node
        /// </summary>
        public Dictionary<string, string> attribute = new Dictionary<string, string>();
        /// <summary>
        /// Sub node list
        /// </summary>
        public List<XmlDataNode> subNodes = new List<XmlDataNode>();


        /// <summary>
        /// Action single xml data node if not null
        /// </summary>
        /// <param name="nameToSearch">Name to search</param>
        /// <param name="actionXmlDataNode">Action single xml data node if not null</param>
        public void ActionXmlDataNodeByName(string nameToSearch, Action<XmlDataNode> actionXmlDataNode)
        {
            XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
            {
                return xmlDataNode.name == nameToSearch;
            });
            if (foundXmlDataNode != null)
                actionXmlDataNode(foundXmlDataNode);
        }

        /// <summary>
        /// Action multiple xml data node
        /// </summary>
        /// <param name="nameToSearch">Name to search</param>
        /// <param name="actionXmlDataNode">Action multiple xml data</param>
        public void ActionXmlDataNodesByName(string nameToSearch, Action<XmlDataNode> actionXmlDataNode)
        {
            subNodes.FindAll((XmlDataNode xmlDataNode) =>
            {
                return xmlDataNode.name == nameToSearch;
            }).ForEachSafe(actionXmlDataNode);
        }

        /// <summary>
        /// Action multiple xml data node if not null
        /// </summary>
        /// <param name="nodeName">Name to search</param>
        /// <param name="attributeName">Attribute name to search</param>
        /// <param name="attributeValue">Attribute value to search</param>
        /// <param name="actionXmlDataNode">Action single xml data node if not null</param>
        public void ActionXmlDataNodesByAttribute(string nodeName, string attributeName, string attributeValue, Action<XmlDataNode> actionXmlDataNode)
        {
            ActionXmlDataNodesByName(nodeName, (XmlDataNode searchedNode) =>
            {
                if (searchedNode.attribute.ContainsKey(attributeName)
                  && searchedNode.attribute[attributeName] == attributeValue)
                    actionXmlDataNode(searchedNode);
            });
        }


        /// <summary>
        /// Get inner text by searched name node if not null or empty
        /// </summary>
        /// <param name="nameToSearch">Xml node data name to search</param>
        /// <param name="defaultText">Default text if given node is null or empty</param>
        /// <returns>Searched text</returns>
        public string GetInnerTextByName(string nameToSearch, string defaultText = "")
        {
            XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
            {
                return xmlDataNode.name == nameToSearch;
            });
            if (foundXmlDataNode == null || string.IsNullOrEmpty(foundXmlDataNode.innerText))
                return defaultText;
            else
                return foundXmlDataNode.innerText;
        }

        /// <summary>
        /// Get inner text by searched attribute node if not null or empty
        /// </summary>
        /// <param name="nodeName">Name to search</param>
        /// <param name="attributeName">Attribute name to search</param>
        /// <param name="attributeValue">Attribute value to search</param>
        /// <param name="defaultText">Default text if given node is null or empty</param>
        /// <returns></returns>
        public string GetInnerTextByAttribute(string nodeName, string attributeName, string attributeValue, string defaultText = "")
        {
            XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
            {
                return xmlDataNode.name == nodeName
                    && xmlDataNode.attribute.ContainsKey(attributeName)
                    && xmlDataNode.attribute[attributeName] == attributeValue;
            });
            if (foundXmlDataNode == null || string.IsNullOrEmpty(foundXmlDataNode.innerText))
                return defaultText;
            else
                return foundXmlDataNode.innerText;
        }


        /// <summary>
        /// Get innner text value safely
        /// </summary>
        /// <param name="defaultText">Default value if string is empty</param>
        /// <returns>Innertext</returns>
        public string GetInnerTextSafe(string defaultText = "")
        {
            return string.IsNullOrEmpty(innerText) ? defaultText : innerText;
        }
    }
}
