using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Graphic2.Library;

namespace Graphic2
{
    public partial class Form1 : Form
    {
        private Surface privSurf;

        private List<Point3D> pointsCurve1;
        private List<Point3D> pointsCurve2;
        private string function1 = "1";
        private string function2 = "2";
        public Surface MySurface
        {
            get
            {
                return privSurf;
            }
            set
            {
                privSurf = value;
                UpdateInfo();
            }
        }

        private void UpdateInfo()
        {
            richTextBox1.Text = MySurface.Lc1.ToString();
            richTextBox2.Text= MySurface.Lc2.ToString();
        }

        public float AngleX
        {
            get
            {
                return trackBarX.Value;
            }
        }

        public float AngleY
        {
            get
            {
                return trackBarY.Value;
            }
        }

        public float AngleZ
        {
            get
            {
                return trackBarZ.Value;
            }
        }

        public int Density
        {
            get
            {
                if (int.TryParse(textBox1.Text, out int res))
                {
                    return res;
                }
                else
                {
                    return 10;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            createCurves();
            //MySurface = new Surface(new PolyLine(new[] { new Point3D(0, 0, 0), new Point3D(0, 100, 0) }), new PolyLine(new[] { new Point3D(100, 0, 0), new Point3D(0, 0, 0), new Point3D(0, 0, 100) }), 20);
            MySurface = new Surface(new PolyLine(this.pointsCurve1), new PolyLine(this.pointsCurve2), 20);
            
        }
        private void createCurves()
        {
            this.pointsCurve1 = new List<Point3D>();
            this.pointsCurve2 = new List<Point3D>();
            Random random = new Random();
            List<int> ts = new List<int>();
            List<double> ts2 = new List<double>();
            int numOfPoints = 10;
            for (int i = 0; i < numOfPoints; i++)
            {
                ts.Add(i + 2);
                ts2.Add(i + 1);
                Point3D temp1;
                Point3D temp2;
                switch (function1)
                {
                    case "1":
                        temp1 = new Point3D(-(float)Math.Pow(ts[i], 2), -(float)Math.Pow(ts[i], 3), -(float)Math.Sqrt(ts[i]));
                        this.pointsCurve1.Add(temp1);
                        break;
                    case "2":
                        temp1 = new Point3D(-(float)Math.Pow(ts[i], 2), -(float)Math.Pow(ts[i], 3), -(float)Math.Pow(ts[i], 3));
                        this.pointsCurve1.Add(temp1);
                        break;
                }
                switch (function2)
                {
                    case "1":
                        temp2 = new Point3D((float)Math.Pow(ts2[i], 2), (float)Math.Pow(ts2[i], 3), (float)Math.Sqrt(ts2[i]));
                        this.pointsCurve2.Add(temp2);
                        break;
                    case "2":
                        temp2 = new Point3D((float)Math.Pow(ts2[i], 2), (float)Math.Pow(ts2[i], 3), (float)Math.Pow(ts2[i], 3));
                        this.pointsCurve2.Add(temp2);
                        break;
                }
                //temp1 = new Point3D((float)Math.Pow(ts[i], 2) * 10, (float)Math.Sqrt(ts[i]) * 10, (float)Math.Sqrt(ts[i]) * 10);
                //this.pointsCurve1.Add(temp1);
                //temp2 = new Point3D((float)(2 * ts2[i] * 10), (float)Math.Sqrt(ts2[i]) * 10, (float)Math.Pow(ts2[i], 2) * 10);
                //this.pointsCurve2.Add(temp2);
            }
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black, 2);
            Point3D mover = new Point3D(panel1.Width / 2, panel1.Height / 2, 0);

            g.Clear(Color.White);


            #region OXYZ
            p.Color = Color.Yellow;
            g.DrawLine(p, (PointF)MatrixManager.Isometric(new Point3D(panel1.Width, 0, 0), AngleX, AngleY, AngleZ, mover), (PointF)MatrixManager.Isometric(new Point3D(-panel1.Width, 0, 0), AngleX, AngleY, AngleZ, mover));

            p.Color = Color.Brown;
            g.DrawLine(p, (PointF)MatrixManager.Isometric(new Point3D(0, panel1.Width, 0), AngleX, AngleY, AngleZ, mover), (PointF)MatrixManager.Isometric(new Point3D(0, -panel1.Width, 0), AngleX, AngleY, AngleZ, mover));

            p.Color = Color.DeepPink;
            g.DrawLine(p, (PointF)MatrixManager.Isometric(new Point3D(0, 0, panel1.Width), AngleX, AngleY, AngleZ, mover), (PointF)MatrixManager.Isometric(new Point3D(0, 0, -panel1.Width), AngleX, AngleY, AngleZ, mover));
            #endregion


            //135 120, 0 rad            

            if (radioButton1.Checked)
            {
                MySurface.OnYZ().Draw(g, p, AngleX, AngleY, AngleZ, mover);
            }
            else if(radioButton2.Checked)
            {
                MySurface.OnXZ().Draw(g, p, AngleX, AngleY, AngleZ, mover);
            }
            else if(radioButton3.Checked)
            {
                MySurface.OnXY().Draw(g, p, AngleX, AngleY, AngleZ, mover);
            }
            else
            {
                MySurface.Draw(g, p, AngleX, AngleY, AngleZ, mover);
            }
          
        }       

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBarX.Value = 210;
            trackBarY.Value = 45;
            trackBarZ.Value = 22;
        }

        #region Track&Write

        private void TrackBarX_Scroll(object sender, EventArgs e)
        {
            textBoxX.Text = trackBarX.Value.ToString();
            this.Panel1_Paint(sender, new PaintEventArgs(panel1.CreateGraphics(),panel1.DisplayRectangle));
        }

        private void TrackBarY_Scroll(object sender, EventArgs e)
        {
            textBoxY.Text = trackBarY.Value.ToString();
            this.Panel1_Paint(sender, new PaintEventArgs(panel1.CreateGraphics(), panel1.DisplayRectangle));
        }

        private void TrackBarZ_Scroll(object sender, EventArgs e)
        {
            textBoxZ.Text = trackBarZ.Value.ToString();
            this.Panel1_Paint(sender, new PaintEventArgs(panel1.CreateGraphics(), panel1.DisplayRectangle));
        }

        private void TextBoxX_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxX.Text, out int res) && res <= 360 && res >= 0)
            {
                trackBarX.Value = res;
            }
            else if (textBoxX.Text == string.Empty)
            {
                trackBarX.Value = 0;
            }
        }

        private void TextBoxY_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxY.Text, out int res) && res <= 360 && res >= 0)
            {
                trackBarY.Value = res;
            }
            else if (textBoxY.Text == string.Empty)
            {
                trackBarY.Value = 0;
            }
        }

        private void TextBoxZ_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxZ.Text, out int res) && res <= 360 && res >= 0)
            {
                trackBarZ.Value = res;
            }
            else if(textBoxZ.Text==string.Empty)
            {
                trackBarZ.Value = 0;
            }
        }

        #endregion

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.Panel1_Paint(sender, new PaintEventArgs(panel1.CreateGraphics(), panel1.DisplayRectangle));
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) { }

        private void ButtonDraw_Click(object sender, EventArgs e)
        {
            try
            {
                function1 = richTextBox3.Text;
                function2 = richTextBox4.Text;
                createCurves();
                MySurface = new Surface(new PolyLine(this.pointsCurve1), new PolyLine(this.pointsCurve2), Density);
                this.Panel1_Paint(sender, new PaintEventArgs(panel1.CreateGraphics(), panel1.DisplayRectangle));
            }
            catch (Exception)
            {

            }
            
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            function1 = richTextBox3.Text;
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            function2 = richTextBox4.Text;
        }
    }
}
