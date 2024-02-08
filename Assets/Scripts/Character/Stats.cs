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
       };
   }

   public Stat GetStat(string name)
   {
       return StatList.Find(x => x.Name == name);
   }
}
