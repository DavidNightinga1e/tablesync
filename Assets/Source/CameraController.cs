using UnityEngine;

namespace TableSync.Demo
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
    }
}