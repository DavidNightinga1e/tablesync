using Photon.Pun;
using UnityEngine.SceneManagement;

namespace TableSync
{
    public class GameCallbacks : MonoBehaviourPunCallbacks
    {
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}