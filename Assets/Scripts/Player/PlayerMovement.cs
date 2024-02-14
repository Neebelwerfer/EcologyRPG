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
    public class PlayerMovement : IPlayerModule
    {
        InputActionReference Movement;
        InputActionReference Sprint;

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
        public void Initialize(PlayerCharacter player)
        {
            this.player = player;
            Movement = player.playerSettings.Movement;
            Movement.action.Enable();

            transform = player.transform.Find("VisualPlayer");

            MovementSpeed = player.stats.GetStat("movementSpeed");
            Stamina = player.stats.GetResource("stamina");
            StaminaGain = player.stats.GetStat("staminaGain");
        }

        public void FixedUpdate()
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
                rb.velocity += speed * 100 * dir;
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, MovementSpeed.Value);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), TimeManager.IngameDeltaTime * rotationSpeed);

            }
            else
            {
                UpdateRotationBasedOnMouse();
            }
        }

        public void UpdateRotationBasedOnMouse()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            var mousePoint = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(mousePoint, out RaycastHit hit, 100f, LayerMask.NameToLayer("Entity")))
            {
                var lookAt = hit.point;
                lookAt.y = transform.position.y;
                var dir = (lookAt - transform.position).normalized;
                var rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), TimeManager.IngameDeltaTime * rotationSpeed);
                transform.rotation = rot;
            }
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {

        }

        public void OnDestroy()
        {

        }
    }
}