using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IngameUIManager : MonoBehaviour
{
    public InputActionReference CharacterUIButton;

    public GameObject characterUI;
    public GameObject DamageNumberPrefab;

    Action<InputAction.CallbackContext> toggleCharacterAction;

    private void Awake()
    {
        Debug.Log("Character UI Manager Started");
        characterUI.SetActive(false);
        CharacterUIButton.action.Enable();
        toggleCharacterAction = _ => ToggleCharacterUI();
        CharacterUIButton.action.started += toggleCharacterAction;
        DamageNumberHandler.Instance.Init(DamageNumberPrefab);
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
