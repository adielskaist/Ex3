using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    class Parser
    {
        #region singleton
        //private static Parser m_Instance = null;
        //public static Parser Instance
        //{
        //    get
        //    {
        //        if (m_Instance == null)
        //        {
        //            m_Instance = new Parser();
        //        }
        //        return m_Instance;
        //    }
        //}
        #endregion
        public double Parse(string line)
        {
            return Double.Parse(line.Split('\'')[1]);
        }

        public string[] parseFromFile(string Line)
        {
            return Line.Split(';');
        }


    }
}