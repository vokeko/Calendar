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

namespace Calendar
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            EventList.LoadDataFrom();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            EventList.SaveDataAs();
        }
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            EventList.ExportData();
        }

        private void TxtBackupPath_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string txtbrush = txtColor.Text.Trim();
            if (txtbrush[0] != '#') txtbrush = txtbrush.Insert(0, "#");
            EventList.SetBrush(txtbrush);

            this.Close();
        }
    }
}
