using Character;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberHandler : MonoBehaviour
{
    static DamageNumberHandler _instance;
    public static DamageNumberHandler Instance { get { return _instance; } }

    public GameObject DamageNumberPrefab;
    public GameObject DamageNumberCanvas;

    readonly List<DamageEvent> DamageEvents = new();

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        EventManager.AddListener("DamageEvent", AddDamageEvent);
        InvokeRepeating(nameof(OnUpdate), 1, 0.2f);
    }

    public void AddDamageEvent(EventData damageEvent)
    {
        if(damageEvent is DamageEvent de)
        {
            DamageEvents.Add(de);
        }
    }

    void OnUpdate()
    {
        if(DamageEvents.Count == 0)
        {
            return;
        }
        foreach (var damageEvent in DamageEvents)
        {
            var damageNumber = Instantiate(DamageNumberPrefab, damageEvent.target.transform.position, Quaternion.identity, DamageNumberCanvas.transform);
            damageNumber.GetComponentInChildren<DamageText>().Init(damageEvent.damageTaken, GetDamageColor(damageEvent));
        }
        DamageEvents.Clear();
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
