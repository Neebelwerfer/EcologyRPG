using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG.Utility.Interactions;
using EcologyRPG.GameSystems.PlayerSystems;
using EcologyRPG.GameSystems;

namespace EcologyRGP.GameSystems.Interactables
{
    public class Interactor : MonoBehaviour, IInteractable
    {
        private InputActionReference Interacts;
        [SerializeField] private Interaction interaction;
        public Interaction Interaction => interaction;

        private PlayerCharacter player;
        private Vector3 playerPosition;
        private Vector3 position;
        private Vector3 distanceVector;
        private float distance;

        private void Start()
        {
            player = Player.PlayerCharacter;
            Interacts = player.playerSettings.Interact;
            Interacts.action.Enable();
            position = transform.position;
        }
        private void Update()
        {
            FindDistance();

            if (distance <= 2.5f)
            {
                if (Interacts.action.ReadValue<float>() == 1)
                {
                    Interact();
                }
            }
        }
        public void Interact()
        {
            if (interaction is QuestProgressor questProgressor)
            {
                Game.Flags.SetFlag(questProgressor.QuestFlag, questProgressor.ProgessStage);
            }
            else return;
        }

        public void FindDistance()
        {
            playerPosition = player.Transform.Position;
            distanceVector = playerPosition - position;
            distance = distanceVector.magnitude;
        }
    }
}