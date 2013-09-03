#region Copyright © 2007 Rotem Sapir
/*
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from the
 * use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not claim
 * that you wrote the original software. If you use this software in a product,
 * an acknowledgment in the product documentation is required, as shown here:
 *
 * Portions Copyright © 2007 Rotem Sapir
 *
 * 2. No substantial portion of the source code of this library may be redistributed
 * without the express written permission of the copyright holders, where
 * "substantial" is defined as enough code to be recognizably from this library.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace ZagueEF.Core
{
    public static class TreeBuilder
    {
        public static System.Xml.XmlDocument GenerateTree(string startingNodeId, TreeData.TreeDataTableDataTable dt)
        {
            XmlDocument xml = new XmlDocument();
            string rootDescription=string.Empty;
            string rootNote = string.Empty;

            if(dt.Select(string.Format("nodeID='{0}'",startingNodeId)).Length>0)
            {
                rootDescription = ((TreeData.TreeDataTableRow) dt.Select(string.Format("nodeID='{0}'",startingNodeId))[0]).nodeDescription;
                rootNote = ((TreeData.TreeDataTableRow) dt.Select(string.Format("nodeID='{0}'",startingNodeId))[0]).nodeNote;
            }

            XmlNode RootNode = GetXMLNode(xml, startingNodeId, rootDescription, rootNote);
            xml.AppendChild(RootNode);
            BuildTree(xml, dt, RootNode, 0);

            dt.Dispose();
            dt = null;

            return xml;
        }

        #region Private Methods
        /// <summary>
        /// convert the datatable to an XML document
        /// </summary>
        /// <param name="oNode"></param>
        /// <param name="y"></param>
        private static void BuildTree(XmlDocument xml, TreeData.TreeDataTableDataTable dtTree, XmlNode oNode, int y)
        {
            XmlNode childNode = null;
            //has children
            foreach (TreeData.TreeDataTableRow  childRow in dtTree.Select(
                string.Format("parentNodeID='{0}'", oNode.Attributes["nodeID"].InnerText)))
            {
                //for each child node call this function again
                childNode = GetXMLNode(xml, childRow.nodeID, childRow.nodeDescription, childRow.nodeNote);
                oNode.AppendChild(childNode);
                BuildTree(xml, dtTree, childNode, y + 1);
            }
        }

        /// <summary>
        /// Get the maximum x of the lowest child on the current tree of nodes
        /// Recursion does not work here, so we'll use a loop to climb down the tree
        /// Recursion is not a solution because we need to return the value of the last leaf of the tree.
        /// That would require managing a global variable.
        /// </summary>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private static int GetMaxXOfDescendants(XmlNode CurrentNode)
        {
            int Result = -1;
            
            while (CurrentNode.HasChildNodes)
            {
               CurrentNode = CurrentNode.LastChild;
               
            }
           
            Result = Convert.ToInt32(CurrentNode.Attributes["X"].InnerText);
            
            return Result;
            //int Result = -1;
            //if (CurrentNode.HasChildNodes)
            //{
            //    GetMaxXOfDescendants(CurrentNode.LastChild);
            //}
            //else
            //{
            //    Result = Convert.ToInt32(CurrentNode.Attributes["X"].InnerText);
            //}
            //return Result;
        }

        /// <summary>
        /// create an xml node based on supplied data
        /// </summary>
        /// <returns></returns>
        private static XmlNode GetXMLNode(XmlDocument xml, string nodeID,string nodeDescription,string nodeNote)
        {
            //build the node
            XmlNode resultNode = xml.CreateElement("Node");
            XmlAttribute attNodeID = xml.CreateAttribute("nodeID");
            
            XmlAttribute attNodeDescription = xml.CreateAttribute("nodeDescription");
            XmlAttribute attNodeNote = xml.CreateAttribute("nodeNote");
            
            //set the values of what we know
            attNodeID.InnerText = nodeID;
            
            attNodeDescription.InnerText=nodeDescription;
            attNodeNote.InnerText=nodeNote;
            resultNode.Attributes.Append(attNodeID);
            resultNode.Attributes.Append(attNodeDescription);
            resultNode.Attributes.Append(attNodeNote);

            return resultNode;
        }

        #endregion

       
    }
}
