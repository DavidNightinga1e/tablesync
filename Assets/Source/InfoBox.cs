using TMPro;
using UnityEngine;

namespace TableSync.Demo
{
    public class InfoBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        public TextMeshProUGUI Text => text;
    }
}