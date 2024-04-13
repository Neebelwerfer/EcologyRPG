using UnityEngine.InputSystem;
using UnityEngine;
using EcologyRPG.GameSystems.PlayerSystems;
using EcologyRPG.GameSystems.Interactables;

namespace EcologyRPG.GameSystems.Interactables
{
    [RequireComponent(typeof(Animator))]
    public class StationaryNPC : Interactor
    {
        private string idleParameter = "Idle";

        private float rotationSpeed = 2.5f;
        private Quaternion _lookRotation;
        private Vector3 _direction;

        protected override void Start()
        {
            base.Start();
            animator.SetBool(idleParameter, true);
        }

        private void Update()
        {
            FindDistance();

            if (distance < 7.5f)
            {
                LookAtPlayer();
            }
            if (distance <= 2.5f)
            {
                if (IsInteracting())
                {
                    Interact();
                }
            }
        }
        private void LookAtPlayer()
        {

            _direction = distanceVector.normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            _lookRotation.Set(0, _lookRotation.y, 0, _lookRotation.w);
            transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}