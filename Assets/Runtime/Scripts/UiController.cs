using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _wrenchText;
    [SerializeField] private string _equipWrenchText;
    [SerializeField] private string _equipHandText; 
    [SerializeField] private TextMeshProUGUI _cameraText;
    [SerializeField] private string _frontCameraText;
    [SerializeField] private string _backCameraText;
    [SerializeField] private TextMeshProUGUI _infoText;

    private Tween _infoTextTween;
    
    public void ReplaceWrenchText(bool equipWrench) => _wrenchText.SetText(equipWrench ?  _equipWrenchText : _equipHandText);
    
    public void ReplaceCameraText(bool isFrontCamera) => _cameraText.SetText(isFrontCamera ?  _frontCameraText : _backCameraText);
    
    public void ReplaceInfoText(string warningText)
    {
        _infoText.SetText(warningText);
        _infoTextTween?.Kill(); 
        _infoTextTween = DOVirtual.DelayedCall(2f, () =>
        {
            _infoText.text = null;
        });
    }
}