using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace SpaceShooterGame
{
    public static class AppSettingsHelper
    {
        public static AppSettings AppSettings;

        public static async Task LoadAppSettings()
        {
            if (AppSettings is null)
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///appsettings.json"));
                var appsettingsJson = await FileIO.ReadTextAsync(file);
                AppSettings = JsonConvert.DeserializeObject<AppSettings>(appsettingsJson);

#if DEBUG
                AppSettings = new AppSettings() { BackendApiBaseUrl = "https://localhost:7238" };
#endif          
            }
        }
    }

    public class AppSettings
    {
        public string BackendApiBaseUrl { get; set; }
    }
}
