using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace AstroOdyssey
{
    public static class AuthCredentialsCacheHelper
    {
        public static PlayerAuthCredentials GetCachedAuthCredentials()
        {
            // if user was already logged in or came here after sign up
            if (App.AuthCredentials is not null)
            {
                App.AuthCredentials.Password = App.AuthCredentials.Password.Decrypt();
                return App.AuthCredentials;
            }
            //else
            //{
            //    ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //    var localValue = localSettings.Values["AuthCredentials"] as string;

            //    if (!localValue.IsNullOrBlank())
            //    {
            //        var authCredentials = JsonConvert.DeserializeObject<PlayerAuthCredentials>(localValue);
            //        if (authCredentials is not null)
            //        {
            //            authCredentials.Password = authCredentials.Password.Decrypt();
            //            return authCredentials;
            //        }
            //    }
            //}

            //TODO: get cookie

            return null;
        }

        public static void SetCachedAuthCredentials(string userName, string password)
        {
            // store auth credentials
            App.AuthCredentials = new PlayerAuthCredentials(
                userName: userName,
                password: password.Encrypt());

            //TODO: set cookie

            //// save in browser cache
            //ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //localSettings.Values["AuthCredentials"] = JsonConvert.SerializeObject(App.AuthCredentials);
        }
    }
}
