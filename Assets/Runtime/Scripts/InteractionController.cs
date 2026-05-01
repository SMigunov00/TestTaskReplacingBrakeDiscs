using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactionDistance = 3f;

    private IInteractable currentTarget;
    private IInteractable interactingTarget;
    private Collider _lastHitCollider;
    
    private bool _isFront = true;

    private void Update()
    {
        Ray ray;
        RaycastHit hit;
        if (_isFront)
        {
            ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * interactionDistance, Color.red);
        }
        else
        {
            ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            if (hit.collider != _lastHitCollider)
            {
                _lastHitCollider = hit.collider;
                
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    currentTarget?.OnHoverExit();
                    currentTarget = interactable;
                    currentTarget.OnHoverEnter();
                }
                else if (currentTarget != null)
                {
                    currentTarget.OnHoverExit();
                    currentTarget = null;
                }
            }
        }
        else
        {
            _lastHitCollider = null; 
            if (currentTarget != null)
            {
                currentTarget.OnHoverExit();
                currentTarget = null;
            }
        }
        
        if (currentTarget != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                interactingTarget = currentTarget;
                interactingTarget.OnClick();
            }
        }

        if (interactingTarget != null)
        {
            if (Input.GetMouseButton(0))
            {
                interactingTarget.OnHold();
            }

            if (Input.GetMouseButtonUp(0))
            {
                interactingTarget.OnRelease();
                interactingTarget = null;
            }
        }

        if (interactingTarget != null && currentTarget != interactingTarget)
        {
            if (Input.GetMouseButtonUp(0))
            {
                interactingTarget.OnRelease();
                interactingTarget = null;
            }
        }
    }

    public void ReplaceCamera(Camera camera, bool isFront)
    {
        _mainCamera = camera;
        _isFront = isFront;
    }
}