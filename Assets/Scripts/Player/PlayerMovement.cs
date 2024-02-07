using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference Movement;

    public float rotationSpeed = 4f;

    Vector3 forward;
    Vector3 right;

    // Start is called before the first frame update
    void Start()
    {
        Movement.action.Enable();
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = Movement.action.ReadValue<Vector2>();
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var mousePoint = Camera.main.ScreenPointToRay(mousePosition);

        if (movement.magnitude > 0)
        {
            var dir = (movement.y * forward + movement.x * right) * Time.deltaTime;
            transform.position += dir;
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
