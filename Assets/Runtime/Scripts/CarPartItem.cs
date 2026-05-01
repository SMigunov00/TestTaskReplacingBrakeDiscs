using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CarPartItem : MonoBehaviour, IInteractable
{
    [Header("Зависимости")]
    [SerializeField] private List<BoltItem> holdingBolts;

    [Tooltip("Детали которые должны быть снять перед снятием детали")]
    [SerializeField] private List<CarPartItem> blockingParts;
    
    [Tooltip("Детали которые должны быть установлены до установки детали")]
    [SerializeField] private List<CarPartItem> requiredPartsForInstall;

    [field: SerializeField] public bool isInstalled { get; private set; } = true;

    [Header("Настройки DOTween")]
    [SerializeField] private Vector3 targetPositionOffset;

    [SerializeField] private Vector3 targetRotationOffset;
    [SerializeField] private float animationDuration = 0.5f;

    [Header("Настройка Outline")]
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private float outlineScalse;
    
    private Vector3 originalLocalPosition;
    private Vector3 originalLocalRotation;


    private void Start()
    {
        if (meshRenderers != null)
        {
            foreach (var meshRenderer in meshRenderers)
                meshRenderer.materials[0].SetFloat("_OutlineScale", 0);
        }

        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localEulerAngles;
    }
    

    private bool CanBeRemoved()
    {
        if (holdingBolts != null)
        {
            foreach (var bolt in holdingBolts)
                if (bolt != null && bolt.isInstalled) return false;
        }

        if (blockingParts != null)
        {
            foreach (var part in blockingParts)
                if (part != null && part.isInstalled) return false;
        }

        return true;
    }
    
    private bool CanBeInstalled()
    {
        if (requiredPartsForInstall != null)
        {
            foreach (var part in requiredPartsForInstall)
            {
                if (part != null && !part.isInstalled)
                {
                    ToolManager.Instance.UiController.ReplaceInfoText($"Сначала установите {part.gameObject.name}");
                    return false;
                }
            }
        }
        
        if (holdingBolts != null)
        {
            foreach (var bolt in holdingBolts)
            {
                if (bolt != null && bolt.isInstalled)
                {
                    ToolManager.Instance.UiController.ReplaceInfoText("Место установки занято болтом, освободите место установки!");
                    return false;
                }
            }
        }

        return true;
    }

    public void OnHoverEnter()
    {
        if (isInstalled && meshRenderers != null)
        {
            foreach (var meshRenderer in meshRenderers)
                meshRenderer.materials[0].SetFloat("_OutlineScale", outlineScalse);
        }
    }

    public void OnHoverExit()
    {
        if (meshRenderers != null)
        {
            foreach (var meshRenderer in meshRenderers)
                meshRenderer.materials[0].SetFloat("_OutlineScale", 0);
        }
    }

    public void OnClick()
    {
        bool canInteract = isInstalled ? CanBeRemoved() : CanBeInstalled();

        if (canInteract)
        {
            isInstalled = !isInstalled;
            transform.DOKill();

            if (!isInstalled)
            {
                transform.DOLocalMove(originalLocalPosition + targetPositionOffset, animationDuration).SetEase(Ease.OutQuad);
                transform.DOLocalRotate(originalLocalRotation + targetRotationOffset, animationDuration).SetEase(Ease.OutQuad);
            }
            else
            {
                transform.DOLocalMove(originalLocalPosition, animationDuration).SetEase(Ease.InOutQuad);
                transform.DOLocalRotate(originalLocalRotation, animationDuration).SetEase(Ease.InOutQuad);
            }
        }
    }
    
    public void OnHold() { }

    public void OnRelease() { }
}