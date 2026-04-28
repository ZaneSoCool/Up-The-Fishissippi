using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour
{
    public player playerScript;
    public TextMeshProUGUI walletText;

    void Update()
    {
        if (playerScript == null) return;
        walletText.text = playerScript.coinsCount.ToString();
    }
}
