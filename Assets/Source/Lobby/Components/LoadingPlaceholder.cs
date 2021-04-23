using Simba;
using UnityEngine;

namespace Source.Lobby.Components
{
    public class LoadingPlaceholder : SimbaComponent
    {
        private CanvasGroup _canvasGroup;

        public bool IsVisible
        {
            set
            {
                _canvasGroup.alpha = value ? 1 : 0;
                _canvasGroup.interactable = value;
                _canvasGroup.blocksRaycasts = value;
            }
        }

        public override void OnAwake()
        {
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        public override void OnOnDestroy()
        {
        }
    }
}