using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProductionEquipmentControlSoftware
{
    /// <summary>
    /// Interaction logic for AddMachineDialog.xaml
    /// </summary>
    public partial class AddMachineDialog : UserControl
    {
        public AddMachineDialog()
        {
            InitializeComponent();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
