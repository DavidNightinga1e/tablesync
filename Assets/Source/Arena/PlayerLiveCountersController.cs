using System;
using TMPro;
using UnityEngine;

namespace TableSync
{
    public class PlayerLiveCountersController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TextMeshProUGUI blueLivesText;
        [SerializeField] private TextMeshProUGUI orangeLivesText;

        private void Awake()
        {
            gameManager.OnBluePlayerLivesUpdate += GameManagerOnOnBluePlayerLivesUpdate;
            gameManager.OnOrangePlayerLivesUpdate += GameManagerOnOnOrangePlayerLivesUpdate;
        }

        private void OnDisable()
        {
            gameManager.OnBluePlayerLivesUpdate -= GameManagerOnOnBluePlayerLivesUpdate;
            gameManager.OnOrangePlayerLivesUpdate -= GameManagerOnOnOrangePlayerLivesUpdate;
        }
        
        private void GameManagerOnOnBluePlayerLivesUpdate(int val)
        {
            blueLivesText.text = $"{val} lives";
        }
        
        private void GameManagerOnOnOrangePlayerLivesUpdate(int val)
        {
            orangeLivesText.text = $"{val} lives";
        }
    }
}