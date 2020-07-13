using System;
using System.Runtime.InteropServices;

namespace Kinematics.Common
{
    /// <summary>
    /// Vector2 is an utility class for manipulating 2 dimensional
    /// vectors with float components
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IEquatable<Vector2>
    {
        #region Properties

        /// <summary>X (horizontal) component of the vector</summary>
        public float X;

        /// <summary>Y (vertical) component of the vector</summary>
        public float Y;

        /// <summary>
        /// Shorthand for writing Vector2(0, 1)
        /// </summary>
        /// <returns>Vector up</returns>
        public static readonly Vector2 Up = new Vector2(1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector2(0, -1)
        /// </summary>
        /// <returns>Vector down</returns>
        public static readonly Vector2 Down = new Vector2(0f, -1f);

        /// <summary>
        /// Shorthand for writing Vector2(-1, 0)
        /// </summary>
        /// <returns>Vector left</returns>
        public static readonly Vector2 Left = new Vector2(-1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector2(1, 0)
        /// </summary>
        /// <returns>Vector right</returns>
        public static readonly Vector2 Right = new Vector2(1f, 0f);

        /// <summary>
        /// Shorthand for writing Vector2(1, 1)
        /// </summary>
        /// <returns>Vector one</returns>
        public static readonly Vector2 One = new Vector2(1f, 1f);

        /// <summary>
        /// Shorthand for writing Vector2(0, 0)
        /// </summary>
        /// <returns>Vector zero</returns>
        public static readonly Vector2 Zero = new Vector2(0f, 0f);
        #endregion

        #region Construtors
        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="v">Vector2</param>
        public Vector2(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Operator - overload ; returns the opposite of a vector
        /// </summary>
        /// <param name="v">Vector to negate</param>
        /// <returns>-v</returns>
        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// Operator + overload ; add two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 + v2</returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="x">Scalar value</param>
        /// <returns>v * x</returns>
        public static Vector2 operator *(Vector2 v, float x)
        {
            return new Vector2(v.X * x, v.Y * x);
        }

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="x">Scalar value</param>
        /// <param name="v">Vector</param>
        /// <returns>x * v</returns>
        public static Vector2 operator *(float x, Vector2 v)
        {
            return new Vector2(v.X * x, v.Y * x);
        }

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="x">Scalar value</param>
        /// <returns>v / x</returns>
        public static Vector2 operator /(Vector2 v, float x)
        {
            return new Vector2(v.X / x, v.Y / x);
        }

        /// <summary>
        /// Performs an explicit conversion from Vector3 to Vector4D
        /// </summary>
        /// <param name="v">The value</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3(Vector2 v)
        {
            return new Vector3(v, 0f);
        }

        /// <summary>
        /// Operator == overload ; check vector equality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 == v2</returns>
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Operator != overload ; check vector inequality
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 != v2</returns>
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !v1.Equals(v2);
        }
        #endregion

        /// <summary>
        /// Compare vector and object and checks if they are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>Object and vector are equal</returns>
        public override bool Equals(object obj)
        {
            return obj is Vector2 v && Equals(v);
        }

        /// <summary>
        /// Compare two vectors and checks if they are equal
        /// </summary>
        /// <param name="other">Vector to check</param>
        /// <returns>Vectors are equal</returns>
        public bool Equals(Vector2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        /// <summary>
        ///  Rotate a vector by a given angle (in radians).
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="r">Angle in radians</param>
        /// <returns>Rotated vector</returns>
        public static Vector2 Rotate(Vector2 v, float r)
        {
            Vector2 ret = new Vector2();
            float cos = Mathf.Cos(r);
            float sin = Mathf.Sin(r);
            ret.X = cos * v.X - sin * v.Y;
            ret.Y = cos * v.Y + sin * v.X;

            return ret;
        }

        /// <summary>
        /// Rotate a vector by a given angle (reference type version)
        /// </summary>
        /// <param name="vIn">Vector to rotate</param>
        /// <param name="r">Angle in radians</param>
        /// <param name="vOut">Rotated vector</param>
        public static void Rotate(ref Vector2 vIn, float r, ref Vector2 vOut)
        {
            float cos = Mathf.Cos(r);
            float sin = Mathf.Sin(r);
            vOut.X = cos * vIn.X - sin * vIn.Y;
            vOut.Y = cos * vIn.Y + sin * vIn.X;
        }

        /// <summary>
        /// Rotate a given vector by a given angle (reference type version)
        /// </summary>
        /// <param name="v">Vector to rotate</param>
        /// <param name="r">Angle in radians</param>
        public static void Rotate(ref Vector2 v, float r)
        {
            float originalX = v.X;
            float originalY = v.Y;
            float cos = Mathf.Cos(r);
            float sin = Mathf.Sin(r);
            v.X = cos * originalX - sin * originalY;
            v.Y = cos * originalY + sin * originalX;
        }


        /// <summary>
        /// Get a vector perpendicular to this vector.
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>Perpendicular vector</returns>
        public static Vector2 Perpendicular(Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        /// <summary>
        /// Get a vector perpendicular to this vector (reference type version)
        /// </summary>
        /// <param name="vIn">Vector int</param>
        /// <param name="vOut">Perpendicular vector out</param>
        public static void Perpendicular(ref Vector2 vIn, ref Vector2 vOut)
        {
            vOut.X = -vIn.Y;
            vOut.Y = vIn.X;
        }

        /// <summary>
        /// Make this vector perpendicular to itself
        /// </summary>
        /// <param name="v">Vector in/out</param>
        public static void Perpendicular(ref Vector2 v)
        {
            float tempX = v.X;
            v.X = -v.Y;
            v.Y = tempX;
        }

        /// <summary>
        /// Is rotating from A to B Counter-clockwise?
        /// </summary>
        /// <param name="v1">Vector A</param>
        /// <param name="v2">Vector B</param>
        /// <returns>true = CCW or opposite (180 degrees), false = CW</returns>
        public static bool IsCCW(Vector2 v1, Vector2 v2)
        {
            Vector2 perp = Perpendicular(v1);
            Vector2.Dot(ref v2, ref perp, out float dot);

            return dot >= 0.0f;
        }

        /// <summary>
        /// Is rotating from A to B Counter-Clockwise?
        /// </summary>
        /// <param name="v1">Vector A</param>
        /// <param name="v2">Vector B</param>
        /// <returns>true = CCW or opposite (180 degrees), false = CW</returns>
        public static bool IsCCW(ref Vector2 v1, ref Vector2 v2)
        {
            Vector2 perp = new Vector2();
            Perpendicular(ref v1, ref perp);
            Vector2.Dot(ref v2, ref perp, out float dot);

            return dot >= 0.0f;
        }

        /// <summary>
        /// Turn a Vector2 into a Vector3 (sets Z component to zero)
        /// </summary>
        /// <param name="v">Input Vector2</param>
        /// <returns>Result Vector3</returns>
        public static Vector3 Vector3FromVector2(Vector2 v)
        {
            return new Vector3(v.X, v.Y, 0);
        }

        /// <summary>
        /// Turn a Vector2 into a Vector3 (sets Z component to zero) (reference type version)
        /// </summary>
        /// <param name="v">Input Vector2</param>
        /// <returns>Result Vector3</returns>
        public static Vector3 Vector3FromVector2(ref Vector2 v)
        {
            return new Vector3(v.X, v.Y, 0);
        }

        /// <summary>
        /// Turn a Vector2 into a Vector3, specifying the Z component to use.
        /// </summary>
        /// <param name="v">Input Vector2</param>
        /// <param name="z">Z component</param>
        /// <returns>Result Vector3</returns>
        public static Vector3 Vector3FromVector2(Vector2 v, float z)
        {
            return new Vector3(v.X, v.Y, z);
        }

        /// <summary>
        /// Turn a Vector2 into a Vector3, specifying the Z component to use.
        /// </summary>
        /// <param name="v">Input Vector2</param>
        /// <param name="z">Z component</param>
        /// <returns>Result Vector3</returns>
        public static Vector3 Vector3FromVector2(ref Vector2 v, float z)
        {
            return new Vector3(v.X, v.Y, z);
        }

        /// <summary>
        /// See if 2 line segments intersect. (line AB collides with line CD)
        /// </summary>
        /// <param name="ptA">First point on line AB</param>
        /// <param name="ptB">Second point on line AB</param>
        /// <param name="ptC">First point on line CD</param>
        /// <param name="ptD">Second point on line CD</param>
        /// <param name="hitPt">Resulting point of intersection</param>
        /// <param name="ua">Distance along AB to intersection [0,1]</param>
        /// <param name="ub">Distance long CD to intersection [0,1]</param>
        /// <returns>true or false</returns>
        public static bool LineIntersect(Vector2 ptA, Vector2 ptB, Vector2 ptC, Vector2 ptD, out Vector2 hitPt, out float ua, out float ub)
        {
            hitPt = Zero;
            ua = 0f;
            ub = 0f;

            float denom = (ptD.Y - ptC.Y) * (ptB.X - ptA.X) - (ptD.X - ptC.X) * (ptB.Y - ptA.Y);
            if (Mathf.Abs(denom) < Mathf.Epsilon)
            {
                return false;
            }

            float uaTop = (ptD.X - ptC.X) * (ptA.Y - ptC.Y) - (ptD.Y - ptC.Y) * (ptA.X - ptC.X);
            float ubTop = (ptB.X - ptA.X) * (ptA.Y - ptC.Y) - (ptB.Y - ptA.Y) * (ptA.X - ptC.X);

            ua = uaTop / denom;
            ub = ubTop / denom;

            if (ua >= 0f && ua <= 1f && ub >= 0f && ub <= 1f)
            {
                hitPt = ptA + ((ptB - ptA) * ua);
                return true;
            }

            return false;
        }

        /// <summary>
        /// See if 2 line segments intersect. (line AB collides with line CD) (reference type version)
        /// </summary>
        /// <param name="ptA">First point on line AB</param>
        /// <param name="ptB">Second point on line AB</param>
        /// <param name="ptC">First point on line CD</param>
        /// <param name="ptD">Second point on line CD</param>
        /// <param name="hitPt">Resulting point of intersection</param>
        /// <param name="ua">Distance along AB to intersection [0,1]</param>
        /// <param name="ub">Distance long CD to intersection [0,1]</param>
        /// <returns>true or false</returns>
        public static bool LineIntersect(ref Vector2 ptA, ref Vector2 ptB, ref Vector2 ptC, ref Vector2 ptD, out Vector2 hitPt, out float ua, out float ub)
        {
            hitPt = Zero;
            ua = 0f;
            ub = 0f;

            float denom = ((ptD.Y - ptC.Y) * (ptB.X - ptA.X)) - ((ptD.X - ptC.X) * (ptB.Y - ptA.Y));
            if (Mathf.Abs(denom) < Mathf.Epsilon)
            {
                return false;
            }

            float uaTop = (ptD.X - ptC.X) * (ptA.Y - ptC.Y) - (ptD.Y - ptC.Y) * (ptA.X - ptC.X);
            float ubTop = (ptB.X - ptA.X) * (ptA.Y - ptC.Y) - (ptB.Y - ptA.Y) * (ptA.X - ptC.X);

            ua = uaTop / denom;
            ub = ubTop / denom;

            if (ua >= 0f && ua <= 1f && ub >= 0f && ub <= 1f)
            {
                hitPt = ptA + ((ptB - ptA) * ua);
                return true;
            }

            return false;
        }

        /// <summary>
        /// See if 2 line segments intersect. (line AB collides with line CD) - simplified version
        /// </summary>
        /// <param name="ptA">First point on line AB</param>
        /// <param name="ptB">Second point on line AB</param>
        /// <param name="ptC">First point on line CD</param>
        /// <param name="ptD">Second point on line CD</param>
        /// <param name="hitPt">Resulting point of intersection</param>
        /// <returns>true or false</returns>
        public static bool LineIntersect(Vector2 ptA, Vector2 ptB, Vector2 ptC, Vector2 ptD, out Vector2 hitPt)
        {
            return LineIntersect(ptA, ptB, ptC, ptD, out hitPt, out float _, out float _);
        }

        /// <summary>
        /// See if 2 line segments intersect. (line AB collides with line CD) - simplified version (reference type version)
        /// </summary>
        /// <param name="ptA">First point on line AB</param>
        /// <param name="ptB">Second point on line AB</param>
        /// <param name="ptC">First point on line CD</param>
        /// <param name="ptD">Second point on line CD</param>
        /// <param name="hitPt">Resulting point of intersection</param>
        /// <returns>true or false</returns>
        public static bool LineIntersect(ref Vector2 ptA, ref Vector2 ptB, ref Vector2 ptC, ref Vector2 ptD, out Vector2 hitPt)
        {
            return LineIntersect(ref ptA, ref ptB, ref ptC, ref ptD, out hitPt, out float _, out float _);
        }

        /// <summary>
        /// Provide a integer describing the object
        /// </summary>
        /// <returns>Integer description of the object</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString()
        {
            return $"[Vector2] X({X}) Y({Y})";
        }
    }
}