using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;

namespace EcologyRPG.Core.Scripting
{
    public class StatContext
    {
        Stat Stat;
        public StatContext(Stat stat)
        {
            Stat = stat;
        }

        public float GetValue()
        {
            return Stat.Value;
        }
    }
}