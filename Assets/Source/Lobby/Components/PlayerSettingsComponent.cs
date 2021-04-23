using Simba;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TableSync.Demo
{
    public class PlayerSettingsComponent : SimbaComponent
    {
        [SerializeField] private TMP_InputField nicknameInputField;

        public TMP_InputField NicknameInputField => nicknameInputField;
        
        public override void OnAwake()
        {
        }

        public override void OnOnDestroy()
        {
        }
    }
}