using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Ex3.Models;
using System.IO;
using System.Net;

namespace Ex3.Controllers
{
    public class FlightController : Controller
    {
        #region singleton
        private static FlightController m_Instance = null;
        public static FlightController Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightController();
                }
                return m_Instance;
            }
        }
        #endregion
        string path { get; set; }
        StreamWriter writer { get; set; }
        StreamReader reader { get; set; }
        // GET: Flight
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Drawing a point on the plain's initial location.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Initial(string IP, int port)
        {
            IPAddress test;
            if (!IPAddress.TryParse(IP, out test))
            {
                load(IP, port);
                return View("load");
            }
            Model.Instance.Connect(IP, port);
            getLocation();
            return View();
        }

        /// <summary>
        /// Showing an animation of the flight location.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="port"></param>
        /// <param name="frequency">the frequency of the interval</param>
        /// <returns></returns>
        public ActionResult display_flight(string IP, int port, int frequency)
        {
            
            Model.Instance.Connect(IP, port);
            getLocation();
            ViewBag.frequency = frequency;
            return View();
        }
        /// <summary>
        /// Saving flight information to a text file.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="port"></param>
        /// <param name="frequency"></param>
        /// <param name="time">how much time the interval will be running</param>
        /// <param name="filename">the name of the file</param>
        /// <returns></returns>
        public ActionResult save(string IP, int port, int frequency, int time, string filename)
        {
            Model.Instance.Connect(IP, port);
            ViewBag.time = time;
            ViewBag.frequency = frequency;
            Instance.path = AppDomain.CurrentDomain.BaseDirectory + @"\" + filename + ".txt";
            if(System.IO.File.Exists(Instance.path))
                System.IO.File.Delete(Instance.path);
            Instance.writer = System.IO.File.AppendText(Instance.path);

            return View();

        }
        /// <summary>
        /// Loading flight data from file and showing the location on the map.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public ActionResult load(string filename, int frequency)
        {
            ViewBag.frequency = frequency;
            Instance.path = AppDomain.CurrentDomain.BaseDirectory + @"\" + filename + ".txt";
            Instance.reader = new StreamReader(Instance.path);
            readLocation();
            return View();
        }
        /// <summary>
        /// setting the current location.
        /// </summary>
        [HttpPost]
        public void getLocation()
        {
            Model.Instance.setLocation();
            ViewBag.lat = Location.Instance.Lat;
            ViewBag.lon = Location.Instance.Lon;
        }
        /// <summary>
        /// Casting the location to an xml document.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ToXml()
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            Model.Instance.setLocation();
            Location.Instance.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
        /// <summary>
        /// getting location from a file and casting it to xml.
        /// </summary>
        /// <returns></returns>
        public string FileToXml()
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            if (Model.Instance.readLocation(Instance.reader))
            {
                Location.Instance.ToXml(writer);
            }
            writer.WriteStartElement("Location");
            writer.WriteElementString("Lon", "null");
            writer.WriteElementString("Lat", "null");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        /// <summary>
        /// saving flight data on a file.
        /// </summary>
        [HttpPost]
        public void ToFile()
        {
            Model.Instance.saveToFile(Instance.writer);
        }
        /// <summary>
        /// setting plain's lon, lat, alt, heading and speed to an Info class.
        /// </summary>
        [HttpPost]
        public void setInfo()
        {
            Model.Instance.setInfo();
        }
        /// <summary>
        /// reading location from file.
        /// </summary>
        [HttpPost]
        public void readLocation()
        {
            Model.Instance.readLocation(Instance.reader);
            ViewBag.lat = Location.Instance.Lat;
            ViewBag.lon = Location.Instance.Lon;
        }
    }
}