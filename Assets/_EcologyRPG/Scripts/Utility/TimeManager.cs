using UnityEngine;

namespace EcologyRPG.Utility
{
    public static class TimeManager
    {
        static float MenuTimeScale = 1f;
        static float IngameTimeScale = 1f;

        public static float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
        }

        public static float IngameDeltaTime
        {
            get
            {
                return Time.deltaTime * IngameTimeScale;
            }
        }

        public static float MenuDeltaTime
        {
            get
            {
                return Time.deltaTime * MenuTimeScale;
            }
        }

        public static float DefaultDeltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }
    }
}
