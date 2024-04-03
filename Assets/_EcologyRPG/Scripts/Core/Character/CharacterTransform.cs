using UnityEngine;

namespace EcologyRPG.Core.Character
{
    public class CharacterTransform
    {
        public CharacterTransform()
        {
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
        }

        public CharacterTransform(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void SetBinding(CharacterBinding binding)
        {
            this.binding = binding;
            Position = binding.transform.position;
            Rotation = binding.transform.rotation;
        }

        public void RemoveBinding()
        {
            binding = null;
        }

        CharacterBinding binding;
        Vector3 position;
        Quaternion rotation;

        public Vector3 Position { 
            get 
            { 
                if(binding != null)
                {
                    return binding.transform.position;
                }
                return position;
            }
            set
            {
                if(binding != null)
                {
                    binding.transform.position = value;
                    position = binding.transform.position;
                }
                else position = value;
            }
        }
        public Quaternion Rotation { get
            {
                if(binding != null)
                {
                    return binding.transform.rotation;
                }
                return rotation;
            }
            set
            {
                if(binding != null)
                {
                    binding.transform.rotation = value;
                    rotation = binding.transform.rotation;
                }
                else rotation = value;
            }
        }

        public Vector3 Forward { get
            {
                return Rotation * Vector3.forward;
            } 
        }

        public Vector3 Right { get
            {
                return Rotation * Vector3.right;
            }
        }

        public Vector3 Up { get
            {
                return Rotation * Vector3.up;
            }
        }

        public void LookAt(Vector3 target)
        {
            Rotation = Quaternion.LookRotation(target - Position);
        }

        public void Move(Vector3 direction, float speed)
        {
            Position += (direction * speed);
        }
    }
}