using UnityEngine;
using UnityEngine.Events;

namespace EcologyRPG.Core.Abilities
{
    public class VFXBinder : MonoBehaviour
    {
        public UnityEvent OnStart;
        public UnityEvent OnEnd;

        public void Start()
        {
            OnStart.Invoke();
        }

        public void End()
        {
            OnEnd.Invoke();
        }
    }
}