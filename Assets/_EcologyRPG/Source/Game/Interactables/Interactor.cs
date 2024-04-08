using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG.GameSystems.PlayerSystems;
using EcologyRPG.GameSystems;
using EcologyRPG.GameSystems.Interactables;

namespace EcologyRGP.GameSystems.Interactables
{
    public class Interactor : MonoBehaviour
    {
        protected InputActionReference Interacts;
        [SerializeField] protected Interaction interaction;
        [SerializeField] protected Animator animator;
        [SerializeField] protected string animationTrigger;
        public Interaction Interaction => interaction;

        protected PlayerCharacter player;
        protected Vector3 playerPosition;
        protected Vector3 position;
        protected Vector3 distanceVector;
        protected float distance;

        bool _isInteracting = false;

        protected virtual void Start()
        {
            player = Player.PlayerCharacter;
            Interacts = player.playerSettings.Interact;
            Interacts.action.Enable();
            position = transform.position;
            if (animator == null)
            {
                if(TryGetComponent(out Animator anim))
                {
                    animator = anim;
                }
            }
        }
        private void Update()
        {
            FindDistance();

            if (distance <= 2.5f)
            {
                if (IsInteracting())
                {
                    Interact();
                }
            }
        }

        protected bool IsInteracting()
        {
            if(!_isInteracting && Interacts.action.ReadValue<float>() == 1)
            {
                _isInteracting = true;
                return true;
            }
            else if(_isInteracting && Interacts.action.ReadValue<float>() == 0)
            {
                _isInteracting = false;
                return false;
            }
            return false;
        }

        public void Interact()
        {
            if (interaction != null)
            {
                if(animator != null)
                    animator.SetTrigger(animationTrigger);
                interaction.Interact();
                if(interaction.OneTimeUse)
                {
                    Destroy(this);
                }
            }
        }

        public void FindDistance()
        {
            playerPosition = player.Transform.Position;
            distanceVector = playerPosition - position;
            distance = distanceVector.magnitude;
        }
    }
}