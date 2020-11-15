using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Graphic2.Library
{
    public class PolyLine
    {
        public List<Point3D> Lst { get; set; }     

        private PolyLine()
        {
            Lst = new List<Point3D>();
        }

        public PolyLine(IEnumerable<Point3D> points)
        {
            Lst = new List<Point3D>(points);
        }

        public PolyLine(PolyLine poly)
        {
            Lst = new List<Point3D>(poly.Lst);
        }

        public float GetLength(int a = 0, int b = -1)
        {
            float res = 0;

            if (b == -1)
            {
                b = Lst.Count - 1;
            }

            for (int i = a; i < Lst.Count - 1 && i <= b; i++)
            {
                res += (float)Math.Sqrt(Math.Pow(Lst[i + 1].X - Lst[i].X, 2) + Math.Pow(Lst[i + 1].Y - Lst[i].Y, 2) + Math.Pow(Lst[i + 1].Z - Lst[i].Z, 2));
                a = i;
            }

            if (a != b - 1)
            {
                return -1;
            }

            return res;
        }

        public Point3D GetVector(int i)
        {
            if (i >= Lst.Count)
            {
                return null;
            }
            else
            {
                return new Point3D((Lst[i + 1].X - Lst[i].X), (Lst[i + 1].Y - Lst[i].Y), (Lst[i + 1].Z - Lst[i].Z));
            }
        }

        public void Draw(Graphics g, Pen pen, Point3D vector = null)
        {
            PolyLine temp = new PolyLine(this);            
            if (vector != null)
            {
                temp = temp.MoveOnVector(vector.X, vector.Y, vector.Z);
            }

            for (int i = 0; i < Lst.Count - 1; i++)
            {
                g.DrawLine(pen, (PointF)temp.Lst[i], (PointF)temp.Lst[i + 1]);
            }
        }

        public static explicit operator PolyLine(string str)
        {
            PolyLine res = new PolyLine();

            string[] arr = str.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var v in arr)
            {
                res.Lst.Add((Point3D)v);
            }

            return res;
        }

        public static explicit operator PolyLine(string[] arr)
        {
            PolyLine res = new PolyLine();

            foreach (var v in arr)
            {
                res.Lst.Add((Point3D)v);
            }

            return res;
        }

        public override string ToString()
        {
            string res = string.Empty;
            foreach (var item in Lst)
            {
                res += item.ToString() + '\n';
            }
            return res;
        }

        #region Transformation

        public PolyLine AngleX(float angle)
        {
            PolyLine temp = new PolyLine(this);
            for (int i = 0; i < temp.Lst.Count; i++)
            {
                temp.Lst[i] = MatrixManager.OnAngleX(temp.Lst[i], angle);
            }
            return temp;
        }

        public PolyLine AngleY(float angle)
        {
            PolyLine temp = new PolyLine(this);
            for (int i = 0; i < temp.Lst.Count; i++)
            {
                temp.Lst[i] = MatrixManager.OnAngleY(temp.Lst[i], angle);
            }
            return temp;
        }

        public PolyLine AngleZ(float angle)
        {
            PolyLine temp = new PolyLine(this);
            for (int i = 0; i < temp.Lst.Count; i++)
            {
                temp.Lst[i] = MatrixManager.OnAngleZ(temp.Lst[i], angle);
            }
            return temp;
        }

        public PolyLine MoveOnVector(float x, float y, float z)
        {
            PolyLine temp = new PolyLine(this);
            for (int i = 0; i < temp.Lst.Count; i++)
            {
                temp.Lst[i] = MatrixManager.MoveOnVector(temp.Lst[i], x, y, z);
            }
            return temp;
        }

        public PolyLine Isometric(float angleX, float angleY, float angleZ)
        {
            PolyLine temp = new PolyLine(this);
            temp = temp.AngleX(angleX);
            temp = temp.AngleY(angleY);
            temp = temp.AngleZ(angleZ);
            return temp;
        }

        #endregion
    }
}
