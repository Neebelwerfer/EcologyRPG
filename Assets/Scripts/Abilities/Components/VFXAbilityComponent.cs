using UnityEngine;

namespace Character.Abilities.AbilityComponents
{
    public class VFXAbilityComponent : VisualAbilityComponent
    {
        public GameObject vfxPrefab;
        public float duration = 1f;

        public override void ApplyEffect(CastInfo cast, BaseCharacter target)
        {
            GameObject effect;
            if(target != null)
            {
                effect = Instantiate(vfxPrefab, target.Position, Quaternion.identity);
            }
            else
            {
                effect = Instantiate(vfxPrefab, cast.castPos, Quaternion.identity);
            }
            Destroy(effect, duration);
        }
    }
}