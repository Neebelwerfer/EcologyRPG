using System;

namespace EcologyRPG.Core.Systems
{
    public abstract class SystemBehavior : IDisposable
    {
        protected SystemBehavior()
        {
            SystemManager.Add(this);
        }

        public virtual void Dispose()
        {
            SystemManager.Remove(this);
        }
    }
}