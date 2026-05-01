using UnityEngine;

public class NewPartItem : MonoBehaviour, IInteractable
{
    [SerializeField] private CarPartItem linkedCarPart; 

    public void OnHoverEnter() { }
    public void OnHoverExit() { }

    public void OnClick()
    {
        ToolManager.Instance.currentHeldPart = linkedCarPart;
        linkedCarPart.transform.position = ToolManager.Instance.PlayerTransform.position;
        ToolManager.Instance.UiController.ReplaceInfoText($"Взяли в руки: {linkedCarPart.gameObject.name}");
        gameObject.SetActive(false); 
    }

    public void OnHold() { }
    public void OnRelease() { }
}