using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility.Events;

public class IngameUIManager : MonoBehaviour
{
    public InputActionReference CharacterUIButton;
    public GameObject characterUI;
    [Header("Tooltip")]
    public GameObject Tooltip;
    public TextMeshProUGUI Title;
    public Image Icon;
    public TextMeshProUGUI Description;

    public LayerMask UILayers;
    object activeTooltipSource;

    Action<InputAction.CallbackContext> toggleCharacterAction;

    private void Awake()
    {
        characterUI.SetActive(false);
        CharacterUIButton.action.Enable();
        toggleCharacterAction = _ => ToggleCharacterUI();
        CharacterUIButton.action.started += toggleCharacterAction;
        EventManager.AddListener("TooltipEntered", ShowTooltip);
        EventManager.AddListener("TooltipExited", HideTooltip);
    }

    private void HideTooltip(EventData arg0)
    {
        if(activeTooltipSource == arg0.source)
        {
            activeTooltipSource = null;
            Tooltip.SetActive(false);
        }
    }

    private void ShowTooltip(EventData arg0)
    {
        if(arg0 is TooltipEvent data)
        {
            if(activeTooltipSource == data.source) return;
            Tooltip.SetActive(true);
            Tooltip.transform.position = Mouse.current.position.ReadValue();
            Title.text = data.Title;
            Icon.sprite = data.Icon;
            Description.text = data.Description;
            activeTooltipSource = data.source;
        }
    }

    private void ToggleCharacterUI()
    {
        characterUI.SetActive(!characterUI.activeSelf);
    }


    private void OnDestroy()
    {
        CharacterUIButton.action.Disable();
        CharacterUIButton.action.started -= toggleCharacterAction;
    }
}
