using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace WPF_Media_Player
{
    /// <summary>
    /// saves /loads media items from xml files
    /// </summary>
    class XmlHandler
    {
        #region public methods
        /// <summary>
        /// saves media items to an xml file
        /// </summary>
        public static bool WriteXmlFile(string filename, List<MediaItem> mediaItems) 
        {
            using (XmlTextWriter writer = new XmlTextWriter(@filename, null))
            {
                //Write the root element
                writer.WriteStartElement("MediaItems");
                //write sub elements
                foreach (MediaItem mi in mediaItems)
                {
                    writer.WriteStartElement("MediaItem");
                    writer.WriteElementString("ItemUri", @mi.ItemUri.ToString());
                    writer.WriteElementString("Rating", mi.ItemRating.ToString());
                    writer.WriteEndElement();
                }
                // end the root element
                writer.WriteEndElement();
                return true;
            }
            return false;
        }

        /// <summary>
        /// loads media items from an xml file
        /// </summary>
        public static List<MediaItem> ReadXmlFile(string filename)
        {

            List<MediaItem> mediaItemsRead = new List<MediaItem>();
            XmlDocument xd = new XmlDocument();
            xd.Load(@filename);
            XmlNodeList xNodes=   xd.SelectNodes("/MediaItems/MediaItem");
            foreach (XmlNode xn in xNodes)
            {
                XmlNode xp = xn.SelectSingleNode("ItemUri");
                string path = xp.InnerText;
                XmlNode xr = xn.SelectSingleNode("Rating");
                string rating = xr.InnerText;
                mediaItemsRead.Add(new MediaItem(xp.InnerText,int.Parse(xr.InnerText)));
            }
            return mediaItemsRead;
        }
        #endregion
    }
}
