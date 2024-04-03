using TMPro;
using UnityEngine;

namespace EcologyRPG.Core.UI
{
    public class DamageText : MonoBehaviour
    {
        public float Duration = 1f;
        public float Speed = 1f;

        [HideInInspector] public float RemainingDuration;
        Vector3 Dir;
        public void Init(float damage, Color textColor)
        {
            var text = GetComponent<TextMeshProUGUI>();
            var damageRounded = Mathf.Ceil(damage);
            text.text = damageRounded.ToString();
            text.color = textColor;
            RemainingDuration = Duration;
            Dir = -Camera.main.transform.up;
        }

        public void OnUpdate()
        {
            RemainingDuration -= Time.deltaTime;

            transform.position += Speed * Time.deltaTime * Dir;
        }
    }
}