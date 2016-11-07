using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace TravelMate
{
   
    class fileHandle
    {
        private static string path = Application.StartupPath+"\\locationCordinates.txt";
        public static void writoToFile(Point p)
        {
            StreamWriter writer;
            if (!File.Exists(path))
            {
                writer = File.CreateText(path);
            }
            else
            {
                writer = File.AppendText(path);
            }
            writer.WriteLine(p.ToString()); 
            writer.Close();
        }

        public static ArrayList readFromFile()
        {
            ArrayList list = new ArrayList();
            try
            {
                StreamReader reader = File.OpenText(path);
                string info = reader.ReadToEnd();
                reader.Close();
                int current = 0;
                while (current < info.Length - 1)
                {
                    current = info.IndexOf("{", current);
                    if (current < 0)
                    {
                        break;
                    }
                    int x = Convert.ToInt32(info.Substring(current + 3, info.IndexOf(",", current + 3) - current - 3));
                    current = info.IndexOf(",", current);
                    int y = Convert.ToInt32(info.Substring(current + 3, info.IndexOf("}", current + 3) - current - 3));
                    list.Add(new Point(x, y));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading locationCordinates.txt file.\n"+e.Message, "TravelMate - File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return list;
        }
    }
}
