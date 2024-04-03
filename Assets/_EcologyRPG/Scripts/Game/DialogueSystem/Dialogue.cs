using UnityEngine;

namespace EcologyRPG.GameSystems.Dialogue
{
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
}

