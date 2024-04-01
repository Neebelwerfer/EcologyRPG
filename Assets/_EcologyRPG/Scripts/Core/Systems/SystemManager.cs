using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Core.Systems
{
    public class SystemManager
    {
        public static SystemManager Instance;
        SystemManager() { }

        readonly List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();
        readonly List<ILateUpdateSystem> lateUpdateSystems = new List<ILateUpdateSystem>();
        readonly List<IFixedUpdateSystem> fixedUpdateSystems = new List<IFixedUpdateSystem>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            Instance = new SystemManager();
        }

        void AddSystem(SystemBehavior system)
        {
            if (system is IUpdateSystem updateSystem)
            {
                updateSystems.Add(updateSystem);
            }
            if (system is ILateUpdateSystem lateUpdateSystem)
            {
                lateUpdateSystems.Add(lateUpdateSystem);
            }
            if (system is IFixedUpdateSystem fixedUpdateSystem)
            {
                fixedUpdateSystems.Add(fixedUpdateSystem);
            }

        }

        void RemoveSystem(SystemBehavior system)
        {
            if (system is IUpdateSystem updateSystem)
            {
                updateSystems.Remove(updateSystem);
            }
            if (system is ILateUpdateSystem lateUpdateSystem)
            {
                lateUpdateSystems.Remove(lateUpdateSystem);
            }
            if (system is IFixedUpdateSystem fixedUpdateSystem)
            {
                fixedUpdateSystems.Remove(fixedUpdateSystem);
            }
        }

        public static void Add(SystemBehavior system)
        {
            Instance.AddSystem(system);
        }
        public static void Remove(SystemBehavior system)
        {
            Instance.RemoveSystem(system);
        }
        public static void Update()
        {
            foreach (var system in Instance.updateSystems)
            {
                if (!system.Enabled)
                    break;

                system.OnUpdate();
            }
        }

        public static void FixedUpdate()
        {
            foreach (var system in Instance.fixedUpdateSystems)
            {
                if (!system.Enabled)
                    break;

                system.OnFixedUpdate();
            }
        }

        public static void LateUpdate()
        {
            foreach (var system in Instance.lateUpdateSystems)
            {
                if (!system.Enabled)
                    break;

                system.OnLateUpdate();
            }

            Instance.updateSystems.Sort((a, b) =>
            {
                if(a.Enabled && !b.Enabled)
                {
                    return -1;
                }

                if(!a.Enabled && b.Enabled)
                {
                    return 1;
                }

                return 0;
            });

            Instance.fixedUpdateSystems.Sort((a, b) =>
            {
                if(a.Enabled && !b.Enabled)
                {
                    return -1;
                }

                if(!a.Enabled && b.Enabled)
                {
                    return 1;
                }

                return 0;
            });

            Instance.lateUpdateSystems.Sort((a, b) =>
            {
                if(a.Enabled && !b.Enabled)
                {
                    return -1;
                }

                if(!a.Enabled && b.Enabled)
                {
                    return 1;
                }

                return 0;
            });
        }
    }
}
