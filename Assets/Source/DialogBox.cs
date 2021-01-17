using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TableSync.Demo
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private RectTransform container;

        public Button CancelButton => cancelButton;
        public Button YesButton => yesButton;
        public Button NoButton => noButton;
        public TextMeshProUGUI Text => text;
        public RectTransform Container => container;

        public bool IsVisible
        {
            get => container.gameObject.activeSelf;
            set => container.gameObject.SetActive(value);
        }
        
        public bool CancelButtonVisible
        {
            get => cancelButton.gameObject.activeSelf;
            set => cancelButton.gameObject.SetActive(value);
        }       
        
        public bool YesButtonVisible
        {
            get => yesButton.gameObject.activeSelf;
            set => yesButton.gameObject.SetActive(value);
        }

        public bool NoButtonVisible
        {
            get => noButton.gameObject.activeSelf;
            set => noButton.gameObject.SetActive(value);
        }

        public void RemoveAllListeners()
        {
            cancelButton.onClick.RemoveAllListeners();
            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
        }
    }
}