using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class Info
    {
        private static Info m_Instance = null;

        public static Info Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Info();
                }
                return m_Instance;
            }
        }

        public double Lon { get; set; }
        public double Lat { get; set; }
        public double Alt { get; set; }
        public double Heading { get; set; }
        public double Speed { get; set; }

        public void save(XmlTextWriter writer)
        {
            writer.WriteStartElement("Info");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Alt", this.Alt.ToString());
            writer.WriteElementString("Heading", this.Heading.ToString());
            writer.WriteElementString("Speed", this.Speed.ToString());
            writer.WriteEndElement();
        }
    }
}