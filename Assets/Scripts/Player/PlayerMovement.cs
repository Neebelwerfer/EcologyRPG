using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference Movement;

    public float rotationSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        Movement.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = Movement.action.ReadValue<Vector2>();
        transform.position += (movement.y * transform.forward + movement.x * transform.right) * Time.deltaTime; //   new Vector3(movement.x, 0, movement.y) * Time.deltaTime;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var mousePoint = Camera.main.ScreenPointToRay(mousePosition);
        if(Physics.Raycast(mousePoint, out RaycastHit hit, 100f, LayerMask.NameToLayer("Player")))
        { 
            var lookAt = hit.point;
            lookAt.y = transform.position.y;
            var dir = (lookAt - transform.position).normalized;
            var rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
            transform.rotation = rot;
        }
    }
}
