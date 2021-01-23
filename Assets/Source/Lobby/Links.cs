using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TableSync
{
    public class Links : MonoBehaviour
    {
        private const string FeedbackLink = "https://forms.gle/rLGpvdNq4gaLDG6R7";
        private const string DownloadLink = "https://github.com/DavidNightinga1e/tablesync/releases";

        [SerializeField] private Button feedbackButton;
        [SerializeField] private Button downloadButton;

        private void Awake()
        {
            feedbackButton.onClick.AddListener(() => Application.OpenURL(FeedbackLink));
            downloadButton.onClick.AddListener(() => Application.OpenURL(DownloadLink));
        }
    }
}