using EcologyRPG.GameSystems.Interactables;
using UnityEngine;

namespace EcologyRPG.GameSystems.Interactables
{
    public abstract class Interaction : ScriptableObject
    {
        public bool OneTimeUse = false;
        abstract public void Interact();
    }
}
