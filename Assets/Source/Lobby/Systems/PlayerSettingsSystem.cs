using Simba;
using TableSync;
using TableSync.Demo;

namespace Source.Lobby.Systems
{
    public class PlayerSettingsSystem : IStartSystem
    {
        public void Start()
        {
            var playerSettingsComponent = SimbaComponent.Get<PlayerSettingsComponent>();

            playerSettingsComponent.NicknameInputField.text = SettingsProvider.Nickname;

            playerSettingsComponent.NicknameInputField.onEndEdit.AddListener(str => SettingsProvider.Nickname = str);
        }
    }
}