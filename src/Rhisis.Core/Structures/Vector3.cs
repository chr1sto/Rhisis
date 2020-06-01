using Rhisis.Core.Helpers;
using System;

namespace Rhisis.Core.Structures
{
    /// <summary>
    /// Represents 3D coordinates in space.
    /// </summary>
    public class Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// Gets or sets the X position in the world.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y position in the world.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the Z position in the world.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Gets the vector length.
        /// </summary>
        public float Length => (float)Math.Sqrt(SquaredLength);

        /// <summary>
        /// Gets the vector squared length.
        /// </summary>
        public float SquaredLength => (X * X) + (Y * Y) + (Z * Z);

        /// <summary>
        /// Creates a new Vector3 initialized to 0.
        /// </summary>
        public Vector3()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Creates a new Vector3 with specific values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a new Vector3 with specific string values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(string x, string y, string z)
        {
            X = Convert.ToSingle(x);
            Y = Convert.ToSingle(y);
            Z = Convert.ToSingle(z);
        }

        /// <summary>
        /// Gets the 2D distance between this position and the other position passed as parameter.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public double GetDistance2D(Vector3 otherPosition)
        {
            return Math.Sqrt(Math.Pow(otherPosition.X - X, 2) + Math.Pow(otherPosition.Z - Z, 2));
        }

        /// <summary>
        /// Gets the 2D distance between this position and the other position passed as parameter.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public double GetDistance3D(Vector3 otherPosition)
        {
            return Math.Sqrt(Math.Pow(otherPosition.X - X, 2) + Math.Pow(otherPosition.Y - Y, 2) + Math.Pow(otherPosition.Z - Z, 2));
        }

        /// <summary>
        /// Check if the other position is in the circleRadius of this position.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <param name="circleRadius"></param>
        /// <returns></returns>
        public bool IsInCircle(Vector3 otherPosition, float circleRadius)
        {
            return Math.Pow(otherPosition.X - X, 2) + Math.Pow(otherPosition.Z - Z, 2) < Math.Pow(circleRadius, 2);
        }

        /// <summary>
        /// Check if this <see cref="Vector3"/> intersects a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle rectangle, float radius)
        {
            var deltaX = X - Math.Max(rectangle.X, Math.Min(X, rectangle.X + rectangle.Width));
            var deltaY = Z - Math.Max(rectangle.Z, Math.Min(Z, rectangle.Z + rectangle.Length));

            return (deltaX * deltaX + deltaY * deltaY) < (radius * radius);
        }

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        /// <returns></returns>
        public Vector3 Normalize()
        {
            var sqLength = SquaredLength;

            if (sqLength <= 0)
                throw new InvalidOperationException("Cannot normalize a vector of zero length.");

            return this / (float)Math.Sqrt(sqLength);
        }

        /// <summary>
        /// Clones this Vector3 instance.
        /// </summary>
        /// <returns></returns>
        public Vector3 Clone() => new Vector3(X, Y, Z);

        /// <summary>
        /// Reset to 0 this Vector3.
        /// </summary>
        public void Reset()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Copies the other vector values into the current vector.
        /// </summary>
        /// <param name="otherVector">Other vector.</param>
        public void Copy(Vector3 otherVector)
        {
            X = otherVector.X;
            Y = otherVector.Y;
            Z = otherVector.Z;
        }

        /// <summary>
        /// Check if the Vector3 is zero.
        /// </summary>
        /// <returns></returns>
        public bool IsZero() => SquaredLength <= 0;

        /// <summary>
        /// Returns a vector3 under string format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Vector3: {X}:{Y}:{Z}";
        }

        /// <summary>
        /// Returns the HashCode for this Vector3D
        /// </summary>
        /// <returns> 
        /// int - the HashCode for this Vector3D
        /// </returns> 
        public override int GetHashCode()
        {
            return X.GetHashCode() ^
                   Y.GetHashCode() ^
                   Z.GetHashCode();
        }

        /// <summary>
        /// Compares two vectors.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this == (Vector3)obj;
        }

        /// <summary>
        /// Computes the dot product <c>a · b</c> of the given vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The dot product.</returns>
        /// <remarks>See <a href="https://en.wikipedia.org/wiki/Dot_product" /> for more information.</remarks>
        public static double DotProduct(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Computes the cross product <c>a x b</c> of the given vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>This cross product</returns>
        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        /// <summary>
        /// Add two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>
        /// Subtract two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>
        /// Multiplies two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 a, Vector3 b) => new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        /// <summary>
        /// Multiplies a Vector3 and a float value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.X * b, a.Y * b, a.Z * b);

        /// <summary>
        /// Divides two Vector3.
        /// </summary>
        /// <remarks>
        /// Be careful with the <see cref="DivideByZeroException"/>.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 a, Vector3 b) => new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        /// <summary>
        /// Divides a vector by a scalar number.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 a, float b) => new Vector3(a.X / b, a.Y / b, a.Z / b);

        /// <summary>
        /// Compares two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return Math.Ceiling(a.X - b.X) < 0.01 && Math.Ceiling(a.Y - b.Y) < 0.01 && Math.Ceiling(a.Z - b.Z) < 0.01;
        }

        /// <summary>
        /// Compares two Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Get the angle between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float AngleBetween(Vector3 a, Vector3 b)
        {
            var dist = b - a;
            float angle = (float)Math.Atan2(dist.X, -dist.Z);

            angle = MathHelper.ToDegree(angle);

            if (angle < 0)
            {
                angle += 360;
            }
            else if (angle >= 360)
            {
                angle -= 360;
            }

            return angle;
        }

        /// <summary>
        /// Gets a random position in a circle center based on the position passed as parameter.
        /// </summary>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Circle radius</param>
        /// <returns></returns>
        public static Vector3 GetRandomPositionInCircle(Vector3 center, float radius)
        {
            Vector3 newVector = center.Clone();
            var angle = (float)(RandomHelper.FloatRandom(0f, 360f) * Math.PI / 180f);
            float power = RandomHelper.FloatRandom(0f, radius);

            newVector.X += (float)Math.Sin(angle) * power;
            newVector.Z += (float)Math.Cos(angle) * power;

            return newVector;
        }

        /// <summary>
        /// Gets the 2D distance between two vectors.
        /// </summary>
        /// <param name="from">Origin vector.</param>
        /// <param name="to">Target vector.</param>
        /// <returns>Distance</returns>
        public static float Distance2D(Vector3 from, Vector3 to)
        {
            float x = from.X - to.X;
            float z = from.Z - to.Z;

            return (float)Math.Sqrt(x * x + z * z);
        }

        /// <summary>
        /// Gets the 3D distance between two vectors.
        /// </summary>
        /// <param name="from">Origin vector.</param>
        /// <param name="to">Target vector.</param>
        /// <returns>Distance</returns>
        public static float Distance3D(Vector3 from, Vector3 to)
        {
            float x = from.X - to.X;
            float y = from.Y - to.Y;
            float z = from.Z - to.Z;

            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Compares two <see cref="Vector3"/> objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Vector3 other) => this == other;
    }
}