using System.Collections.Generic;
using UnityEngine;

public class Stats
{
   List<Stat> StatList;
   public Stats()
   {
       StatList = new List<Stat>()
       {
           new Stat("movementSpeed", 5f, "Governs the characters movement speed", "Movement Speed"), 
           new Stat("maxHP", 100f, "Governs the characters max hp value", "Max HP"), 
           new Stat("maxStamina", 100f, "Governs how much stamina the character have", "Max Stamina"), 
           new Stat("staminaDrain", 25f, "Governs how much stamina is lost per second of sprint", "Stamina Drain"), 
           new Stat("staminaGain", 10f, "Governs how fast the character gets stamina back", "Stamina Gain"), 
       };
   }

   public Stat GetStat(string name)
   {
       foreach (Stat stat in StatList)
       {
           if (stat.Name == name)
           {
               return stat;
           }
       }
       Debug.LogError("Couldn't find Stat " + name);
       return null;
   }
}
