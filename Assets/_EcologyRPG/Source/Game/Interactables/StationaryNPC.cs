using EcologyRPG.GameSystems.Dialogue;
using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG.Utility.Interactions;
using EcologyRPG.GameSystems.PlayerSystems;

namespace EcologyRPG.GameSystems.Interactables
{
    public class StationaryNPC : MonoBehaviour, IInteractable
    {
        private InputActionReference Interacts;

        [SerializeField] private Interaction interaction;
        [SerializeField] private DialogueWindow dialogueWindow;
        private PlayerCharacter player;
        private Vector3 playerPosition;
        private Vector3 position;
        private Vector3 distanceVector;
        private float distance;
        private bool initialized = false;
        [SerializeField] private Animator animator;
        private string idleParameter = "Idle";

        private float rotationSpeed = 2.5f;
        private Quaternion _lookRotation;
        private Vector3 _direction;
        public Interaction Interaction => interaction;

        private void Start()
        {
            player = Player.PlayerCharacter;
            Interacts = player.playerSettings.Interact;
            Interacts.action.Enable();
            position = transform.position;
            animator.SetBool(idleParameter, true);
            
        }

        private void Update()
        {
            if (!initialized)
            {
                dialogueWindow = FindObjectOfType<DialogueWindow>();
                if (dialogueWindow != null) { initialized = true; }
            }
            FindDistance();

            if (distance < 7.5f)
            {
                LookAtPlayer();
            }
            if (distance <= 2.5f)
            {
                if (Interacts.action.ReadValue<float>() == 1)
                {
                    Interact();
                }
            }
        }
        public void FindDistance()
        {
            playerPosition = player.Transform.Position;
            distanceVector = playerPosition - position;
            distance = distanceVector.magnitude;
        }
        private void LookAtPlayer()
        {

            _direction = distanceVector.normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            _lookRotation.Set(0, _lookRotation.y, 0, _lookRotation.w);
            transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }
        public void Interact()
        {
            //animator.SetTrigger(interactParameter);

            if (interaction is DialoguePathLine path)
            {
                dialogueWindow.Open(path);
            }
            else if (interaction is DialogueChoices choices)
            {
                dialogueWindow.Open(choices);
            }
            else if (interaction is DialogueConnector connector)
            {
                dialogueWindow.Open(connector);
            }

            else return;
        }
    }
}