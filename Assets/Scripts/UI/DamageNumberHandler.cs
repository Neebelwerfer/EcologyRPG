using Character;
using TMPro;
using UnityEngine;

public class DamageNumberHandler
{
    static DamageNumberHandler _instance;
    public static DamageNumberHandler Instance { get { return _instance ?? (_instance = new DamageNumberHandler()); } }

    GameObject DamageNumberPrefab;

    public void Init(GameObject DamageNumberPrefab)
    {
        this.DamageNumberPrefab = DamageNumberPrefab;
        EventManager.AddListener("DamageEvent", OnDamageEvent);
    }

    private void OnDamageEvent(EventData data)
    {
        if (data is DamageEvent damageEvent)
        {
            Debug.Log("Damage Event Received");
            var damageNumber = GameObject.Instantiate(DamageNumberPrefab, damageEvent.target.transform.position, Quaternion.identity);
            damageNumber.GetComponentInChildren<DamageText>().Init(damageEvent.damageTaken, GetDamageColor(damageEvent));
        }
    }

    Color GetDamageColor(DamageEvent damageEvent)
    {
        if(damageEvent.damageType == Character.Abilities.DamageType.Physical)
        {
            return Color.red;
        }
        else if(damageEvent.damageType == Character.Abilities.DamageType.Water)
        {
            return Color.blue;
        }
        else if(damageEvent.damageType == Character.Abilities.DamageType.Toxic)
        {
            return Color.magenta;
        }
        else
        {
            return Color.white;
        }
    }
}
