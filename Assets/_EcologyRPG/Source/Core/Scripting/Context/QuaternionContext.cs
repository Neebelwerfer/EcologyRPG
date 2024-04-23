using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using UnityEngine;

namespace EcologyRPG.Core.Scripting
{
    public class QuaternionContext
    {
        [MoonSharpHidden]
        public Quaternion Value;

        public static void Register(Script script)
        {
            script.Globals["Quaternion"] = UserData.CreateStatic<QuaternionContext>();
        }

        public QuaternionContext(Quaternion value)
        {
            Value = value;
        }

        public void LookAt(Vector3 dir)
        {
            Value = Quaternion.LookRotation(dir);
        }

        public static QuaternionContext Identity()
        {
            return new QuaternionContext(Quaternion.identity);
        }

        public static QuaternionContext FromLookat(Vector3 dir)
        {
            return new QuaternionContext(Quaternion.LookRotation(dir));
        }

        public static QuaternionContext FromCharacter(BaseCharacter character)
        {
            return new QuaternionContext(character.Transform.Rotation);
        }
    }
}