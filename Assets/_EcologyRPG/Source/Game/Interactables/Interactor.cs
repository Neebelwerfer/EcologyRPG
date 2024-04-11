using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG.GameSystems.PlayerSystems;
using EcologyRPG.GameSystems;
using EcologyRPG.GameSystems.Interactables;

namespace EcologyRPG.GameSystems.Interactables
{
    public class Interactor : MonoBehaviour
    {
        protected InputActionReference Interacts;
        [SerializeField] protected Interaction interaction;
        [SerializeField] protected Animator animator;
        [SerializeField] protected string animationTrigger;
        [SerializeField] private float oWidth = 0.35f;
        private ObjectOutline outline;
        
        public Interaction Interaction => interaction;

        protected PlayerCharacter player;
        protected Vector3 playerPosition;
        protected Vector3 position;
        protected Vector3 distanceVector;
        protected float distance;

        bool _isInteracting = false;

        private void Awake()
        {
            outline = gameObject.AddComponent<ObjectOutline>();
            outline.OutlineWidth = 0;
            outline.OutlineMode = ObjectOutline.Mode.OutlineAll;
            outline.OutlineColor = new Color(255, 215, 0);
        }
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
                outline.OutlineWidth = oWidth;

                if (IsInteracting())
                {
                    Interact();
                }
            }
            else outline.OutlineWidth = 0;

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