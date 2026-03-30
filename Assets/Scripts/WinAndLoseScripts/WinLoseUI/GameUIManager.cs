using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CarMovement car;
    [SerializeField] private CarHealth playerHealth;
    [SerializeField] private FinishProgress progress;
    [SerializeField] private CarMovement carMovement;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private TurretController turretController;
    [SerializeField] private TurretShooting turretShooting;


    private void Awake()
    {
        playerHealth.Died += ShowLose;
        progress.Finished += OnFinished;
    }
    private void OnFinished()
    {
        ShowWin();
    }
    public void ShowWin()
    {
        winPanel?.SetActive(true);
        car.Disable();

        turretController.Disable();
        turretShooting.Disable();

        spawner?.StopAllEnemies();
    }

    public void ShowLose()
    {
        losePanel?.SetActive(true);
        car.Disable();

        turretController.Disable();
        turretShooting.Disable();

        spawner?.StopAllEnemies();
    }

    public void Restart()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}