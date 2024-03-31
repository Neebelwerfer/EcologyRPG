using System;
using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG.Game.Player;

namespace EcologyRPG.Game.UI
{
    public class IngameUIManager : MonoBehaviour
    {
        public InputActionReference CharacterUIButton;
        public GameObject characterSheetUI;
        public GameObject playerUI;
        Action<InputAction.CallbackContext> toggleCharacterAction;

        private void Awake()
        {
            Debug.Log("Character UI Manager Started");
            characterSheetUI.SetActive(false);
            CharacterUIButton.action.Enable();
            toggleCharacterAction = _ => ToggleCharacterUI();
            CharacterUIButton.action.started += toggleCharacterAction;
            EventManager.AddListener("PlayerDeath", OnPlayerDeath);
            EventManager.AddListener("PlayerSpawn", OnPlayerSpawn);

            if (PlayerManager.IsPlayerAlive) playerUI.SetActive(true);
        }

        private void OnPlayerSpawn(EventData arg0)
        {
            playerUI.SetActive(true);
        }

        private void OnPlayerDeath(EventData arg0)
        {
            characterSheetUI.SetActive(false);
            playerUI.SetActive(false);
        }

        private void ToggleCharacterUI()
        {
            characterSheetUI.SetActive(!characterSheetUI.activeSelf);
        }


        private void OnDestroy()
        {
            CharacterUIButton.action.Disable();
            CharacterUIButton.action.started -= toggleCharacterAction;
        }
    }
}