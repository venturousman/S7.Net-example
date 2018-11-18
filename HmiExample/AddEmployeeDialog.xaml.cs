using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace HmiExample
{
    /// <summary>
    /// Interaction logic for AddEmployeeDialog.xaml
    /// </summary>
    public partial class AddEmployeeDialog : UserControl
    {
        public AddEmployeeDialog()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                txtPhotoPath.Text = openFileDialog.FileName;
            }
        }
    }
}
