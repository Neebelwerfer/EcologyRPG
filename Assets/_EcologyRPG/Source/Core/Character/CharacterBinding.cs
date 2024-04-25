using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace EcologyRPG.Core.Character
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class CharacterBinding : MonoBehaviour
    {
        public Transform CastingPoint;
        public Rigidbody Rigidbody { get
            {
                rb ??= GetComponent<Rigidbody>();
                return rb;
            }
        }

        public CapsuleCollider Collider { get
            {
                col ??= GetComponent<CapsuleCollider>();
                return col;
            }
        }

        public Animator Animator { get
            {
                anim ??= GetComponent<Animator>();
                return anim;
            }
        }

        public AudioSource AudioSource { get
            {
                audioSource ??= GetComponent<AudioSource>();
                return audioSource;
            }
        }

        public UnityEvent CharacterUpdated = new UnityEvent();

        public UnityEvent<Collision> OnCollisionEnterEvent = new UnityEvent<Collision>();
        public UnityEvent<Collision> OnCollisionStayEvent = new UnityEvent<Collision>();
        public UnityEvent<Collision> OnCollisionExitEvent = new UnityEvent<Collision>();

        public BaseCharacter Character { get { return character; } }

        BaseCharacter character;
        Rigidbody rb;
        Animator anim;
        CapsuleCollider col;
        AudioSource audioSource;

        public void SetCharacter(BaseCharacter character)
        {
            this.character = character;
            CharacterUpdated.Invoke();
        }

        public void Reset()
        {
            character = null;
            OnCollisionExitEvent.RemoveAllListeners();
            OnCollisionStayEvent.RemoveAllListeners();
            OnCollisionEnterEvent.RemoveAllListeners();
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterEvent?.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            OnCollisionStayEvent?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnCollisionExitEvent?.Invoke(collision);
        }
    }
}