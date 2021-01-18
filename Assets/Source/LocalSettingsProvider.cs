using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TableSync.Demo
{
    public class LocalSettingsProvider : MonoBehaviour
    {
        [HideInInspector] public LocalSettings settings;

        private string PathToSettings => Path.Combine(Application.persistentDataPath, "UserSettings.bin");

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SaveToDisk()
        {
            using (var write = new StreamWriter(PathToSettings))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(write.BaseStream, settings);
            }
        }

        public void LoadFromDisk()
        {
            using (var read = new StreamReader(PathToSettings))
            {
                var bf = new BinaryFormatter();
                settings = (LocalSettings) bf.Deserialize(read.BaseStream);
            }
        }

        private void OnDisable()
        {
            SaveToDisk();
        }
    }
}