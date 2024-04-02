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
        [SerializeField] private Animator animator;
        [SerializeField] private bool isNPC = false;
        private string idleParameter = "Idle";
        private string interactParameter = "Interacted";

        private float rotationSpeed = 2.5f;
        private Quaternion _lookRotation;
        private Vector3 _direction;
        public Interaction Interaction => interaction;

        private void Start()
        {
            player = PlayerManager.PlayerCharacter;
            Interacts = player.playerSettings.Interact;
            Interacts.action.Enable();
            position = transform.position;

            if (isNPC)
            {
                animator.SetBool(idleParameter, true);
            }


        }

        private void Update()
        {
            if (!initialized)
            {
                dialogueWindow = FindObjectOfType<DialogueWindow>();
                if (dialogueWindow != null) { initialized = true; }
            }
            findDistance();

            if (distance < 7.5f && isNPC)
            {
                lookAtPlayer();
            }

            if (distance <= 2.5f)
            {
                if (Interacts.action.ReadValue<float>() == 1)
                {
                    Interact();
                }
            }
        }
        public void findDistance()
        {
            playerPosition = player.Transform.Position;
            distanceVector = playerPosition - position;
            distance = distanceVector.magnitude;
        }
        private void lookAtPlayer()
        {

            _direction = distanceVector.normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            _lookRotation.Set(0, _lookRotation.y, 0, _lookRotation.w);
            transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }
        public void Interact()
        {
            if (!isNPC)
            {
                animator.SetTrigger(interactParameter);
            }
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