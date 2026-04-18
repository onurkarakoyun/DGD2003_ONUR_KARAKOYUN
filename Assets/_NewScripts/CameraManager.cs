using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Kameralar")]
    public CinemachineCamera vcamPlayer;
    public CinemachineCamera vcamPanel;
    public CinemachineCamera vcamExit;

    [Header("Oyuncu Kontrolü")]
    public FPSController playerController;
    void Start()
    {
        ActivatePlayerCamera();
    }
    public void ActivatePanelCamera()
    {
        SetCamerasPriority(0, 10, 0);
        playerController.enabled = false; 
    }
    public void ActivateExitCamera()
    {
        SetCamerasPriority(0, 0, 10);
        playerController.enabled = false; 
    }
    public void ActivatePlayerCamera()
    {
        SetCamerasPriority(10, 0, 0);
        playerController.enabled = true; 
    }
    private void SetCamerasPriority(int playerPriority, int panelPriority, int exitPriority)
    {
        vcamPlayer.Priority = playerPriority;
        vcamPanel.Priority = panelPriority;
        vcamExit.Priority = exitPriority;
    }
}