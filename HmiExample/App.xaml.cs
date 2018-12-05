using log4net;
using ProductionEquipmentControlSoftware.Helpers;
using System;
using System.IO;
using System.Windows;

namespace ProductionEquipmentControlSoftware
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Set the file name and get the output directory
            var baseDirectory = @"C:\" + Constants.ApplicationName + @"\logs\";

            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            var currentDate = DateTime.Now;
            var fileName = "Log-" + currentDate.ToString("yyyy-MM-dd") + ".txt";
            var filePath = Path.Combine(baseDirectory, fileName);

            log4net.GlobalContext.Properties["LogFileName"] = filePath; //log file path
            log4net.Config.XmlConfigurator.Configure();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            // Process unhandled exception
            var msg = e.Exception.GetAllExceptionInfo();
            log.Error(msg, e.Exception);

            // Prevent default unhandled exception processing
            e.Handled = true;

            MessageBoxResult messageBoxResult =
                MessageBox.Show(Constants.ApplicationCommonErrorMessage + ", the application will close immediately", Constants.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                Environment.Exit(1);
            }
        }
    }
}
