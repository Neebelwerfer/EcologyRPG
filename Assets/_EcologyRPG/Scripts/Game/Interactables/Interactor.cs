using EcologyRPG.Game.Dialogue;
using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG.Utility.Interactions;
using EcologyRPG.Game.Player;

namespace EcologyRPG.Game.Interactables
{
    public class Interactor : MonoBehaviour, IInteractable
    {
        private InputActionReference Interacts;

        [SerializeField] private Interaction interaction;
        [SerializeField] private DialogueWindow dialogueWindow;
        private PlayerCharacter player;
        private Vector3 playerPosition;
        private Vector3 position;
        private Vector3 distanceVector;
        [SerializeField] private float distance;
        private bool initialized = false;

        public Interaction Interaction => interaction;

        private void Start()
        {
            player = PlayerManager.Instance.GetPlayerCharacter();
            Interacts = player.playerSettings.Interact;
            Interacts.action.Enable();
            position = transform.position;
        }

        private void Update()
        {
            if (!initialized)
            {
                dialogueWindow = FindObjectOfType<DialogueWindow>();
                if (dialogueWindow != null) { initialized = true; }
            }

            findDistance();
            if (distance <= 2.5)
            {
                if (Interacts.action.ReadValue<float>() == 1)
                {
                    Interact();
                }
            }
        }
        public void findDistance()
        {
            playerPosition = player.Transform.position;
            distanceVector = playerPosition - position;
            distance = distanceVector.magnitude;
        }

        public void Interact()
        {
            if (interaction is DialoguePathLine path)
            {
                dialogueWindow.Open(path);
            }
            else if (interaction is DialogueChoices choices)
            {
                dialogueWindow.Open(choices);
            }
        }
    }
}