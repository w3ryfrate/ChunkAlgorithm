using System;
using System.CodeDom;

namespace Minesharp.Logic
{
    public struct Vector3Int : IEquatable<Vector3Int>
    {
        public int X;
        public int Y;
        public int Z;

        public static readonly Vector3Int Zero = new(0);

        public Vector3Int()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vector3Int(int val)
        {
            X = val;
            Y = val;
            Z = val;
        }
        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override readonly string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public readonly Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is not Vector3Int vector)
            {
                return false;
            }

            if (X == vector.X && Y == vector.Y)
            {
                return Z == vector.Z;
            }

            return false;
        }

        public readonly bool Equals(Vector3Int other)
        {
            if (X == other.X && Y == other.Y)
            {
                return Z == other.Z;
            }

            return false;
        }

        public static bool operator ==(Vector3Int value1, Vector3Int value2)
        {
            if (value1.X == value2.X && value1.Y == value2.Y)
            {
                return value1.Z == value2.Z;
            }

            return false;
        }

        public static bool operator !=(Vector3Int value1, Vector3Int value2)
        {
            return !(value1 == value2);
        }

        public static Vector3Int operator +(Vector3Int value1, Vector3Int value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        public static Vector3Int operator -(Vector3Int value)
        {
            value = new Vector3Int(0 - value.X, 0 - value.Y, 0 - value.Z);
            return value;
        }

        public static Vector3Int operator -(Vector3Int value1, Vector3Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        public static Vector3Int operator *(Vector3Int value1, Vector3Int value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }

        public static Vector3Int operator *(Vector3Int value, int scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }

        public static Vector3Int operator *(int scaleFactor, Vector3Int value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }

        public static Vector3Int operator /(Vector3Int value1, Vector3Int value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }
    }
}
