using System.Configuration;

namespace HmiExample.Helpers
{
    public class SettingHelpers
    {
        public static bool hasSetting(string name)
        {
            bool found = false;
            foreach (SettingsProperty currentProperty in Properties.Settings.Default.Properties)
            {
                if (currentProperty.Name == name)
                {
                    found = true;
                }
            }
            return found;
        }
    }
}
