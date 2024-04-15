using EcologyRPG.GameSystems;
using EcologyRPG.GameSystems.PlayerSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRGP.Scripts
{
    public class MinimapCamera : MonoBehaviour
    {
        PlayerCharacter player;
        void Start()
        {
            player = Player.PlayerCharacter;
        }

        void Update()
        {
            transform.position = new Vector3(player.Transform.Position.x, transform.position.y, player.Transform.Position.z);
        }
    }
}
