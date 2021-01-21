using System;
using UnityEngine;

namespace TableSync
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform bluePosition;
        [SerializeField] private Transform orangePosition;
        [SerializeField] private Transform middlePosition;
        [SerializeField] private Camera targetCamera;

        public void SetBlue()
        {
            targetCamera.transform.position = bluePosition.position;
            targetCamera.transform.rotation = bluePosition.rotation;
        }

        public void SetOrange()
        {
            targetCamera.transform.position = orangePosition.position;
            targetCamera.transform.rotation = orangePosition.rotation;
        }

        public void SetMiddle()
        {
            targetCamera.transform.position = middlePosition.position;
            targetCamera.transform.rotation = middlePosition.rotation;
        }

        public void SetPlayerColor(PlayerColor playerColor)
        {
            switch (playerColor)
            {
                case PlayerColor.Blue:
                    SetBlue();
                    break;
                case PlayerColor.Orange:
                    SetOrange();
                    break;
                default:
                    SetMiddle();
                    break;
            }
        }
    }
}