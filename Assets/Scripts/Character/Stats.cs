using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
   List<Stat> StatList;
   public Stats()
   {
       StatList = new List<Stat>()
       {
           new Stat("Movement Speed", 100f, "Governs the characters movement speed."), 
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
