using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(MeshRenderer))]
public class BoltItem : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isFront;
    
    [field: SerializeField] public bool isInstalled { get; private set; } = true;
    [field: SerializeField] public bool isScrewed { get; private set; } = true;
    [field: SerializeField] public bool abilityPullOut { get; private set; } = false;

    [Header("Настройки анимации")] 
    [SerializeField] private float unscrewDistance = 0.05f;
    [SerializeField] private float unscrewTime = 1.5f;

    private Sequence _unscrewSequence;
    private MeshRenderer _meshRenderer;
    
    private bool _isActionFinished = false; 

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if (_meshRenderer != null) _meshRenderer.materials[0].SetFloat("_OutlineScale", 0);

        _unscrewSequence = DOTween.Sequence();

        int direction = _isFront ? -1 : 1;

        _unscrewSequence.Append(transform.DOLocalMoveX(transform.localPosition.x - unscrewDistance * direction, unscrewTime).SetEase(Ease.Linear));

        _unscrewSequence.OnComplete(() =>
        {
            _meshRenderer.enabled = false;
            isInstalled = false;
            isScrewed = false;
            abilityPullOut = true;
            _isActionFinished = true;
            ToolManager.Instance.UiController.ReplaceInfoText($"Болт полностью откручен");
        });

        _unscrewSequence.OnRewind(() =>
        {
            isScrewed = true;
            abilityPullOut = false;
            _isActionFinished = true;
            ToolManager.Instance.UiController.ReplaceInfoText($"Болт полностью закручен");
        });
        
        _unscrewSequence.SetAutoKill(false);
        _unscrewSequence.Pause();
    }

    public void OnHoverEnter()
    {
        if (isInstalled && _meshRenderer != null) _meshRenderer.materials[0].SetFloat("_OutlineScale", 0.9999f);
    }

    public void OnHoverExit()
    {
        if (_meshRenderer != null) _meshRenderer.materials[0].SetFloat("_OutlineScale", 0);
    }

    public void OnClick()
    {
        if (ToolManager.Instance.isWrenchEquipped) return;
        
        if (!isScrewed)
        {
            isInstalled = !isInstalled;
            _meshRenderer.enabled = isInstalled;
        }
    }

    public void OnHold()
    {
        if (!ToolManager.Instance.isWrenchEquipped)
        {
            ToolManager.Instance.UiController.ReplaceInfoText($"Используйте динамометрический ключ");
            return;
        }

        if (!isInstalled) return;
        if (_isActionFinished) return; 

        if (isScrewed)
        {
            if (_unscrewSequence.ElapsedPercentage() < 1f)
                _unscrewSequence.PlayForward();
        }
        else
        {
            if (_unscrewSequence.ElapsedPercentage() > 0f)
                _unscrewSequence.PlayBackwards();
        }
    }

    public void OnRelease()
    {
        _isActionFinished = false; 
        
        if (_unscrewSequence != null && _unscrewSequence.IsPlaying())
        {
            _unscrewSequence.Pause();
        }
    }
}