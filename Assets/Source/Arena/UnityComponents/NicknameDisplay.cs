using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI local;
    [SerializeField] private TextMeshProUGUI remote;

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
}