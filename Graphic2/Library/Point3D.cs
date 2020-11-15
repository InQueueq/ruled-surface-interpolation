using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic2.Library
{
    public class Point3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        private Point3D() { }

        public Point3D(PointF point, float Z = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Point3D(Point3D point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
            this.W = point.W;
        }

        public Point3D(float X, float Y, float Z = 0, float W = 1)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        public static explicit operator PointF(Point3D point)
        {
            return new PointF(point.X / point.W, point.Y / point.W);           
        }

        public override string ToString()
        {
            return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
        }

        public static explicit operator Point3D(string str)
        {
            string[] temp = str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length != 3)
                throw new ArgumentException();
            else
                return new Point3D((float)Convert.ToDouble(temp[0]), (float)Convert.ToDouble(temp[1]), (float)Convert.ToDouble(temp[2]));
        }

        public bool Equals(Point3D obj)
        {
            return this.X == obj.X && this.Y == obj.Y && this.Z == obj.Z && this.W == obj.W;
        }

        public static float GetLength(Point3D a, Point3D b)
        {
            return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2) + Math.Pow(b.Z - a.Z, 2));
        }

        public static Point3D MoveOnLength(Point3D start, Point3D end, float length)
        {            
            float L = Point3D.GetLength(start, end);
            // Нормалізація і перехід до однорідних координат
            Point3D vector = new Point3D((end.X - start.X) / L * length, (end.Y - start.Y) / L * length, (end.Z - start.Z) / L * length); 
            return MatrixManager.MoveOnVector(start, vector.X, vector.Y, vector.Z);
        }
    }
}
