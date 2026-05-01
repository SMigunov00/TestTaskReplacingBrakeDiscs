using UnityEngine;

[RequireComponent(typeof(Collider),typeof(MeshRenderer))]
public class InstallSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private CarPartItem partToInstall;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.enabled = false;
    }

    public void OnHoverEnter() 
    { 
        if (ToolManager.Instance.currentHeldPart == partToInstall && meshRenderer != null) meshRenderer.enabled = true;
    }
    
    public void OnHoverExit() 
    { 
        if (meshRenderer != null) meshRenderer.enabled = false;
    }

    public void OnClick()
    {
        if (ToolManager.Instance.currentHeldPart == partToInstall)
        {
            partToInstall.OnClick(); 
            ToolManager.Instance.currentHeldPart = null;
            
            if (meshRenderer != null) meshRenderer.enabled = false;
        }
    }

    public void OnHold() { }
    public void OnRelease() { }
}