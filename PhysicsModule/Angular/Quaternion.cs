using System;
using System.Runtime.InteropServices;

namespace CoreModule.Angular
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <summary>X component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float X { get; set; }

        /// <summary>Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float Y { get; set; }

        /// <summary>Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float Z { get; set; }

        /// <summary>W component of the Quaternion. Don't modify this directly unless you know quaternions inside out</summary>
        public float W { get; set; }

        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <param name="w">The width</param>
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// The identity rotation (RO). This quaternion corresponds to "no rotation": the object
        /// </summary>
        /// <value>The identity matrix</value>
        public static Quaternion Identity => new Quaternion(0f, 0f, 0f, 1f);

        /// <summary>
        /// Compare quaternion and object and checks if they are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>Object and quaternion are equal</returns>
        public override bool Equals(object obj) => (obj is Quaternion) && Equals((Quaternion)obj);

        /// <summary>
        /// Compare two quaternions and checks if they are equal
        /// </summary>
        /// <param name="other">Quaternion to check</param>
        /// <returns>Quaternions are equal</returns>
        public bool Equals(Quaternion other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

        /// <summary>
        /// Used to allow Quaternions to be used as keys in hash tables
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2) ^ (W.GetHashCode() >> 1);

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString() => $"[Quaternion] X({ X }) Y({ Y }) Z({ Z }) W({ W })";
    }
}