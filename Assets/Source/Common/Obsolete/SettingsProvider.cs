using UnityEngine;

namespace TableSync
{
    public static class SettingsProvider
    {
        public static string Nickname
        {
            get => PlayerPrefs.GetString(nameof(Nickname));
            set => PlayerPrefs.SetString(nameof(Nickname), value);
        }
    }
}