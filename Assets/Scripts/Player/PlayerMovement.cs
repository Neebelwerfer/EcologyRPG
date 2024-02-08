using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : IPlayerModule
{
    InputActionReference Movement;
    InputActionReference Sprint;

    public float rotationSpeed = 4f;

    Vector3 forward;
    Vector3 right;
    PlayerCharacter player;
    Transform transform;

    Stat MovementSpeed;
    Stat MaxStamina;
    Stat StaminaDrain;
    Stat StaminaGain;
    Stat SprintingSpeed;

    float _MaxStamina;
    float _CurrentStamina;

    // Start is called before the first frame update
    public void Initialize(PlayerCharacter player)
    {
        Movement = player.Movement;
        Movement.action.Enable();
        Sprint = player.Sprint;
        Sprint.action.Enable();

        transform = player.transform;
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        MovementSpeed = player.stats.GetStat("movementSpeed");
        MaxStamina = player.stats.GetStat("maxStamina");
        StaminaDrain = player.stats.GetStat("staminaDrain");
        StaminaGain = player.stats.GetStat("staminaGain");
        SprintingSpeed = player.stats.GetStat("sprintSpeed");

        _MaxStamina = MaxStamina.Value;
        _CurrentStamina = _MaxStamina;

        MaxStamina.OnStatChanged.AddListener((value) => _MaxStamina = value);
    }

    // Update is called once per frame
    public void Update()
    {
        Vector2 movement = Movement.action.ReadValue<Vector2>();
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var mousePoint = Camera.main.ScreenPointToRay(mousePosition);

        if (movement.magnitude > 0)
        {
            var speed = 0f;
            if (Sprint.action.IsPressed())
            {
                speed = SprintingSpeed.Value * Time.deltaTime;
                _CurrentStamina -= StaminaDrain.Value * Time.deltaTime;
                _CurrentStamina = Mathf.Clamp(_CurrentStamina, 0, _MaxStamina);
                if(_CurrentStamina == 0)
                {
                    Sprint.action.Disable();
                }
            } else
            {
                speed = MovementSpeed.Value * Time.deltaTime;
                _CurrentStamina += StaminaGain.Value * Time.deltaTime;
                _CurrentStamina = Mathf.Clamp(_CurrentStamina, 0, _MaxStamina);
                if(!Sprint.action.enabled && _CurrentStamina == _MaxStamina)
                {
                    Sprint.action.Enable();
                }

            }
            var dir = (movement.y * forward + movement.x * right);
            transform.position += speed * dir;
            var rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
            transform.rotation = rot;

        }
        else
        {
            if (Physics.Raycast(mousePoint, out RaycastHit hit, 100f, LayerMask.NameToLayer("Player")))
            {
                var lookAt = hit.point;
                lookAt.y = transform.position.y;
                var dir = (lookAt - transform.position).normalized;
                var rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
                transform.rotation = rot;
            }
        }
       
    }
}
