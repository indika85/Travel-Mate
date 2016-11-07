using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace TravelMate
{
    public partial class Form1 : Form
    {
        private ArrayList pointList;
        private ArrayList boundinBox;
        private Rectangle currentRectangle;
        private ArrayList selectedPoints;
        private ArrayList sorted;
        private ArrayList selectedIndexes;
        private ArrayList original;

        private int currentIndex = 0;

        private ArrayList distance;
        private ArrayList towns;

        public Form1()
        {
            InitializeComponent();
            pointList = new ArrayList();
            boundinBox=new ArrayList();
            selectedPoints = new ArrayList();
            distance = new ArrayList();
            towns = new ArrayList();
            selectedIndexes = new ArrayList();
            original = new ArrayList();
            sorted = new ArrayList();

            lbl_hide.Height = 35;

            currentRectangle = Rectangle.Empty;

            getPointList();// gets the points from the file
            drawBoundingRectangles();//Draws the bounding boxes to check if the mouse pointer is in a correct location

        }

        private void ll_Close_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }

        private void ll_next_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tc_main.SelectedIndex = 1;
        }

        private void ll_pre_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tc_main.SelectedIndex = 0;
            
        }

        private void get_Cursor_Location()
        {
            Point p = Cursor.Position;
            p = pictureBox1.PointToClient(p);
            Text = p.ToString();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //------------------------- For testing purpose only -----------------------------
            //Point p = Cursor.Position;
            //p = pictureBox1.PointToClient(p);
            //lbl_hide.Text = p.ToString();
            //--------------------------------------------------------------------------------
            
            //get_Cursor_Location();
            checkMouseLocation();
        }
        private void checkMouseLocation()
        {
            Point p = Cursor.Position;
            p = pictureBox1.PointToClient(p);
            Rectangle rec=new Rectangle();
            for (int i = 0; i < boundinBox.Count; i++)
            {
                rec = (Rectangle)boundinBox[i];
                if (rec.Contains(p))
                {
                        Cursor.Current = Cursors.Hand;

                        pictureBox4.Image = imageList1.Images[i + 1];
                        currentIndex = i;
                        string name = xmxData.getName(i);
                        lbl_Info.Text = xmxData.getDetails(name).Trim();
                        //ll_partner.Enabled = true;
                        ll_partner.Visible = true;
                        return;
                }
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void showDetails(Point p)
        {
            //Show the details when the pointer is near a city
            pictureBox1.Refresh();
            Graphics g = pictureBox1.CreateGraphics();
            Rectangle rec=new Rectangle(p,new Size(200,120));
            LinearGradientBrush b = new LinearGradientBrush(rec, Color.FromArgb(220, Color.YellowGreen), Color.FromArgb(180, Color.White), LinearGradientMode.Vertical);
            g.DrawRectangle(new Pen(Brushes.DarkBlue, 2), rec);
            g.FillRectangle(b, rec);
        }
        private void writeToFile(Point p)
        {
            fileHandle.writoToFile(p);
        }
        private void addToSelectedPoints(Point p)
        {
            selectedPoints.Add(p);
        }
        private void drawSelectedPoints()
        {
            pictureBox1.Refresh();
            Graphics g = pictureBox1.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Point p = new Point();
            for (int i = 0; i < selectedPoints.Count; i++)
            {
                LinearGradientBrush b = new LinearGradientBrush(new Rectangle(new Point(p.X - 15, p.Y - 15), new Size(30, 30)), Color.FromArgb(160, Color.Red), Color.FromArgb(80, Color.Red), LinearGradientMode.Vertical);
                p = (Point)selectedPoints[i];
                g.FillEllipse(b, new Rectangle(new Point(p.X - 15, p.Y - 15), new Size(30, 30)));
                g.DrawEllipse(new Pen(Brushes.White, 2), new Rectangle(new Point(p.X - 15, p.Y - 15), new Size(30, 30)));
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            Point p = Cursor.Position;
            p = pictureBox1.PointToClient(p);
            Point pt=new Point();
            //fileHandle.writoToFile(p);/////*************************************************    TEMP
            //return;

            //*******************************************************
            Rectangle rec = new Rectangle();
            for (int i = 0; i < boundinBox.Count; i++)
            {
                rec = (Rectangle)boundinBox[i];
                if (rec.Contains(p))
                {
                    p = new Point(rec.Location.X + rec.Width / 2, rec.Location.Y + rec.Height / 2);
                    for (int j = 0; j < selectedPoints.Count; j++)
                    {
                        pt=(Point)selectedPoints[j];
                        if (pt.Equals(p))
                        {
                            //remove that point
                            selectedPoints.RemoveAt(j);
                            selectedIndexes.RemoveAt(j);
                            original.RemoveAt(j);
                            //return;
                            goto xxx;
                        }
                    }
                    addToSelectedPoints(p);
                    original.Add(p);
                    selectedIndexes.Add(i);
                    
                    break;
                }
            }
            //*******************************************************
            //addToSelectedPoints(p);
            xxx:
            drawSelectedPoints();

            return;
        }

        private void ll_Next1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (selectedPoints.Count < 1)
            {
                MessageBox.Show("No destinations were selected!","TravelMate",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            bool nn = true;
            if (rb_ni.Checked)
                nn = false;

            //Hardcode the location of Colombo..
            selectedPoints.Insert(0, new Point(60, 446));
            original.Insert(0, new Point(60, 446));
;
            sorted = null;
            sorted = new ArrayList();
            sorted = findPath.travelling_Salesman(selectedPoints, nn);

            tc_main.SelectedIndex = 2;

            displayInfo();

            drawPath();
            tmrRedraw.Start();
        }

        private void displayInfo()
        {
            lblDist.Text = "";
            int tot = 0;
            string from = "";
            string to = "";
            int dist = 0;
            int lastIndex = 0;
            Point p1, p2;
            distance.Clear();

            for (int i = 0; i < sorted.Count - 1; i++)
            {
                p1 = (Point)sorted[i];
                for (int j = 0; j < original.Count; j++)
                {
                    p2 = (Point)original[j];
                    if (p1.Equals(p2))
                    {
                        if (i == 0)
                            from = "colombo";
                        else
                            from = xmxData.getName((int)selectedIndexes[j-1]);
                        break;
                    }
                }
                p1 = (Point)sorted[i+1];
                for (int j = 0; j < original.Count; j++)
                {
                    p2 = (Point)original[j];
                    if (p1.Equals(p2))
                    {
                        to = xmxData.getName((int)selectedIndexes[j-1]);
                        lastIndex = j-1;
                        break;
                    }
                }

                dist = xmxData.getDiatance(from, to);
                distance.Add(from + " - " + to + "  " + dist.ToString() + " Km" + "\n");
                //lblDist.Text += from + " - " + to + "  " + dist.ToString() + " Km"+"\n";
                tot += dist;
            }
            from =  xmxData.getName((int)selectedIndexes[lastIndex]);
            to = "colombo";
            dist = xmxData.getDiatance(from, to);
            distance.Add(from + " - " + to + "  " + dist.ToString() + " Km" + "\n");
            //lblDist.Text += from + " - " + to + "  " + dist.ToString() + " Km" + "\n";
            tot += dist;
            distance.Add("\n \n" + "Total distance : " + tot.ToString() + "Km");
            //lblDist.Text += "\n \n" + "Total distance : " + tot.ToString() + "Km";

            if (rb_nn.Checked)
            {
                for (int i = 0; i < distance.Count; i++)
                {
                    lblDist.Text += distance[i].ToString();
                }
            }
            else
            {
                for (int i = distance.Count-2; i >=0; i--)
                {
                    lblDist.Text += distance[i].ToString();
                }
                lblDist.Text += "\n"+"\n"+distance[distance.Count-1].ToString();
            }

        }

        private void drawPath()
        {
            Graphics g = pictureBox3.CreateGraphics();
            LinearGradientBrush b = new LinearGradientBrush(new Rectangle(0, 0, 50, 50), Color.FromArgb(255, Color.DimGray), Color.FromArgb(160, Color.DimGray), LinearGradientMode.Vertical);
            Point p1=new Point();
            Point p2=new Point();
            Size s = new Size(20, 20);

            Font fnt = new Font("Trebuchet MS", 12);

            Point t1, t2, t3;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            for (int i = 0; i < sorted.Count; i++)
            {
                p1=(Point)sorted[i];
                if(i<sorted.Count-1)
                    p2=(Point)sorted[i+1];
                else
                    p2 = (Point)sorted[0];

                t1 = new Point(p1.X-4, p1.Y - 10);
                t2 = new Point(p1.X - 11, p1.Y + 10);
                t3 = new Point(p1.X + 3, p1.Y + 10);

                //----- Drawing the Traingles--------
                g.DrawLine(new Pen(Brushes.Red, 2), t1, t2);
                g.DrawLine(new Pen(Brushes.Red, 2), t2, t3);
                g.DrawLine(new Pen(Brushes.Red, 2), t3, t1);

                //----- Drawing the paths -----------
                g.DrawLine(new Pen(b,2), p1, p2);

                //----- Drawing the Circles ---------
                g.FillEllipse(b, new Rectangle(new Point(t1.X, t1.Y - 8), s));

                //----- Drawing the Numbers ---------
                if (rb_nn.Checked)
                    g.DrawString(i.ToString(), fnt, Brushes.WhiteSmoke, t1.X + 3, t1.Y - 8);
                else
                {
                    if(i==0)
                        g.DrawString(Convert.ToString(sorted.Count - sorted.Count), fnt, Brushes.WhiteSmoke, t1.X + 3, t1.Y - 8);
                    else
                        g.DrawString(Convert.ToString(sorted.Count - i), fnt, Brushes.WhiteSmoke, t1.X + 3, t1.Y - 8);
                }

                
            }
            p1 = (Point)sorted[0];
            p2 = (Point)sorted[sorted.Count - 1];

            t1 = new Point(p2.X-4, p2.Y - 10);
            t2 = new Point(p2.X - 11, p2.Y + 10);
            t3 = new Point(p2.X + 3, p2.Y + 10);

            g.DrawLine(new Pen(Brushes.Red, 2), t1, t2);
            g.DrawLine(new Pen(Brushes.Red, 2), t2, t3);
            g.DrawLine(new Pen(Brushes.Red, 2), t3, t1);

            g.DrawLine(new Pen(b, 2), p1, p2);

            //g.DrawString(Convert.ToString(selectedPoints.Count-1), new Font("Arial Black", 12), Brushes.White, t1.X, t1.Y - 8);
        }

        private void getPointList()
        {
            pointList = fileHandle.readFromFile();
        }

        private void drawBoundingRectangles()
        {
           for (int i = 0; i < pointList.Count; i++)
            {
                Point temp = (Point)pointList[i];
                boundinBox.Add(new Rectangle(temp.X - 8, temp.Y - 8, 25, 15));
            }
        }

        private void ll_Redraw_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            drawPath();
        }

        private void LL_Prev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tc_main.SelectedIndex = 1;
            drawSelectedPoints();
            if (selectedIndexes.Count > 0)
            {
                original.RemoveAt(0);
                selectedPoints.RemoveAt(0);
            }

        }

        private void ll_RedrawPoints_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            drawSelectedPoints();
        }

        private void ll_partner_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openCHM(xmxData.getName(currentIndex));
        }
        private void openCHM(string city)
        {
            try
            {
                Process.Start(city + ".chm");
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading " + city + ".chm file.", "TravelMate - File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ll_abt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tc_main.SelectedIndex = 3;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tc_main.SelectedIndex = 0;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tc_main.SelectedIndex = 3;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tc_main.SelectedIndex = 3;
        }

        private void ll_Exit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }

        private void tmrRedraw_Tick(object sender, EventArgs e)
        {
            drawPath();
            tmrRedraw.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit the program ?", "TravelMate", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

    }
}
