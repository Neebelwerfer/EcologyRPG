using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float Duration = 1f;
    public float Speed = 1f;
    Vector3 Dir;
    public void Init(float damage, Color textColor)
    {
        var text = GetComponent<TextMeshProUGUI>();
        text.text = damage.ToString();
        text.color = textColor;
        Dir = -Camera.main.transform.up;
    }

    private void Update()
    {
        Duration -= Time.deltaTime;

        transform.position += Speed * Time.deltaTime * Dir;

        if (Duration <= 0)
        {
            Destroy(gameObject);
        }
    }
}