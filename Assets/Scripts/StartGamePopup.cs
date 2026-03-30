using UnityEngine;
using UnityEngine.UI;

public class StartGamePopup : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private CarMovement carMovementScript;
    [SerializeField] private GameObject panel;
    [SerializeField] private TurretController turretControl;
    [SerializeField] private TurretShooting turretShootControl;

    private void Start()
    {
        startGameButton.onClick.AddListener(CloseStartGamePopup);
    }

    private void CloseStartGamePopup()
    {
        carMovementScript.canMove = true;
        turretControl.canMoveTurret = true;
        gameObject.SetActive(false);
        panel.SetActive(false);
        turretShootControl.EnableShooting();
    }

}
