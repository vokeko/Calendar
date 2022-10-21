using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            rcColorShow.Fill = EventList.HighlightBrush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Shapes.Rectangle rec)
            {
                Point point = e.GetPosition(rec);
                System.Drawing.Color col = GetColor(point, rec);

                if (this.Resources.Contains("CurrentColor"))
                    this.Resources["CurrentColor"] = Color.FromRgb(col.R, col.G, col.B);
            }
        }
        private void Saturation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Shapes.Rectangle rec)
            {
                Point point = e.GetPosition(rec);
                System.Drawing.Color col = GetColor(point, rec);
                PickedColor = "#" + col.Name.Substring(2).ToUpper();
                txtColor.Text = PickedColor;
                rcColorShow.Fill = new SolidColorBrush(Color.FromRgb(col.R, col.G, col.B));
            }
        }

        private System.Drawing.Color GetColor(Point point, System.Windows.Shapes.Rectangle rec)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1, 1);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            Point screenP = rec.PointToScreen(point);
            g.CopyFromScreen((int)screenP.X, (int)screenP.Y, 0, 0, new System.Drawing.Size(1, 1));
            System.Drawing.Color ret = bmp.GetPixel(0, 0);
            bmp.Dispose();

            return ret;
        }
    }
}
