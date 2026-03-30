using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishProgress : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float finishDistance = 100f;

    [SerializeField] private Slider bar;
    [SerializeField] private TMP_Text text;

    public event System.Action Finished;

    private float _startZ;
    private bool _done;

    private void Awake()
    {
        _startZ = player.position.z;
        UpdateUI(0);
    }

    private void Update()
    {
        if (_done) return;

        float dist = Mathf.Clamp(player.position.z - _startZ, 0, finishDistance);
        float progress = dist / finishDistance;

        UpdateUI(dist);

        if (progress >= 1f)
        {
            _done = true;
            Finished?.Invoke();
        }
    }

    private void UpdateUI(float dist)
    {
        bar.value = dist / finishDistance;
        text.text = $"{Mathf.FloorToInt(dist)} m";
    }
}