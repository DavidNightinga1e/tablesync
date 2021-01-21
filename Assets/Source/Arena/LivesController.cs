using System;
using UnityEngine;
using UnityEngine.UI;

namespace TableSync
{
    [RequireComponent(typeof(RectTransform))]
    public class LivesController : MonoBehaviour
    {
        [SerializeField] private Color red;
        [SerializeField] private Color black;
        [SerializeField] private Image heart1;
        [SerializeField] private Image heart2;

         public Player player;
         public Camera mainCamera;

        private void Update()
        {
            var rectTransform = (RectTransform) transform;
            rectTransform.position = mainCamera.WorldToScreenPoint(player.transform.position);

            print($"player {player.playerColorType.ToString()} - {player.Lives}");
            heart1.color = player.Lives > 2 ? red : black;
            heart2.color = player.Lives > 1 ? red : black;
        }
    }
}