using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityComponents
{
    public class SoundComponent : AbilityComponent
    {
        public AudioClip Clip;
        public float Volume = 1;
        public float Pitch = 1;
        public bool Loop = false;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            if (Clip == null)
            {
                Debug.LogError("SoundComponent: No clip assigned");
                return;
            }
            AudioSource.PlayClipAtPoint(Clip, cast.castPos, Volume);
        }
    }
}