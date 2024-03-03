using Character;
using Codice.Client.Commands;
using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Player
{
    public class PlayerMovement : PlayerModule
    {
        InputActionReference Movement;

        public float rotationSpeed = 4f;

        //Rotated forward and right vectors to match the camera
        Vector3 forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        Vector3 right = Quaternion.Euler(new Vector3(0, 135, 0)) * Vector3.forward;

        PlayerCharacter player;
        Transform transform;

        //Cached Character references
        Stat MovementSpeed;
        Resource Stamina;
        Stat StaminaGain;

        StatModification sprintMod;

        // Start is called before the first frame update
        public override void Initialize(PlayerCharacter player)
        {
            this.player = player;
            Movement = player.playerSettings.Movement;
            Movement.action.Enable();

            transform = player.transform.Find("VisualPlayer");

            MovementSpeed = player.Stats.GetStat("movementSpeed");
            Stamina = player.Stats.GetResource("stamina");
            StaminaGain = player.Stats.GetStat("staminaGain");
        }

        public override void FixedUpdate()
        {
            var rb = player.Rigidbody;
            Vector2 movement = Movement.action.ReadValue<Vector2>();
            Stamina += StaminaGain.Value * TimeManager.IngameDeltaTime;

            if(player.state == CharacterStates.disabled || player.state == CharacterStates.dead || player.state == CharacterStates.dodging )
            {
                return;
            }

            if (movement.magnitude > 0)
            {
                var speed = MovementSpeed.Value * TimeManager.IngameDeltaTime;
                var dir = (movement.y * forward + movement.x * right).normalized;
                Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100, LayerMask.GetMask("Ground"));
                dir = Vector3.ProjectOnPlane(dir, hit.normal).normalized;
                rb.MovePosition(transform.position + speed * dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), TimeManager.IngameDeltaTime * rotationSpeed);

            }
            else
            {
                UpdateRotationBasedOnMouse();
            }
        }

        public void UpdateRotationBasedOnMouse()
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            {
                var dir = (hit.point - transform.position).normalized;
                dir = Vector3.ProjectOnPlane(dir, Vector3.up).normalized;
                var rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), TimeManager.IngameDeltaTime * rotationSpeed);
                transform.rotation = rot;
            }
        }

        public override void OnDestroy()
        {
        }
    }
}