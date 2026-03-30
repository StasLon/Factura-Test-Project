using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public int currentGold = 0;

    public TMP_Text goldText;

    void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = currentGold.ToString();
        }
    }
}