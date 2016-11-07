using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TravelMate
{
    class xmxData
    {
         public static int getDiatance(string from, string to)
        {
            string path = "data.xml";
            DataSet ds = new DataSet();
            ds.ReadXml(path);
            for (int i = 0; i < ds.Tables["node"].Rows.Count; i++)
            {
                if (ds.Tables["node"].Rows[i][0].ToString().ToLower() == from.ToLower().Trim() && ds.Tables["node"].Rows[i][1].ToString().ToLower() == to.ToLower().Trim())
                {
                    return Convert.ToInt32(ds.Tables["node"].Rows[i][2]);
                }
            }
            return 0;
        }

         public static string getName(int index)
         {
             string path = "data.xml";
             DataSet ds = new DataSet();
             ds.ReadXml(path);
             for (int i = 0; i < ds.Tables["mapping"].Rows.Count; i++)
             {
                 if (ds.Tables["mapping"].Rows[i][0].ToString() == index.ToString())
                 {
                     return Convert.ToString(ds.Tables["mapping"].Rows[i][1]);
                 }
             }
             return "";
         }
         public static string getDetails(string name)
         {
             string path = "data.xml";
             DataSet ds = new DataSet();
             ds.ReadXml(path);
             for (int i = 0; i < ds.Tables["city"].Rows.Count; i++)
             {
                 if (ds.Tables["city"].Rows[i][0].ToString() == name.ToString())
                 {
                     return Convert.ToString(ds.Tables["city"].Rows[i][1]);
                 }
             }
             return "";
         }
    }
}
