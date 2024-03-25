using Character;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class DamageNumberHandler : MonoBehaviour
{
    static DamageNumberHandler _instance;
    public static DamageNumberHandler Instance { get { return _instance; } }

    public GameObject DamageNumberPrefab;
    public GameObject DamageNumberCanvas;
    GameObjectPool damageNumberPool;
    readonly List<DamageText> damageNumbers = new();
    readonly List<DamageEvent> damageEvents = new();

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
        damageNumberPool = new GameObjectPool(DamageNumberPrefab);
        damageNumberPool.Preload(20, DamageNumberCanvas.transform);
        EventManager.AddListener("DamageEvent", AddDamageEvent);
    }

    public void AddDamageEvent(EventData damageEvent)
    {
        if(damageEvent is DamageEvent de)
        {
            damageEvents.Add(de);
        }
    }

    private void Update()
    {
        foreach (var de in damageEvents)
        {
            var damageNumber = damageNumberPool.GetObject(de.Point, Quaternion.identity);
            damageNumber.transform.SetParent(DamageNumberCanvas.transform);
            var damageText = damageNumber.GetComponent<DamageText>();
            damageText.Init(de.damageTaken, GetDamageColor(de));
            damageNumbers.Add(damageText);
        }
        damageEvents.Clear();

        for (int i = damageNumbers.Count - 1; i >= 0; i--)
        {
            var damageNumber = damageNumbers[i];
            if (damageNumber == null)
            {
                damageNumbers.RemoveAt(i);
            }
            damageNumber.OnUpdate();
            if(damageNumber.RemainingDuration <= 0)
            {
                damageNumbers.RemoveAt(i);
                damageNumberPool.ReturnObject(damageNumber.gameObject);
            }
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
