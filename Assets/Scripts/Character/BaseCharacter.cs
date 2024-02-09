using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public abstract class BaseCharacter : MonoBehaviour
    {
        public Stats stats;

        public virtual void Start()
        {
            stats = new Stats();
        }
    }
}
