using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera frontCamera;
    [SerializeField] private Camera backCamera;
    [SerializeField] private InteractionController interactionController;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool cameraActiveStatus = frontCamera.gameObject.activeSelf;
            frontCamera.gameObject.SetActive(!cameraActiveStatus);
            backCamera.gameObject.SetActive(cameraActiveStatus);
            ToolManager.Instance.UiController.ReplaceCameraText(!cameraActiveStatus);
            
            interactionController.ReplaceCamera(!cameraActiveStatus ? frontCamera : backCamera, !cameraActiveStatus);
            
            Cursor.lockState = cameraActiveStatus ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = cameraActiveStatus;
        }
    }
}