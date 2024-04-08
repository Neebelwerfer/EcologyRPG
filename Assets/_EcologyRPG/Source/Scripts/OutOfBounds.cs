using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRGP.Scripts
{
    public class OutOfBounds : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if(GameUtility.IsLayerInLayerMask(collision.gameObject.layer, Game.Settings.EntityMask))
            {
                if(collision.gameObject.TryGetComponent(out CharacterBinding entity))
                {
                    entity.Character.Die();
                }
            }
        }
    }
}
