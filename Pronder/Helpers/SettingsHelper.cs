using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static Pronder.Classes.SaveData;

namespace Pronder.Helpers;
internal class SettingsHelper
{
    private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

    private readonly Dictionary<string, object> defaultSettings = new Dictionary<string, object>
    {
        { "AppTheme", "Default" },
        { "PaneProjectsSorted", 0}
    };

    public SettingsHelper()
    {

    }


    public void WriteTo(string key, object value)
    {
        localSettings.Values[key] = value;
    }

    public object Read(string key)
    {
        if (localSettings.Values.ContainsKey(key))
        {
            return localSettings.Values[key];
        }
        return defaultSettings.ContainsKey(key) ? defaultSettings[key] : default;
    }

    public void RemoveIfExists(string key)
    {
        if (localSettings.Values.ContainsKey(key))
        {
            localSettings.Values.Remove(key);
        }
    }
}
