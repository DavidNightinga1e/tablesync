using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI local;
    [SerializeField] private TextMeshProUGUI remote;
    [SerializeField] private TextMeshProUGUI localScore;
    [SerializeField] private TextMeshProUGUI remoteScore;

    public string Local
    {
        get => local.text;
        set => local.text = value;
    }

    public string Remote
    {
        get => remote.text;
        set => remote.text = value;
    }

    public string LocalScore
    {
        get => localScore.text;
        set => localScore.text = value;
    }

    public string RemoteScore
    {
        get => remoteScore.text;
        set => remoteScore.text = value;
    }
}