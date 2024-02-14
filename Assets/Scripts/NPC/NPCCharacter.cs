using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCharacter : BaseCharacter
{
    
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
