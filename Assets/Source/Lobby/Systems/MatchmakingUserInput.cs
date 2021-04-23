using Simba;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Lobby.Systems
{
    public class MatchmakingUserInput : SimbaComponent
    {
        [SerializeField] private TMP_InputField privateRoomNameText;
        [SerializeField] private Button quickSearchButton;
        [SerializeField] private Button joinOrCreatePrivateRoomButton;

        public TMP_InputField PrivateRoomNameText => privateRoomNameText;
        public Button JoinOrCreatePrivateRoomButton => joinOrCreatePrivateRoomButton;
        public Button QuickSearchButton => quickSearchButton;
        
        public override void OnAwake()
        {
        }

        public override void OnOnDestroy()
        {
        }
    }
}