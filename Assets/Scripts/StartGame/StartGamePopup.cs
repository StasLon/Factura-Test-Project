using UnityEngine;
using UnityEngine.UI;

public class StartGamePopup : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject panel;

    [SerializeField] private CarMovement car;
    [SerializeField] private TurretController turret;
    [SerializeField] private TurretShooting shooting;

    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        car.Enable();
        turret.Enable();
        shooting.EnableWithDelay().Forget();

        panel.SetActive(false);
        gameObject.SetActive(false);
    }
}