using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace TravelMate
{
    class findPath
    {
        private static int MAX_X, MAX_Y;
        private static ArrayList usedIndexes;
        private static ArrayList completedPoints;
        private static bool nnh = true;
        //private static wait w;

        public static ArrayList travelling_Salesman(ArrayList list, bool use_Nearest_Neighbour_Heristics)
        {

            MAX_X = 0;
            MAX_Y = 0;
            nnh = use_Nearest_Neighbour_Heristics;
            completedPoints = new ArrayList();
            usedIndexes = new ArrayList();
            int minVal, minIndex;
            Point p = (Point)list[0];
            minVal = p.X + p.Y;
            minIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                //gets the Starting position (Nearect location to 0,0)
                p = (Point)list[i];
                if (p.X + p.Y < minVal)
                {
                    minVal = p.X + p.Y;
                    minIndex = i;
                }
                if (p.X > MAX_X) MAX_X = p.X;
                if (p.Y > MAX_Y) MAX_Y = p.Y;
            }
            //completedPoints.Add(list[minIndex]);
            //usedIndexes.Add(minIndex);

            completedPoints.Add(list[0]);
            usedIndexes.Add(0);

            //********************************************************
            if (nnh)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    //***************Using nearest Neighbour Heristics algorhythem***********
                    completedPoints.Add(getNextClosesPoint_neighbour(list, (Point)completedPoints[i]));
                }
            }
            else
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    //***************Using nearest Insertion Heristics algorhythem***********
                    insertThePoint(getNextClosesPoint_insertion(list, completedPoints.Count));
                }
            }

            return completedPoints;

        }
        private static ArrayList getTheSortedPoints(ArrayList list)
        {
            return completedPoints;
        }

        private static Point getNextClosesPoint_insertion(ArrayList original, int c)
        {
            ArrayList index_value = new ArrayList();//used to store the indexes and the values as points
            int xDist = 0, yDist = 0;
            int currentIndex = 0;
            double minDist = Math.Sqrt((MAX_X * MAX_X) + (MAX_Y * MAX_Y));
            double rdist;
            Point p;
            Point currentPoint;

            for (int outter = 0; outter < c; outter++)
            {
                currentPoint = (Point)completedPoints[outter];
                for (int inner = 0; inner < original.Count; inner++)
                {
                    if (!checkIfPointIsUsed(inner))
                    {
                        p = (Point)original[inner];
                        xDist = currentPoint.X - p.X;
                        yDist = currentPoint.Y - p.Y;
                        rdist = Math.Sqrt((xDist * xDist) + (yDist * yDist));
                        if (rdist < minDist)
                        {
                            minDist = rdist;
                            currentIndex = inner;
                        }
                    }
                }
                index_value.Add(new PointF(currentIndex, (float)minDist));
            }
            int temp = getMinimumValue(index_value);
            usedIndexes.Add(temp);
            //MessageBox.Show("getNextClosesPoint  min val index" + temp.ToString());
            return (Point)original[temp];
        }
        private static int getMinimumValue(ArrayList ls)
        {
            //string ttt = "";//*****************************
            PointF p;
            int minIndex = 0;
            float minVal = (float)(MAX_Y * MAX_Y) + (MAX_X * MAX_X);//just to give a high value initially
            for (int i = 0; i < ls.Count; i++)
            {
                p = (PointF)ls[i];
                if (p.Y < minVal)
                {
                    minVal = p.Y;
                    minIndex = (int)p.X;
                }
                //ttt += ls[i].ToString() + "\n";
                //MessageBox.Show(ttt + " i value = " + i.ToString());
            }

            return minIndex;
        }
        private static void insertThePoint(Point p)//Insert the point to the completed list
        {
            //MessageBox.Show("insertThePoint -completedPoints.Count= " + completedPoints.Count.ToString());
            double totalDist = 0.0;
            double minDist = 0.0;
            int xDist = 0, yDist = 0;
            ArrayList position_distance = new ArrayList();
            //ArrayList temp = new ArrayList();
            Point p1, p2;
            if (completedPoints.Count == 1)
            {
                completedPoints.Add(p);
                return;
            }
            for (int i = 0; i < completedPoints.Count - 1; i++)
            {
                ArrayList temp = new ArrayList(completedPoints);
                //temp = completedPoints;
                temp.Insert(i + 1, p);
                totalDist = 0.0;
                for (int j = 0; j < temp.Count - 1; j++)
                {
                    p1 = (Point)temp[j];
                    p2 = (Point)temp[j + 1];
                    xDist = p1.X - p2.X;
                    yDist = p1.Y - p2.Y;
                    totalDist += Math.Sqrt((xDist * xDist) + (yDist * yDist));
                }
                position_distance.Add(new PointF(i, (float)totalDist));
            }
            PointF f;
            int index = 0; ;



            for (int i = 0; i < position_distance.Count; i++)//get the max distance
            {
                f = (PointF)position_distance[i];
                if (f.Y > minDist) minDist = f.Y;
            }

            for (int i = 0; i < position_distance.Count; i++)//get the min distance
            {
                f = (PointF)position_distance[i];
                if (f.Y < minDist)
                {
                    minDist = f.Y;
                    index = (int)f.X;
                }
            }
            completedPoints.Insert(index + 1, p);
        }
        private static Point getNextClosesPoint_neighbour(ArrayList original, Point currentPoint)
        {
            int xDist = 0, yDist = 0;
            int currentIndex = 0;
            double realDist = Math.Sqrt((MAX_X * MAX_X) + (MAX_Y * MAX_Y));
            Point p;
            for (int i = 0; i < original.Count; i++)
            {
                if (!checkIfPointIsUsed(i))
                {
                    p = (Point)original[i];
                    xDist = currentPoint.X - p.X;
                    yDist = currentPoint.Y - p.Y;
                    double rdist = Math.Sqrt((xDist * xDist) + (yDist * yDist));
                    if (rdist < realDist)
                    {
                        realDist = rdist;
                        currentIndex = i;
                    }
                }
            }
            usedIndexes.Add(currentIndex);
            return (Point)original[currentIndex];
        }
        private static bool checkIfPointIsUsed(int currentIndex)
        {
            //string t = "";
            for (int i = 0; i < usedIndexes.Count; i++)
            {
                int index = (int)usedIndexes[i];
                if (index == currentIndex)
                    return true;
                //t += index.ToString() + "\n";
                //MessageBox.Show("Used indexes  \n"+t);
            }
            return false;
        }
    }
}
