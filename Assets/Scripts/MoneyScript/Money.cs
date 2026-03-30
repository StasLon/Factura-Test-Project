using TMPro;
using UnityEngine;
using System;

public class Money : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private int _gold;

    public event Action<int> Changed;

    public void Add(int amount)
    {
        _gold += amount;
        Changed?.Invoke(_gold);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (text != null)
            text.text = _gold.ToString();
    }
}