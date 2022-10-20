using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        public string PickedColor { get; private set; }
        public ColorPicker()
        {
            InitializeComponent();
            this.txtColor.Text = EventList.HighlightBrush.ToString().Remove(1, 2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Shapes.Rectangle rec)
            {
                System.Windows.Point point = e.GetPosition(rec);
                System.Drawing.Color col = GetColor(point, rec);

                if (this.Resources.Contains("CurrentColor"))
                    this.Resources["CurrentColor"] = System.Windows.Media.Color.FromRgb(col.R, col.G, col.B);
            }
        }
        private void Saturation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Shapes.Rectangle rec)
            {
                System.Windows.Point point = e.GetPosition(rec);
                System.Drawing.Color col = GetColor(point, rec);
                PickedColor = "#" + col.Name.Substring(2).ToUpper();
                txtColor.Text = PickedColor;
            }
        }

        private System.Drawing.Color GetColor(System.Windows.Point point, System.Windows.Shapes.Rectangle rec)
        {
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            System.Windows.Point screenP = rec.PointToScreen(point);
            g.CopyFromScreen((int)screenP.X, (int)screenP.Y, 0, 0, new System.Drawing.Size(1, 1));
            System.Drawing.Color ret = bmp.GetPixel(0, 0);
            bmp.Dispose();

            return ret;
        }
    }
}
