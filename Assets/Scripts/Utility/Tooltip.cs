using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject TooltipObject;
    public TextMeshProUGUI Title;
    public Image Icon;
    public TextMeshProUGUI Description;

    static Tooltip instance;
    object activeTooltipSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Show(object source, TooltipData data)
    {
        if (activeTooltipSource == source) return;
        TooltipObject.SetActive(true);
        TooltipObject.transform.position = Mouse.current.position.ReadValue();
        Title.text = data.Title;
        Icon.sprite = data.Icon;
        Description.text = data.Description;
        activeTooltipSource = source;
    }

    public void Hide(object source)
    {
        if (instance != null)
        {
            if (activeTooltipSource == source)
            {
                activeTooltipSource = null;
                TooltipObject.SetActive(false);
            }
        }
    }

    public static void HideTooltip(object source)
    {
        instance.Hide(source);
    }

    public static void ShowTooltip(object source, TooltipData data)
    {
        instance.Show(source, data);
    }
}

public class TooltipData
{
    public string Title;
    public Sprite Icon;
    public string Description;
}