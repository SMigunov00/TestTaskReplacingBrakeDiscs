using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }
    [HideInInspector] public CarPartItem currentHeldPart;

    [field: SerializeField] public bool isWrenchEquipped { get; private set; }
    [field: SerializeField] public Transform PlayerTransform {get; private set;}
    [field: SerializeField] public UiController UiController {get; private set;}

    [SerializeField] private GameObject wrenchModel;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateToolVisuals();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isWrenchEquipped)
                EquipHand();
            else
                EquipWrench();
        }
    }

    public void EquipHand()
    {
        isWrenchEquipped = false;
        UpdateToolVisuals();
    }

    public void EquipWrench()
    {
        isWrenchEquipped = true;
        UpdateToolVisuals();
    }

    private void UpdateToolVisuals()
    {
        if (wrenchModel != null)
        {
            wrenchModel.SetActive(isWrenchEquipped);
            UiController.ReplaceWrenchText(isWrenchEquipped);
        }
    }
}