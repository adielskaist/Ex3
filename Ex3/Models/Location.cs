using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class Location
    {
        private static Location m_Instance = null;
        public static Location Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new Location();
                }
                return m_Instance;
            }
        }
        public double Lon { get; set; }
        public double Lat { get; set; }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Location");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteEndElement();
        }
    }
}