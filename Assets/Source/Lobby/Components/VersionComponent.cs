using Simba;
using TMPro;
using UnityEngine;

namespace Source.Lobby.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VersionComponent : SimbaComponent
    {
        private TextMeshProUGUI _textMeshPro;

        public string Text
        {
            get => _textMeshPro.text;
            set => _textMeshPro.text = value;
        }
    
        public override void OnAwake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        public override void OnOnDestroy()
        {
        }
    }
}
