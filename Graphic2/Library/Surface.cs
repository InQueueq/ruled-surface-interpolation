using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Graphic2.Library
{
    public class Surface
    {
        public PolyLine Lc1 { get; set; }

        public PolyLine Lc2 { get; set; }

        private List<Point3D> pc1;

        private List<Point3D> pc2;

        private Surface() { }

        public Surface(PolyLine p1, PolyLine p2, int d)
        {
            Lc1 = new PolyLine(p1.Lst);
            Lc2 = new PolyLine(p2.Lst);
            Connect(d);
        }

        public Surface(Surface s) 
        {
            Lc1 = new PolyLine(s.Lc1.Lst);
            Lc2 = new PolyLine(s.Lc2.Lst);

            pc1 = new List<Point3D>(s.pc1);
            pc2 = new List<Point3D>(s.pc2);
        }

        public void Draw(Graphics g, Pen pen, float angleX, float angleY, float angleZ, Point3D vector = null)
        {           
            Surface temp = new Surface(this);

            temp = temp.Isometric(angleX, angleY, angleZ);

            if (vector != null)
            {
                temp = temp.MoveOnVector(vector.X, vector.Y, vector.Z);
            }

            //    

            pen.Color = Color.Red;
            pen.Width = 1;

            for (int i = 0; i < temp.pc1.Count && i < temp.pc2.Count; i++)
            {
                g.DrawLine(pen, (PointF)temp.pc1[i], (PointF)temp.pc2[i]);
            }

            //

            pen.Width = 3;
            pen.Color = Color.DarkOrange;
            temp.Lc1.Draw(g, pen);

            pen.Color = Color.DarkBlue;
            temp.Lc2.Draw(g, pen);
        }

        public Surface OnXY()
        {
            Surface res = new Surface(this);

            for (int i = 0; i < res.Lc1.Lst.Count; i++)
            {
                res.Lc1.Lst[i] = MatrixManager.MoveOnVector(res.Lc1.Lst[i], 0, 0, -res.Lc1.Lst[i].Z);
            }

            for (int i = 0; i < res.Lc2.Lst.Count; i++)
            {
                res.Lc2.Lst[i] = MatrixManager.MoveOnVector(res.Lc2.Lst[i], 0, 0, -res.Lc2.Lst[i].Z);
            }

            for (int i = 0; i < res.pc1.Count; i++)
            {
                res.pc1[i]= MatrixManager.MoveOnVector(res.pc1[i], 0, 0, -res.pc1[i].Z);
            }

            for (int i = 0; i < res.pc2.Count; i++)
            {
                res.pc2[i] = MatrixManager.MoveOnVector(res.pc2[i], 0, 0, -res.pc2[i].Z);
            }

            return res;
        }

        public Surface OnXZ()
        {
            Surface res = new Surface(this);

            for (int i = 0; i < res.Lc1.Lst.Count; i++)
            {
                res.Lc1.Lst[i] = MatrixManager.MoveOnVector(res.Lc1.Lst[i], 0, -res.Lc1.Lst[i].Y, 0);
            }

            for (int i = 0; i < res.Lc2.Lst.Count; i++)
            {
                res.Lc2.Lst[i] = MatrixManager.MoveOnVector(res.Lc2.Lst[i], 0, -res.Lc2.Lst[i].Y, 0);
            }

            for (int i = 0; i < res.pc1.Count; i++)
            {
                res.pc1[i] = MatrixManager.MoveOnVector(res.pc1[i], 0, -res.pc1[i].Y, 0);
            }

            for (int i = 0; i < res.pc2.Count; i++)
            {
                res.pc2[i] = MatrixManager.MoveOnVector(res.pc2[i], 0, -res.pc2[i].Y, 0);
            }

            return res;
        }

        public Surface OnYZ()
        {
            Surface res = new Surface(this);

            for (int i = 0; i < res.Lc1.Lst.Count; i++)
            {
                res.Lc1.Lst[i] = MatrixManager.MoveOnVector(res.Lc1.Lst[i], -res.Lc1.Lst[i].X, 0, 0);
            }

            for (int i = 0; i < res.Lc2.Lst.Count; i++)
            {
                res.Lc2.Lst[i] = MatrixManager.MoveOnVector(res.Lc2.Lst[i], -res.Lc2.Lst[i].X, 0, 0);
            }

            for (int i = 0; i < res.pc1.Count; i++)
            {
                res.pc1[i] = MatrixManager.MoveOnVector(res.pc1[i], -res.pc1[i].X, 0, 0);
            }

            for (int i = 0; i < res.pc2.Count; i++)
            {
                res.pc2[i] = MatrixManager.MoveOnVector(res.pc2[i], -res.pc2[i].X, 0, 0);
            }

            return res;
        }

        private void Connect(int n) // n = density
        {
            pc1 = new List<Point3D>();
            pc2 = new List<Point3D>();

            float delta1 = Lc1.GetLength() / n; 
            float delta2 = Lc2.GetLength() / n;
            Point3D t = Lc1.Lst[0];
            pc1.Add(t);
            Point3D t2 = Lc2.Lst[0];
            pc2.Add(t2);


            float tempDelta = delta1;

            for (int z = 0; z < Lc1.Lst.Count - 1; z++)
            {
                Point3D start = Lc1.Lst[z];
                Point3D end = Lc1.Lst[z + 1];
                Point3D temp = start;

                while (true)
                {
                    float len = Point3D.GetLength(end, temp);
                    if (delta1 > len)
                    {
                        tempDelta -= len;
                        break;
                    }
                    else if (temp != start)
                    {
                        tempDelta = delta1;
                    }

                    temp = Point3D.MoveOnLength(temp, end, tempDelta);
                    pc1.Add(temp);
                }
            }

            tempDelta = delta2;

            for (int z = 0; z < Lc2.Lst.Count - 1; z++)
            {
                Point3D start = Lc2.Lst[z];
                Point3D end = Lc2.Lst[z + 1];
                Point3D temp = start;

                while (true)
                {
                    float len = Point3D.GetLength(end, temp);
                    if (delta2 > len)
                    {
                        tempDelta -= len;
                        break;
                    }
                    else if (temp != start)
                    {
                        tempDelta = delta2;
                    }

                    temp = Point3D.MoveOnLength(temp, end, tempDelta);
                    pc2.Add(temp);
                }
            }

            if (!pc1[pc1.Count - 1].Equals(Lc1.Lst[Lc1.Lst.Count - 1]))
            {
                pc1.Add(Lc1.Lst[Lc1.Lst.Count - 1]);
            }

            if (!pc2[pc2.Count - 1].Equals(Lc2.Lst[Lc2.Lst.Count - 1]))
            {
                pc2.Add(Lc2.Lst[Lc2.Lst.Count - 1]);
            }
        
        }

        #region Transformation

        public Surface AngleX(float angle)
        {
            Surface temp = new Surface(this);
            temp.Lc1 = temp.Lc1.AngleX(angle);
            temp.Lc2 = temp.Lc2.AngleX(angle);

            for (int i = 0; i < temp.pc1.Count && i < temp.pc2.Count; i++)
            {
                temp.pc1[i] = MatrixManager.OnAngleX(temp.pc1[i], angle);
                temp.pc2[i] = MatrixManager.OnAngleX(temp.pc2[i], angle);
            }

            return temp;
        }

        public Surface AngleY(float angle)
        {
            Surface temp = new Surface(this);
            temp.Lc1 = temp.Lc1.AngleY(angle);
            temp.Lc2 = temp.Lc2.AngleY(angle);

            for (int i = 0; i < temp.pc1.Count && i < temp.pc2.Count; i++)
            {
                temp.pc1[i] = MatrixManager.OnAngleY(temp.pc1[i], angle);
                temp.pc2[i] = MatrixManager.OnAngleY(temp.pc2[i], angle);
            }

            return temp;
        }

        public Surface AngleZ(float angle)
        {
            Surface temp = new Surface(this);
            temp.Lc1 = temp.Lc1.AngleZ(angle);
            temp.Lc2 = temp.Lc2.AngleZ(angle);

            for (int i = 0; i < temp.pc1.Count && i < temp.pc2.Count; i++)
            {
                temp.pc1[i] = MatrixManager.OnAngleZ(temp.pc1[i], angle);
                temp.pc2[i] = MatrixManager.OnAngleZ(temp.pc2[i], angle);
            }

            return temp;
        }

        public Surface MoveOnVector(float x, float y, float z)
        {
            Surface temp = new Surface(this);
            temp.Lc1 = temp.Lc1.MoveOnVector(x, y, z);
            temp.Lc2 = temp.Lc2.MoveOnVector(x, y, z);

            for (int i = 0; i < temp.pc1.Count && i < temp.pc2.Count; i++)
            {
                temp.pc1[i] = MatrixManager.MoveOnVector(temp.pc1[i], x, y, z);
                temp.pc2[i] = MatrixManager.MoveOnVector(temp.pc2[i], x, y, z);
            }

            return temp;
        }

        public Surface Isometric(float angleX, float angleY, float angleZ)
        {
            //120 135 y x
            Surface temp = new Surface(this);
            temp = temp.AngleX(angleX);
            temp = temp.AngleY(angleY);
            temp = temp.AngleZ(angleZ);
            return temp;
        }

        #endregion
    }
}
