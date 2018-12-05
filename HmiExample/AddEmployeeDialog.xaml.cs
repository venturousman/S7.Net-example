using log4net;
using Microsoft.Win32;
using ProductionEquipmentControlSoftware.Helpers;
using ProductionEquipmentControlSoftware.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProductionEquipmentControlSoftware
{
    /// <summary>
    /// Interaction logic for AddEmployeeDialog.xaml
    /// </summary>
    public partial class AddEmployeeDialog : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AddEmployeeDialog()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png";
                if (openFileDialog.ShowDialog() == true)
                {
                    var filePath = openFileDialog.FileName;

                    txtPhotoPath.Text = filePath;

                    //ImageSourceConverter isc = new ImageSourceConverter();
                    //imgPhoto.SetValue(Image.SourceProperty, isc.ConvertFromString(filePath));


                    //imgPhoto.Source = new BitmapImage(new Uri(filePath));


                    //byte[] bytes = File.ReadAllBytes(filePath); // ok, auto close file


                    //Initialize a file stream to read the image file
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    //Initialize a byte array with size of stream
                    byte[] imgByteArr = new byte[fs.Length];

                    //Read data from the file stream and put into the byte array
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                    // Create a BitmapSource
                    fs.Seek(0, SeekOrigin.Begin);
                    BitmapImage imgsrc = new BitmapImage();
                    imgsrc.BeginInit();
                    imgsrc.CacheOption = BitmapCacheOption.OnLoad;
                    imgsrc.StreamSource = fs;
                    imgsrc.EndInit();

                    // Set Image.Source 
                    imgPhoto.Source = imgsrc;

                    //Close a file stream
                    fs.Close();

                    // set to datacontext
                    if (DataContext is EmployeeViewModel)
                    {
                        var context = DataContext as EmployeeViewModel;
                        context.PhotoContent = imgByteArr;
                        context.Photo = filePath;
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.GetAllExceptionInfo();
                log.Error(msg, ex);
                MessageBox.Show("Cannot import image", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddEmployeeDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EmployeeViewModel)
            {
                var context = DataContext as EmployeeViewModel;

                // Set Image.Source 
                imgPhoto.Source = context.PhotoImage;

                if (!string.IsNullOrEmpty(context.Photo))
                {
                    txtPhotoPath.Text = context.Photo.Trim();
                }

                /*
                if (context.PhotoContent != null && context.PhotoContent.Length > 0)
                {
                    //Initialize a memory stream from bytes
                    MemoryStream ms = new MemoryStream(context.PhotoContent);

                    // Create a BitmapSource
                    BitmapImage imgsrc = new BitmapImage();
                    imgsrc.BeginInit();
                    imgsrc.CacheOption = BitmapCacheOption.OnLoad;
                    imgsrc.StreamSource = ms;
                    imgsrc.EndInit();

                    // Set Image.Source 
                    imgPhoto.Source = imgsrc;

                    //Close a memory stream
                    ms.Close();
                }
                else
                {
                    if (!string.IsNullOrEmpty(context.Photo) && File.Exists(context.Photo.Trim()))
                    {
                        var filePath = context.Photo.Trim();

                        //Initialize a file stream to read the image file
                        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                        // Create a BitmapSource
                        BitmapImage imgsrc = new BitmapImage();
                        imgsrc.BeginInit();
                        imgsrc.CacheOption = BitmapCacheOption.OnLoad;
                        imgsrc.StreamSource = fs;
                        imgsrc.EndInit();

                        // Set Image.Source 
                        imgPhoto.Source = imgsrc;

                        //Close a file stream
                        fs.Close();
                    }
                }
                */
            }
        }
    }
}
