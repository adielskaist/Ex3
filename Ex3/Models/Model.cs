using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading;
using System.Xml;
using System.IO;

namespace Ex3.Models
{
    class Model
    {
        #region singleton
        private static Model m_Instance = null;

        public static Model Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Model();
                }
                return m_Instance;
            }
        }
        #endregion
        Mutex mutex = new Mutex();
        bool connected = false;
        /// <summary>
        /// Connects to simulator server.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="port"></param>
        public void Connect(string IP, int port)
        {
            if (connected)
                return;
            FlightTelnetClient.Instance.connect(IP, port);
            connected = true;
        }
        /// <summary>
        /// Sending a request to the server and getting a response.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>a string</returns>
        private string getLine(string cmd)
        {
            FlightTelnetClient.Instance.write(cmd);
            return FlightTelnetClient.Instance.read();
        }
        private double getLat()
        {
            return new Parser().Parse(getLine("get /position/latitude-deg\r\n"));
        }
        private double getLon()
        {
            return new Parser().Parse(getLine("get /position/longitude-deg\r\n"));
        }

        private double getAlt()
        {
            return new Parser().Parse(getLine("get /instrumentation/altimeter/indicated-altitude-ft\r\n"));
        }

        private double getSpeed()
        {
            return new Parser().Parse(getLine("get /instrumentation/airspeed-indicator/indicated-speed-kt\r\n"));
        }

        private double getHeading()
        {
            return new Parser().Parse(getLine("get /instrumentation/heading-indicator/indicated-heading-deg\r\n"));
        }
        /// <summary>
        /// setting the location class.
        /// </summary>
        public void setLocation()
        {
            mutex.WaitOne();
            Location.Instance.Lon = getLon();
            Location.Instance.Lat = getLat();
            mutex.ReleaseMutex();
        }
        /// <summary>
        /// setting the info class.
        /// </summary>
        public void setInfo()
        {
            mutex.WaitOne();
            Info.Instance.Lon = getLon();
            Info.Instance.Lat = getLat();
            Info.Instance.Alt = getAlt();
            Info.Instance.Speed = getSpeed();
            Info.Instance.Heading = getHeading();
            mutex.ReleaseMutex();
        }
        /// <summary>
        /// writing info to a text file.
        /// </summary>
        /// <param name="writer"></param>
        public void saveToFile(StreamWriter writer)
        {
            mutex.WaitOne();
            writer.Write("Lon'" + Info.Instance.Lon.ToString());
            writer.Write(";Lat'" + Info.Instance.Lat.ToString());
            writer.Write(";Alt'" + Info.Instance.Alt.ToString());
            writer.Write(";Heading'" + Info.Instance.Speed.ToString());
            writer.WriteLine(";Speed'" + Info.Instance.Heading.ToString());
            writer.Flush();
            mutex.ReleaseMutex();
        }
        /// <summary>
        /// getting location from a text file and setting it into a location class.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>false if the line is empty ant true otherwise</returns>
        public bool readLocation(StreamReader reader)
        {
            mutex.WaitOne();
            string line = reader.ReadLine();
            if (line == null)
            {
                mutex.ReleaseMutex();
                return false;
            }
            string[] tokens = new Parser().parseFromFile(line);
            Location.Instance.Lon = new Parser().Parse(tokens[0]);
            Location.Instance.Lat = new Parser().Parse(tokens[1]);
            mutex.ReleaseMutex();
            return true;
        }
    }
}