using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("References")]
    public InputActionReference Movement;
    public InputActionReference Sprint;

    [Header("Movement Settings")]
    public float SprintMultiplier = 1.2f;
}
