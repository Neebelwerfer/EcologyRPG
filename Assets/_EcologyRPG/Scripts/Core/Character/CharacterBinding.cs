using UnityEngine;
using UnityEngine.AI;

namespace EcologyRPG.Core.Character
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterBinding : MonoBehaviour
    {
        public Transform CastingPoint;
        public Rigidbody Rigidbody { get
            {
                rb ??= GetComponent<Rigidbody>();
                return rb;
            }
        }
        public Animator Animator { get
            {
                anim ??= GetComponent<Animator>();
                return anim;
            }
        }


        public BaseCharacter Character { get { return character; } }

        BaseCharacter character;
        Rigidbody rb;
        Animator anim;

        public void SetCharacter(BaseCharacter character)
        {
            this.character = character;
        }
    }
}