using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]

public class Dialogue
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string subjectName;
    [SerializeField][TextArea(3, 5)] private string message;

    public Sprite Sprite => sprite;
    public string Name => subjectName;
    public string Message => message;

}

