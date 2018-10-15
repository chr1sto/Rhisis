using Rhisis.Core.Helpers;
using System;

namespace Rhisis.Core.Structures
{
    /// <summary>
    /// Represents 3D coordinates in space.
    /// </summary>
    public class Vector3 : IEquatable<Vector3>
    {
        private float _x;
        private float _y;
        private float _z;

        /// <summary>
        /// Gets or sets the X position in the world.
        /// </summary>
        public float X
        {
            get => this._x;
            set => this._x = value;
        }

        /// <summary>
        /// Gets or sets the Y position in the world.
        /// </summary>
        public float Y
        {
            get => this._y;
            set => this._y = value;
        }

        /// <summary>
        /// Gets or sets the Z position in the world.
        /// </summary>
        public float Z
        {
            get => this._z;
            set => this._z = value;
        }

        /// <summary>
        /// Gets the vector length.
        /// </summary>
        public float Length => (float)Math.Sqrt(this.SquaredLength);

        /// <summary>
        /// Gets the vector squared length.
        /// </summary>
        public float SquaredLength => (this._x * this._x) + (this._y * this._y) + (this._z * this._z);

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
            this._x = x;
            this._y = y;
            this._z = z;
        }

        /// <summary>
        /// Creates a new Vector3 with specific string values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(string x, string y, string z)
        {
            float.TryParse(x, out this._x);
            float.TryParse(y, out this._y);
            float.TryParse(z, out this._z);
        }

        /// <summary>
        /// Gets the 2D distance between this position and the other position passed as parameter.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public double GetDistance2D(Vector3 otherPosition)
        {
            return Math.Sqrt(Math.Pow(otherPosition.X - this._x, 2) + Math.Pow(otherPosition.Z - this._z, 2));
        }

        /// <summary>
        /// Gets the 2D distance between this position and the other position passed as parameter.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public double GetDistance3D(Vector3 otherPosition)
        {
            return Math.Sqrt(Math.Pow(otherPosition.X - this._x, 2) + Math.Pow(otherPosition.Y - this._y, 2) + Math.Pow(otherPosition.Z - this._z, 2));
        }

        /// <summary>
        /// Check if the other position is in the circleRadius of this position.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <param name="circleRadius"></param>
        /// <returns></returns>
        public bool IsInCircle(Vector3 otherPosition, float circleRadius)
        {
            return Math.Pow(otherPosition.X - this._x, 2) + Math.Pow(otherPosition.Z - this._z, 2) < circleRadius;
        }

        /// <summary>
        /// Check if this <see cref="Vector3"/> instersects a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle rectangle, float radius)
        {
            var deltaX = this._x - Math.Max(rectangle.X, Math.Min(this._x, rectangle.X + rectangle.Width));
            var deltaY = this._z - Math.Max(rectangle.Z, Math.Min(this._z, rectangle.Z + rectangle.Length));

            return (deltaX * deltaX + deltaY * deltaY) < (radius * radius);
        }

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        /// <returns></returns>
        public Vector3 Normalize()
        {
            var sqLength = this.SquaredLength;

            if (sqLength <= 0)
                throw new InvalidOperationException("Cannot normalize a vector of zero length.");

            return this / (float)Math.Sqrt(sqLength);
        }

        /// <summary>
        /// Clones this Vector3 instance.
        /// </summary>
        /// <returns></returns>
        public Vector3 Clone() => new Vector3(this._x, this._y, this._z);

        /// <summary>
        /// Reset to 0 this Vector3.
        /// </summary>
        public void Reset()
        {
            this._x = 0;
            this._y = 0;
            this._z = 0;
        }

        /// <summary>
        /// Check if the Vector3 is zero.
        /// </summary>
        /// <returns></returns>
        public bool IsZero() => this.SquaredLength <= 0;

        /// <summary>
        /// Returns a vector3 under string format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Vector3: {0}:{1}:{2}", this._x, this._y, this._z);
        }

        /// <summary>
        /// Returns the HashCode for this Vector3D
        /// </summary>
        /// <returns> 
        /// int - the HashCode for this Vector3D
        /// </returns> 
        public override int GetHashCode()
        {
            return this._x.GetHashCode() ^
                   this._y.GetHashCode() ^
                   this._z.GetHashCode();
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
        /// Add two Vecto3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>
        /// Substract two Vector3.
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
        /// Be carefull with the <see cref="DivideByZeroException"/>.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 a, Vector3 b) => new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        /// <summary>
        /// Devides a vector by a scalar number.
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
                angle += 360;
            else if (angle >= 360)
                angle -= 360;

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
        /// Compares two <see cref="Vector3"/> objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Vector3 other) => this == other;
    }
}