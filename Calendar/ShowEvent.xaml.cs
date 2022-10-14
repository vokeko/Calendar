using System.Windows;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for ShowEvent.xaml
    /// </summary>
    public partial class ShowEvent : Window
    {
        internal Event Value { get; }
        internal ShowEvent(Event prevEvent)
        {
            InitializeComponent();
            Value = prevEvent;
            Load();
        }

        private void Load()
        {
            txtName.Text = Value.Name;
            txtDesc.Text = Value.Description;
            txtDate.Text = Value.Date.ToString();
            chbRepeat.IsChecked = Value.Repeat;
        }
    }
}
