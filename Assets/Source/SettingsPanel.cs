using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TableSync.Demo
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nicknameInputField;
        [SerializeField] private Toggle isBlueToggle;
        [SerializeField] private Toggle isOrangeToggle;

        private LocalSettingsProvider _localSettingsProvider;

        private void Awake()
        {
            _localSettingsProvider = FindObjectOfType<LocalSettingsProvider>();
            _localSettingsProvider.LoadFromDisk();

            nicknameInputField.text = _localSettingsProvider.settings.nickname;
            if (_localSettingsProvider.settings.isPreferedColorBlue)
                isBlueToggle.isOn = true;
            else
                isOrangeToggle.isOn = true;

            nicknameInputField.onEndEdit.AddListener(str => _localSettingsProvider.settings.nickname = str);
            isBlueToggle.onValueChanged.AddListener(b => _localSettingsProvider.settings.isPreferedColorBlue = b);
        }
    }
}