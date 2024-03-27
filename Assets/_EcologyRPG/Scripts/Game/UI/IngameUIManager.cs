using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace EcologyRPG.Game.UI
{
    public class IngameUIManager : MonoBehaviour
    {
        public InputActionReference CharacterUIButton;
        public GameObject characterUI;
        Action<InputAction.CallbackContext> toggleCharacterAction;

        private void Awake()
        {
            Debug.Log("Character UI Manager Started");
            characterUI.SetActive(false);
            CharacterUIButton.action.Enable();
            toggleCharacterAction = _ => ToggleCharacterUI();
            CharacterUIButton.action.started += toggleCharacterAction;
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
}