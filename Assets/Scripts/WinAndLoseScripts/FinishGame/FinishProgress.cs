using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishProgress : MonoBehaviour
{
    [SerializeField] private GameUIManager gameUI;
    [SerializeField] private CarMovement carMovScript;

    public Transform player;

    public float finishDistance = 100f;

    public Slider progressBar;
    public TMP_Text progressText; // TMP текст

    private float startZ;
    private bool isFinished = false;

    void Start()
    {
        startZ = player.position.z;

        if (progressBar != null)
            progressBar.value = 0f;

        if (progressText != null)
            progressText.text = "0 / " + finishDistance + " m";
    }

    void Update()
    {
        if (isFinished) return;

        float distanceTravelled = player.position.z - startZ;
        distanceTravelled = Mathf.Clamp(distanceTravelled, 0, finishDistance);

        float progress = distanceTravelled / finishDistance;

        if (progressBar != null)
            progressBar.value = progress;

        if (progressText != null)
        {
            int current = Mathf.FloorToInt(distanceTravelled);
            int total = Mathf.FloorToInt(finishDistance);
            progressText.text = current + " " + "m";
        }

        if (progress >= 1f)
        {
            Finish();
        }
    }

    void Finish()
    {
        isFinished = true;

        gameUI.ShowWinPanel();
        carMovScript.enabled = false;

        Debug.Log("ПОБЕДА");
    }
}