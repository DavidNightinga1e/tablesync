using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TableSync.Demo
{
    public class GameServer : MonoBehaviour
    {
        public Button createServerButton;
        public RectTransform menu;
        public TextMeshProUGUI logger;

        private GameController _gameController;

        private NetworkingClient _blue;
        private NetworkingClient _orange;
        private NetworkingServer _server;
        
        private void Awake()
        {
            createServerButton.onClick.AddListener(CreateHost);
            _gameController = FindObjectOfType<GameController>();
        }

        public void CreateHost()
        {
            _server = new NetworkingServer();
            menu.gameObject.SetActive(false);
            logger.gameObject.SetActive(true);
            StartCoroutine(Host());
        }

        IEnumerator Host()
        {
            logger.text += "\n\nServerNetworkingController - Waiting for connections...";
            yield return _server.WaitForClientToConnect();
            logger.text += $"Connected - {_server.Clients.Last().Id}";
            yield return _server.WaitForClientToConnect();
            logger.text += $"Connected - {_server.Clients.Last().Id}";

            _blue = _server.Clients[0];
            _orange = _server.Clients[1];

            while ()
            {
                
            }
        }
    }
}