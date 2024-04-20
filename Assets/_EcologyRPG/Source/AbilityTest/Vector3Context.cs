using MoonSharp.Interpreter;
using UnityEngine;

namespace EcologyRPG.AbilityTest
{
    public class Vector3Context
    {
        float x;
        float y;
        float z;

        [MoonSharpHidden]
        public Vector3Context(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3Context(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [MoonSharpHidden]
        public Vector3 Vector => new Vector3(x, y, z);

        public float X => x;
        public float Y => y;
        public float Z => z;

        public Vector3Context Add(Vector3Context other)
        {
            return new Vector3Context(Vector + other.Vector);
        }

        public Vector3Context Subtract(Vector3Context other)
        {
            return new Vector3Context(Vector - other.Vector);
        }

        public Vector3Context Multiply(float scalar)
        {
            return new Vector3Context(Vector * scalar);
        }

        public Vector3Context Divide(float scalar)
        {
            return new Vector3Context(Vector / scalar);
        }

        public Vector3Context Normalize()
        {
            return new Vector3Context(Vector3.Normalize(Vector));
        }

        public static Vector3Context _Vector3(float x, float y, float z)
        {
            return new Vector3Context(x, y, z);
        }

        public override string ToString()
        {
            return Vector.ToString();
        }

        public float Magnitude()
        {
            return Vector.magnitude;
        }
    }
}