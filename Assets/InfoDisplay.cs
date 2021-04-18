using TMPro;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI playerName;

    public string EnemyName
    {
        get => enemyName.text;
        set => enemyName.text = value;
    }

    public string PlayerName
    {
        get => playerName.text;
        set => playerName.text = value;
    }
}