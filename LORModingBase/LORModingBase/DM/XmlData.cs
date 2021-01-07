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
    [Serializable]
    public class XmlData
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

            List<string> dataFileSplit = dataFilePath.Split('\\').ToList();
            dataFileSplit.RemoveAt(dataFileSplit.Count - 1);
            string DIC_PATH = String.Join("\\", dataFileSplit);
            if (!Directory.Exists(DIC_PATH))
                Directory.CreateDirectory(DIC_PATH);

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

            foreach (KeyValuePair<string, string> attributeKeyPair in xmlDataNodeToUse.attribute)
                writer.WriteAttributeString(attributeKeyPair.Key, attributeKeyPair.Value);

            if (!string.IsNullOrEmpty(xmlDataNodeToUse.innerText))
                writer.WriteString(xmlDataNodeToUse.innerText);

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
    [Serializable]
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


        #region XmlDataNode constructor
        public XmlDataNode() {}

        /// <summary>
        /// Make new xml data node with name
        /// </summary>
        /// <param name="name">Name to set</param>
        public XmlDataNode(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Make new xml data with innerText
        /// </summary>
        /// <param name="name">Name to set</param>
        /// <param name="innerText">Inner text to set</param>
        public XmlDataNode(string name, string innerText)
        {
            this.name = name;
            this.innerText = innerText;
        }

        /// <summary>
        /// Make new xml data node with name, attribute
        /// </summary>
        /// <param name="name">Name to set</param>
        /// <param name="attributeName">Attribute name to set</param>
        /// <param name="attributeValue">Attribute value to set</param>
        public XmlDataNode(string name, string attributeName, string attributeValue)
        {
            this.name = name;
            attribute[attributeName] = attributeValue;
        }

        /// <summary>
        /// Make new data node with name, attribute dictionary
        /// </summary>
        /// <param name="name">Name to set</param>
        /// <param name="attribute">Attribute dictionary to use</param>
        public XmlDataNode(string name, Dictionary<string, string> attribute)
        {
            this.name = name;
            if(attribute != null)
                this.attribute = attribute;
        }

        /// <summary>
        /// Make new data node with name, attribute dictionary
        /// </summary>
        /// <param name="name">Name to set</param>
        /// <param name="innerText">InnerText to set</param>
        /// <param name="attribute">Attribute dictionary to use</param>
        public XmlDataNode(string name, string innerText, Dictionary<string, string> attribute)
        {
            this.name = name;
            this.innerText = innerText;
            if (attribute != null)
                this.attribute = attribute;
        }
        #endregion

        #region Search nodes functions
        /// <summary>
        /// Action multiple xml data node
        /// </summary>
        /// <param name="nameToSearch">Name to search</param>
        /// <param name="actionXmlDataNode">Action multiple xml data</param>
        public void ActionXmlDataNodesByPath(string path, Action<XmlDataNode> actionXmlDataNode)
        {
            if (string.IsNullOrEmpty(path)) return;

            List<string> NAME_LIST = path.Split('/').ToList();
            if (NAME_LIST.Count > 1)
            {
                subNodes.FindAll((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                }).ForEachSafe((XmlDataNode xmlDataNode) => {
                    xmlDataNode.ActionXmlDataNodesByPath(String.Join("/", NAME_LIST.Skip(1)), actionXmlDataNode);
                });
            }
            else
            {
                subNodes.FindAll((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                }).ForEachSafe((XmlDataNode xmlDataNode) => {
                    actionXmlDataNode(xmlDataNode);
                });
            }
        }

        /// <summary>
        /// Action multiple xml data node if not null
        /// </summary>
        /// <param name="nodeName">Name to search</param>
        /// <param name="attributeName">Attribute name to search</param>
        /// <param name="attributeValue">Attribute value to search</param>
        /// <param name="actionXmlDataNode">Action single xml data node if not null</param>
        public void ActionXmlDataNodesByAttributeWithPath(string path, string attributeName, string attributeValue, Action<XmlDataNode> actionXmlDataNode)
        {
            ActionXmlDataNodesByPath(path, (XmlDataNode searchedNode) =>
            {
                if (searchedNode.attribute.ContainsKey(attributeName)
                  && searchedNode.attribute[attributeName] == attributeValue)
                    actionXmlDataNode(searchedNode);
            });
        }

        /// <summary>
        /// Get multiple xml data node
        /// </summary>
        /// <param name="path">XmlDataNode path to search</param>
        /// <param name="foundXmlDataNodes">Rollback para</param>
        /// <returns>Searched xml data nodes</returns>
        public List<XmlDataNode> GetXmlDataNodesByPath(string path, List<XmlDataNode> foundXmlDataNodes = null)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if(foundXmlDataNodes == null)
                foundXmlDataNodes = new List<XmlDataNode>();

            List<string> NAME_LIST = path.Split('/').ToList();
            if (NAME_LIST.Count > 1)
            {
                subNodes.FindAll((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                }).ForEachSafe((XmlDataNode xmlDataNode) => {
                    xmlDataNode.GetXmlDataNodesByPath(String.Join("/", NAME_LIST.Skip(1)), foundXmlDataNodes);
                });
            }
            else
                foundXmlDataNodes.AddRange(subNodes.FindAll((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                }));

            return foundXmlDataNodes;
        }

        /// <summary>
        /// Get multiple xml data node
        /// </summary>
        /// <param name="path">XmlDataNode path to search</param>
        /// <param name="innerTextToCheck">Inner text to check</param>
        /// <param name="attributeToCheck">Attribute dic to check</param>
        /// <param name="foundXmlDataNodes">Rollback para</param>
        /// <returns>Searched xml data nodes</returns>
        public List<XmlDataNode> GetXmlDataNodesByPathWithXmlInfo(string path, string innerTextToCheck = "", 
            Dictionary<string, string> attributeToCheck = null, List<XmlDataNode> foundXmlDataNodes = null)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (foundXmlDataNodes == null)
                foundXmlDataNodes = new List<XmlDataNode>();

            List<string> NAME_LIST = path.Split('/').ToList();
            if (NAME_LIST.Count > 1)
            {
                subNodes.FindAll((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                }).ForEachSafe((XmlDataNode xmlDataNode) => {
                    xmlDataNode.GetXmlDataNodesByPathWithXmlInfo(String.Join("/", NAME_LIST.Skip(1)),
                        innerTextToCheck, attributeToCheck, foundXmlDataNodes);
                });
            }
            else
                foundXmlDataNodes.AddRange(subNodes.FindAll((XmlDataNode xmlDataNode) =>
                {
                    if (!string.IsNullOrEmpty(innerTextToCheck) && attributeToCheck == null)
                        return xmlDataNode.innerText == innerTextToCheck;
                    else if ((!string.IsNullOrEmpty(innerTextToCheck) && attributeToCheck != null)
                          && (xmlDataNode.innerText == innerTextToCheck))
                    {
                        bool attritubteCheck = true;
                        attributeToCheck.ForEachKeyValuePairSafe((string atName, string atValue) =>
                        {
                            if (xmlDataNode.attribute.ContainsKey(atName)
                               && xmlDataNode.attribute[atName] != atValue)
                                attritubteCheck = false;
                            else if (!xmlDataNode.attribute.ContainsKey(atName))
                                attritubteCheck = false;
                        });
                        return attritubteCheck;
                    }
                    else if (string.IsNullOrEmpty(innerTextToCheck) && attributeToCheck != null)
                    {
                        bool attritubteCheck = true;
                        attributeToCheck.ForEachKeyValuePairSafe((string atName, string atValue) =>
                        {
                            if (xmlDataNode.attribute.ContainsKey(atName)
                               && xmlDataNode.attribute[atName] != atValue)
                                attritubteCheck = false;
                            else if (!xmlDataNode.attribute.ContainsKey(atName))
                                attritubteCheck = false;
                        });
                        return attritubteCheck;
                    }
                    else
                        return true;
                }));

            return foundXmlDataNodes;
        }

        /// <summary>
        /// Returns whether given path with attriubte is exists
        /// </summary>
        /// <param name="path">XmlDataNode path to search</param>
        /// <param name="innerTextToCheck">Inner text to check</param>
        /// <param name="attributeToCheck">Attribute dic to check</param>
        /// <returns>If exists, return true. Else, return false</returns>
        public bool CheckIfGivenPathWithXmlInfoExists(string path, string innerTextToCheck = "",
            Dictionary<string, string> attributeToCheck = null)
        {
            return GetXmlDataNodesByPathWithXmlInfo(path, innerTextToCheck, attributeToCheck).Count > 0;
        }
        #endregion

        #region Getting data functions
        /// <summary>
        /// Get inner text by searched name node if not null or empty
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="defaultText">Default text if given node is null or empty</param>
        /// <returns>Searched text</returns>
        public string GetInnerTextByPath(string path, string defaultText = "")
        {
            if (string.IsNullOrEmpty(path)) return null;

            List<string> NAME_LIST = path.Split('/').ToList();
            if (NAME_LIST.Count > 1)
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                });
                if (foundXmlDataNode != null)
                    return foundXmlDataNode.GetInnerTextByPath(String.Join("/", NAME_LIST.Skip(1)), defaultText);
                else
                    return defaultText;
            }
            else
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                });
                if (foundXmlDataNode != null)
                    return foundXmlDataNode.innerText;
                else
                    return defaultText;
            }
        }

        /// <summary>
        /// Action if given path has inner text
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="actionForInnerText">Action for not empty innertext</param>
        public void ActionIfInnertTextIsNotNullOrEmpty(string path, Action<string> actionForInnerText)
        {
            string SEARCHED_TEXT = GetInnerTextByPath(path);
            if (!string.IsNullOrEmpty(SEARCHED_TEXT))
                actionForInnerText(SEARCHED_TEXT);
        }


        /// <summary>
        /// Get inner text by searched attribute node if not null or empty
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="attributeName">Attribute name to search</param>
        /// <param name="attributeValue">Attribute value to search</param>
        /// <param name="defaultText">Default text if given node is null or empty</param>
        /// <returns></returns>
        public string GetInnerTextByAttributeWithPath(string path, string attributeName, string attributeValue, string defaultText = "")
        {
            if (string.IsNullOrEmpty(path)) return null;

            List<string> NAME_LIST = path.Split('/').ToList();
            if (NAME_LIST.Count > 1)
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                });
                if (foundXmlDataNode != null)
                    return foundXmlDataNode.GetInnerTextByAttributeWithPath(String.Join("/", NAME_LIST.Skip(1)), attributeName, attributeValue, defaultText);
                else
                    return defaultText;
            }
            else
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0]
                      && xmlDataNode.attribute.ContainsKey(attributeName)
                        && xmlDataNode.attribute[attributeName] == attributeValue;
                });
                if (foundXmlDataNode != null)
                    return foundXmlDataNode.innerText;
                else
                    return defaultText;
            }
        }

        /// <summary>
        /// Action if given path has inner text
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="attributeName">Attribute name to search</param>
        /// <param name="attributeValue">Attribute value to search</param>
        /// <param name="actionForInnerText">Action for not empty innertext</param>
        public void ActionIfInnertTextIsNotNullOrEmptyWithAttribute(string path, string attributeName, string attributeValue, Action<string> actionForInnerText)
        {
            string SEARCHED_TEXT = GetInnerTextByAttributeWithPath(path, attributeName, attributeValue);
            if (!string.IsNullOrEmpty(SEARCHED_TEXT))
                actionForInnerText(SEARCHED_TEXT);
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

        /// <summary>
        /// Get attribute text value safely
        /// </summary>
        /// <param name="attributeName">Attribute name to use</param>
        /// <param name="defaultText">Default value if string is empty</param>
        /// <returns>Attribute text</returns>
        public string GetAttributesSafe(string attributeName, string defaultText = "")
        {
            if (!attribute.ContainsKey(attributeName) || string.IsNullOrEmpty(attribute[attributeName]))
                return defaultText;
            else
                return attribute[attributeName];
        }
        #endregion

        #region Setting data functions
        /// <summary>
        /// Set inner text by paths (If not exist, make new XmlData)
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="valueToSet">Value to set</param>
        /// <returns>Edited XmlDataNode</returns>
        public XmlDataNode SetXmlInfoByPath(string path, string valueToSet = "", Dictionary<string, string> attributePairsToSet = null)
        {
            if (string.IsNullOrEmpty(path)) return null;

            List<string> NAME_LIST = path.Split('/').ToList();
            if(NAME_LIST.Count > 1)
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                });
                if (foundXmlDataNode != null)
                    return foundXmlDataNode.SetXmlInfoByPath(String.Join("/", NAME_LIST.Skip(1)), valueToSet, attributePairsToSet);
                else
                {
                    XmlDataNode createdXmlDataNode = new XmlDataNode(NAME_LIST[0]);
                    subNodes.Add(createdXmlDataNode);
                    return createdXmlDataNode.SetXmlInfoByPath(String.Join("/", NAME_LIST.Skip(1)), valueToSet, attributePairsToSet);
                }
            }
            else
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                });
                if (foundXmlDataNode != null)
                {
                    foundXmlDataNode.innerText = valueToSet;
                    attributePairsToSet.ForEachKeyValuePairSafe((string atName, string atValue) => {
                        attribute[atName] = atValue;
                    });
                    MainWindow.mainWindow.UpdateDebugInfo();
                    return foundXmlDataNode;
                }
                else
                {
                    XmlDataNode createdXmlDataNode = new XmlDataNode(NAME_LIST[0], valueToSet, attributePairsToSet);
                    subNodes.Add(createdXmlDataNode);
                    MainWindow.mainWindow.UpdateDebugInfo();
                    return createdXmlDataNode;
                }
            }
        }

        /// <summary>
        /// Set inner text by paths. But, if valueToSet is empty, It remove the given path
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="valueToSet">Value to set</param>
        /// <param name="attributePairsToSet"></param>
        public void SetXmlInfoByPathAndEmptyWillRemove(string path, string valueToSet = "", Dictionary<string, string> attributePairsToSet = null)
        {
            if (string.IsNullOrEmpty(valueToSet))
                RemoveXmlInfosByPath(path, deleteOnce: true);
            else
                SetXmlInfoByPath(path, valueToSet, attributePairsToSet);
        }

        /// <summary>
        /// Add new XmlInfoData
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="valueToSet">Value to set</param>
        /// <returns>Created XmlDataNode</returns>
        public XmlDataNode AddXmlInfoByPath(string path, string valueToSet = "", Dictionary<string, string> attributePairsToSet = null)
        {
            if (string.IsNullOrEmpty(path)) return null;

            List<string> NAME_LIST = path.Split('/').ToList();
            if (NAME_LIST.Count > 1)
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                });
                if (foundXmlDataNode != null)
                    return foundXmlDataNode.AddXmlInfoByPath(String.Join("/", NAME_LIST.Skip(1)), valueToSet, attributePairsToSet);
                else
                {
                    XmlDataNode createdXmlDataNode = new XmlDataNode(NAME_LIST[0]);
                    subNodes.Add(createdXmlDataNode);
                    return createdXmlDataNode.AddXmlInfoByPath(String.Join("/", NAME_LIST.Skip(1)), valueToSet, attributePairsToSet);
                }
            }
            else
            {
                XmlDataNode createdXmlDataNode = new XmlDataNode(NAME_LIST[0], valueToSet, attributePairsToSet);
                subNodes.Add(createdXmlDataNode);
                MainWindow.mainWindow.UpdateDebugInfo();
                return createdXmlDataNode;
            }
        }
       
        /// <summary>
        /// Make empty node if not exists
        /// </summary>
        /// <param name="path">Node to create</param>
        /// <returns>Created empty node</returns>
        public XmlDataNode MakeEmptyNodeGivenPathIfNotExist(string path)
        {
            return SetXmlInfoByPath(path);
        }
        #endregion

        #region Removing data functions
        /// <summary>
        /// Delete xml data node from path if exists
        /// </summary>
        /// <param name="path">XmlData path</param>
        /// <param name="innerTextToCheck">Inner text to check</param>
        /// <param name="attributeToCheck">Attribute dic to check</param>
        /// <param name="deleteOnce">If it true, Delete only first found XmlDataNode</param>
        public void RemoveXmlInfosByPath(string path, string innerTextToCheck = "", Dictionary<string, string> attributeToCheck = null, bool deleteOnce = false)
        {
            if (string.IsNullOrEmpty(path)) return;

            List<string> NAME_LIST = path.Split('/').ToList();
            if (NAME_LIST.Count > 1)
            {
                XmlDataNode foundXmlDataNode = subNodes.Find((XmlDataNode xmlDataNode) =>
                {
                    return xmlDataNode.name == NAME_LIST[0];
                });
                if (foundXmlDataNode != null)
                    foundXmlDataNode.RemoveXmlInfosByPath(String.Join("/", NAME_LIST.Skip(1)), innerTextToCheck, attributeToCheck, deleteOnce);
            }
            else
            {
                List<XmlDataNode> foundNodesToRemove = subNodes.FindAll((XmlDataNode xmlDataNode) =>
                {
                    if (xmlDataNode.name != NAME_LIST[0])
                        return false;

                    if (!string.IsNullOrEmpty(innerTextToCheck) && attributeToCheck == null)
                        return xmlDataNode.innerText == innerTextToCheck;
                    else if ((!string.IsNullOrEmpty(innerTextToCheck) && attributeToCheck != null)
                          && (xmlDataNode.innerText == innerTextToCheck))
                    {
                        bool attritubteCheck = true;
                        attributeToCheck.ForEachKeyValuePairSafe((string atName, string atValue) =>
                        {
                            if (xmlDataNode.attribute.ContainsKey(atName)
                               && xmlDataNode.attribute[atName] != atValue)
                                attritubteCheck = false;
                            else if (!xmlDataNode.attribute.ContainsKey(atName))
                                attritubteCheck = false;
                        });
                        return attritubteCheck;
                    }
                    else if (string.IsNullOrEmpty(innerTextToCheck) && attributeToCheck != null)
                    {
                        bool attritubteCheck = true;
                        attributeToCheck.ForEachKeyValuePairSafe((string atName, string atValue) =>
                        {
                            if (xmlDataNode.attribute.ContainsKey(atName)
                               && xmlDataNode.attribute[atName] != atValue)
                                attritubteCheck = false;
                            else if (!xmlDataNode.attribute.ContainsKey(atName))
                                attritubteCheck = false;
                        });
                        return attritubteCheck;
                    }
                    else
                        return true;
                });

                if (deleteOnce && foundNodesToRemove.Count > 0)
                    subNodes.Remove(foundNodesToRemove[0]);
                else if(!deleteOnce)
                    foundNodesToRemove.ForEachSafe((XmlDataNode xmlDataToRemove) =>
                    {
                        subNodes.Remove(xmlDataToRemove);
                    });
                MainWindow.mainWindow.UpdateDebugInfo();
            }
        }
        #endregion

        #region Extra data functions
        /// <summary>
        /// Copy XmlDataNode and return it
        /// </summary>
        /// <returns>Copied XmlDataNode</returns>
        public XmlDataNode Copy()
        {
            return Tools.DeepCopy.DeepClone(this);
        }
        #endregion
    }
}
