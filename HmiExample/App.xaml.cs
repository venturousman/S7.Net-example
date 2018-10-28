using System;
using System.IO;
using System.Windows;

namespace HmiExample
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
    }
}
