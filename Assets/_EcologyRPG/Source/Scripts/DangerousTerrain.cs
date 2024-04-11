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

        Collider _collider;
        List<Target> _characters;

        bool isActive = false;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _characters = new();
            _collider.isTrigger = true;
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

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out CharacterBinding character))
            {
                if(excludedTags.Length == 0 || AllowedTag(character.Character))
                {
                    if(!ContainsCharacter(character.Character.GUID))
                    {
                        AddCharacter(character.Character);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out CharacterBinding character))
            {
                if(ContainsCharacter(character.Character.GUID))
                {
                    RemoveCharacter(character.Character.GUID);
                }
            }
        }
    }
}