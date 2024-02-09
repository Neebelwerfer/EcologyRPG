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

        //Cached Stat references
        Stat MovementSpeed;
        Stat MaxStamina;
        Stat StaminaDrain;
        Stat StaminaGain;

        StatModification sprintMod;

        float _CurrentStamina;

        // Start is called before the first frame update
        public void Initialize(PlayerCharacter player)
        {
            Movement = player.playerSettings.Movement;
            Movement.action.Enable();
            Sprint = player.playerSettings.Sprint;
            Sprint.action.Enable();
            Sprint.action.started += (c) => OnSprint(true);
            Sprint.action.canceled += (c) => OnSprint(false);

            sprintMod = new StatModification(player.playerSettings.SprintMultiplier, StatModType.PercentMult, this);

            transform = player.transform;

            MovementSpeed = player.stats.GetStat("movementSpeed");
            MaxStamina = player.stats.GetStat("maxStamina");
            StaminaDrain = player.stats.GetStat("staminaDrain");
            StaminaGain = player.stats.GetStat("staminaGain");

            _CurrentStamina = MaxStamina.Value;
        }

        private void OnSprint(bool start)
        {
            if (start)
            {
                MovementSpeed.AddModifier(sprintMod);
            }
            else
            {
                MovementSpeed.RemoveModifier(sprintMod);
            }
        }

        public void FixedUpdate()
        {
            Vector2 movement = Movement.action.ReadValue<Vector2>();


            if (movement.magnitude > 0)
            {
                if (Sprint.action.IsPressed())
                {
                    _CurrentStamina -= StaminaDrain.Value * TimeManager.IngameDeltaTime;
                    _CurrentStamina = Mathf.Clamp(_CurrentStamina, 0, MaxStamina.Value);
                    if (_CurrentStamina == 0)
                    {
                        Sprint.action.Disable();
                    }
                }
                else
                {
                    _CurrentStamina += StaminaGain.Value * TimeManager.IngameDeltaTime;
                    _CurrentStamina = Mathf.Clamp(_CurrentStamina, 0, MaxStamina.Value);
                    if (!Sprint.action.enabled && _CurrentStamina == MaxStamina.Value)
                    {
                        Sprint.action.Enable();
                    }

                }

                var speed = MovementSpeed.Value * TimeManager.IngameDeltaTime;
                var dir = (movement.y * forward + movement.x * right);
                transform.position += speed * dir;
                var rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), TimeManager.IngameDeltaTime * rotationSpeed);
                transform.rotation = rot;

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
            if (Physics.Raycast(mousePoint, out RaycastHit hit, 100f, LayerMask.NameToLayer("Player")))
            {
                var lookAt = hit.point;
                lookAt.y = transform.position.y;
                var dir = (lookAt - transform.position).normalized;
                var rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), TimeManager.IngameDeltaTime * rotationSpeed);
                transform.rotation = rot;
            }
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {

        }
    }
}
