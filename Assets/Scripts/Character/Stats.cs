using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
   List<Stat> StatList;
   public Stats()
   {
       StatList = new List<Stat>()
       {
           new Stat("movementSpeed", 10f, "Governs the characters movement speed", "Movement Speed"), 
           new Stat("maxHP", 100f, "Governs the characters max hp value", "Max HP"), 
           new Stat("maxStamina", 100f, "Governs how much stamina the character have", "Max Stamina"), 
           new Stat("staminaDrain", 25f, "Governs how much stamina is lost per second of sprint", "Stamina Drain"), 
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
       return null;
   }
}
