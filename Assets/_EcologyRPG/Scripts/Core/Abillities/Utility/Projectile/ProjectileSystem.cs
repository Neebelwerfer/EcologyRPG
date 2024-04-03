using EcologyRPG.Core.Systems;
using System.Collections.Generic;

namespace EcologyRPG.Core.Abilities
{
    public class ProjectileSystem : SystemBehavior, IFixedUpdateSystem
    {
        public static ProjectileSystem Instance;

        public static void Init()
        {
            Instance = new ProjectileSystem();
        }

        public ProjectileSystem() : base()
        {
            behaviours = new List<ProjectileBehaviour>();
        }


        public bool Enabled => behaviours.Count > 0;

        readonly List<ProjectileBehaviour> behaviours;
        readonly Queue<ProjectileBehaviour> toRemove = new Queue<ProjectileBehaviour>();

        public void AddBehaviour(ProjectileBehaviour behaviour)
        {
            behaviours.Add(behaviour);
        }

        public void RemoveBehaviour(ProjectileBehaviour behaviour)
        {
            toRemove.Enqueue(behaviour);
        }

        public void OnFixedUpdate()
        {
            while (toRemove.Count > 0)
            {
                behaviours.Remove(toRemove.Dequeue());
            }

            foreach (var behaviour in behaviours)
            {
                behaviour.OnUpdate();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            behaviours.Clear();
            Instance = null;
        }
    }
}