using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Scripts
{
    class Target
    {
        public string GUID;
        public float timer;
    }

    [RequireComponent(typeof(Collider))]
    public class DangerousTerrain : MonoBehaviour
    {
        [CharacterTag]
        public string[] excludedTags;

        public float damagePerTick= 1f;
        public float damageInterval = 1f;
        public DamageType damageType;

        List<Target> _characters;

        private void Awake()
        {
            _characters = new();
            gameObject.layer = LayerMask.NameToLayer(GameSettings.GroundHazardName);
        }

        void Update()
        {
            if (_characters.Count > 0)
            {
                foreach (var target in _characters)
                {
                    target.timer -= Time.deltaTime;
                    if (target.timer <= 0)
                    {
                        var Character = Characters.Instance.GetCharacter(target.GUID);
                        Character.ApplyDamage(new DamageInfo()
                        {
                            damage = damagePerTick,
                            type = damageType,
                            source = gameObject
                        });
                        target.timer = damageInterval;
                    }
                }
            }
        }

        bool ContainsCharacter(string guid)
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                if (_characters[i].GUID == guid)
                {
                    return true;
                }
            }
            return false;
        }

        bool AllowedTag(BaseCharacter character)
        {
            foreach (var tag in excludedTags)
            {
                if (character.Tags.Contains(tag))
                {
                    return false;
                }
            }
            return true;
        }

        void AddCharacter(BaseCharacter character)
        {
            character.ApplyDamage(new DamageInfo()
            {
                damage = damagePerTick,
                type = damageType,
                source = gameObject
            });
            _characters.Add(new Target { GUID = character.GUID, timer = damageInterval });
        }

        void RemoveCharacter(string guid)
        {
            _characters.RemoveAll(x => x.GUID == guid);
        }

        void CheckCollision(Collider other, bool onEnter)
        {
            if (other.TryGetComponent(out CharacterBinding character))
            {
                if (excludedTags.Length == 0 || AllowedTag(character.Character))
                {
                    if (onEnter)
                    {
                        if (!ContainsCharacter(character.Character.GUID))
                        {
                            AddCharacter(character.Character);
                        }
                    }
                    else
                    {
                        if (ContainsCharacter(character.Character.GUID))
                        {
                            RemoveCharacter(character.Character.GUID);
                        }
                    }
                }
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
           CheckCollision(collision.collider, true);
        }

        private void OnCollisionExit(Collision collision)
        {
            CheckCollision(collision.collider, false);
        }

        private void OnTriggerEnter(Collider other)
        {
           CheckCollision(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckCollision(other, false);
        }
    }
}