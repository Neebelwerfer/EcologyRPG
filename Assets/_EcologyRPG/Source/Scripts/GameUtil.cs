using EcologyRPG.GameSystems;
using UnityEngine;

namespace EcologyRPG.Scripts
{
    public class GameUtil : MonoBehaviour
    {
        public void StartGame()
        {
            Game.StartGame();
        }

        public void CloseGame()
        {
            Game.ExitGame();
        }
    }
}