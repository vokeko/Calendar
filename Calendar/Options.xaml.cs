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
using Microsoft.Win32;

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
            cmbBackupFreq.ItemsSource = Enum.GetValues(typeof(EventList.BackupFrequency));
            cmbBackupFreq.SelectedIndex = 0;
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
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Pozice zálohování",
                DefaultExt = "bin",
                Filter = "Binary files (*.bin)|*.bin",
                CheckPathExists = true,
                FileName = "events.bin",
            };

            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                txtBackupPath.Text = saveFileDialog.FileName;
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            EventList.SetBrush(txtColor.Text);
            EventList.SetBackupInfo((EventList.BackupFrequency)cmbBackupFreq.SelectedItem, txtBackupPath.Text);
            
            this.Close();
        }
    }
}
