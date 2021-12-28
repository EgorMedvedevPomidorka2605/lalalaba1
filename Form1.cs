using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using org.mariuszgromada.math.mxparser;
using ZedGraph;
namespace Laba1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void стартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Task(t1f1);
            t.Start();
        }
        public void t1f1()
        {
            try
            {
                GraphPane pane = zedGraphControl1.GraphPane;
                pane.CurveList.Clear();
                string x1 = Xmin.Text;
                double x_1 = Convert.ToDouble(x1);
                string x2 = Xmax.Text;
                double x_2 = Convert.ToDouble(x2);
                string func = function.Text;
                PointPairList list = new PointPairList();

                if (x_1 > x_2)
                {
                    throw new Exception();
                }

                for (double z = x_1; z <= x_2; z += 1)
                {
                    Argument x = new Argument("x = " + z);
                    Expression exp = new Expression(func, x);
                    if (double.IsNaN(exp.calculate()))
                    {
                        throw new Exception();
                    }
                    list.Add(z, exp.calculate());
                    LineItem myCurve = pane.AddCurve("", list, Color.Blue, SymbolType.None);
                }
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
            catch
            {
                MessageBox.Show("Ашибка");
            }
        }
        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            Xmin.Text = "";
            Xmax.Text = "";
            etextBox.Text = "";
            function.Text = "";
            textBox1.Text = "";
        }
        private void минимумToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Task(minf);
            t.Start();
        }
        private void minf()
        {
            try
            {
                GraphPane pane = zedGraphControl1.GraphPane;
                pane.CurveList.Clear();
                double eplition = Convert.ToDouble(etextBox.Text);
                double x1 = Convert.ToDouble(Xmin.Text);
                double x2 = Convert.ToDouble(Xmax.Text);
                string func = function.Text;
                double y1 = 0;
                double y2 = 0;
                double xm = 0;
                double ym = 0;

                Argument x = new Argument("x = " + x1);
                Expression exp = new Expression(func, x);
                y1 = exp.calculate();
                Debug.WriteLine("y1=" + y1);

                x = new Argument("x = " + x2);
                exp = new Expression(func, x);
                y2 = exp.calculate();
                Debug.WriteLine("y2=" + y2);

                while (Math.Abs(y1 - y2) > eplition)
                {
                    xm = (x1 + x2) / 2;
                    Debug.WriteLine("xm=" + xm);
                    x = new Argument("x = " + xm);
                    exp = new Expression(func, x);
                    ym = exp.calculate();
                    Debug.WriteLine("ym=" + ym);

                    if (y1 > y2)
                    {
                        x1 = xm;
                        Debug.WriteLine("y1>y2  x1=" + xm);
                    }
                    else
                    {
                        x2 = xm;
                        Debug.WriteLine("y1<y2  x2=" + xm);
                    }
                    
                    x = new Argument("x = " + x1);
                    exp = new Expression(func, x);
                    y1 = exp.calculate();
                    Debug.WriteLine("y1=" + y1 + " x1 ="+x1);

                    x = new Argument("x = " + x2);
                    exp = new Expression(func, x);
                    y2 = exp.calculate();
                    Debug.WriteLine("y2=" + y2 + " x2 =" + x2);
                }
                xm = (x1 + x2) / 2;
                x = new Argument("x = " + xm);
                exp = new Expression(func, x);
                ym = exp.calculate();
                Debug.WriteLine("ym=" + ym);
                Action text = () => textBox1.Text = String.Format("{0}, {1}", Math.Round(xm, etextBox.Text.Length - 2), Math.Round(ym, etextBox.Text.Length - 2));
                Invoke(text);
            }
            catch
            {
                MessageBox.Show("Ашибка");
            }
        }
        private void Xmin_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 45)
            {
                Xmin.MaxLength = 5;
                e.Handled = true;
            }
        }
        private void Xmax_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 45)
            {
                e.Handled = true;
            }
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void etextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44)
            {
                e.Handled = true;
            }
        }

        private void Xmin_TextChanged(object sender, EventArgs e)
        {
            Xmin.MaxLength = 4;
        }

        private void Xmax_TextChanged(object sender, EventArgs e)
        {
            Xmax.MaxLength = 4;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

