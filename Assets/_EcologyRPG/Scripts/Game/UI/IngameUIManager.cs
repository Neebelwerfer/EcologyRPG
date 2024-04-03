using System;
using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG._Game.Player;

namespace EcologyRPG._Game.UI
{
    public class IngameUIManager : MonoBehaviour
    {
        public InputActionReference CharacterUIButton;
        public InputActionReference IngameMenuButton;
        public GameObject characterSheetUI;
        public GameObject playerUI;
        public GameObject DeathScreen;
        public GameObject IngameMenu;
        Action<InputAction.CallbackContext> toggleCharacterAction;
        Action<InputAction.CallbackContext> toggleIngameMenuAction;

        private void Awake()
        {
            Debug.Log("Character UI Manager Started");
            characterSheetUI.SetActive(false);
            IngameMenu.SetActive(false);
            CharacterUIButton.action.Enable();
            toggleCharacterAction = _ => ToggleCharacterUI();
            toggleIngameMenuAction = _ => ToggleIngameMenu();
            IngameMenuButton.action.Enable();
            IngameMenuButton.action.started += toggleIngameMenuAction;
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
            DeathScreen.SetActive(true);
        }

        public void RespawnPlayer()
        {
            DeathScreen.SetActive(false);
            PlayerManager.Instance.RespawnPlayer();
        }

        public void ExitGame()
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void ToggleCharacterUI()
        {
            characterSheetUI.SetActive(!characterSheetUI.activeSelf);
        }

        public void ToggleIngameMenu()
        {
            if(IngameMenu.activeSelf)
            {
                IngameMenu.SetActive(false);
                playerUI.SetActive(true);
                Game.Instance.Resume();
            }
            else
            {
                IngameMenu.SetActive(true);
                playerUI.SetActive(false);
                characterSheetUI.SetActive(false);
                Game.Instance.Pause();
            }
        }

        private void OnDestroy()
        {
            CharacterUIButton.action.Disable();
            CharacterUIButton.action.started -= toggleCharacterAction;
            IngameMenuButton.action.started -= toggleIngameMenuAction;
        }
    }
}