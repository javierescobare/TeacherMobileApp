using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace TeacherMobileApp.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string IsLoggedIn = "loggedIn";
        private static readonly bool SettingsDefault = false;

        #endregion


        public static bool LoggedIn
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsLoggedIn, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsLoggedIn, value);
            }
        }

    }
}