using System;
using System.Runtime.InteropServices;

namespace Kinematics.MathModule
{
    /// <summary>
    /// Vector2f is an utility class for manipulating 2 dimensional
    /// vectors with float components
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IEquatable<Vector2>
    {
        #region Properties
        /// <summary>X (horizontal) component of the vector</summary>
        private float _x;
        public float X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>Y (vertical) component of the vector</summary>
        private float _y;
        public float Y
        {
            get => _y;
            set => _y = value;
        }

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
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="v">Vector2</param>
        public Vector2(Vector2 v)
        {
            _x = v._x;
            _y = v._y;
        }
        
        /// <summary>
        /// Construct the vector from it's coordinates
        /// </summary>
        /// <param name="values">The values to assign to the X and Y components of the vector. This must be an array with three elements</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements</exception>
        public Vector2(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(values), "There must be three and only three input values for Vector2.");
            }

            _x = values[0];
            _y = values[1];
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
            return new Vector2(-v._x, -v._y);
        }

        /// <summary>
        /// Operator - overload ; subtracts two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 - v2</returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x - v2._x, v1._y - v2._y);
        }

        /// <summary>
        /// Operator + overload ; add two vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>v1 + v2</returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1._x + v2._x, v1._y + v2._y);
        }

        /// <summary>
        /// Operator * overload ; multiply a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="x">Scalar value</param>
        /// <returns>v * x</returns>
        public static Vector2 operator *(Vector2 v, float x)
        {
            return new Vector2(v._x * x, v._y * x);
        }

        /// <summary>
        /// Operator * overload ; multiply a scalar value by a vector
        /// </summary>
        /// <param name="x">Scalar value</param>
        /// <param name="v">Vector</param>
        /// <returns>x * v</returns>
        public static Vector2 operator *(float x, Vector2 v)
        {
            return new Vector2(v._x * x, v._y * x);
        }

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="x">Scalar value</param>
        /// <returns>v / x</returns>
        public static Vector2 operator /(Vector2 v, float x)
        {
            return new Vector2(v._x / x, v._y / x);
        }

        /// <summary>
        /// Performs an explicit conversion from Vector3 to Vector4D
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3(Vector2 value)
        {
            return new Vector3(value, 0f);
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
            return obj is Vector2 vector && Equals(vector);
        }

        /// <summary>
        /// Compare two vectors and checks if they are equal
        /// </summary>
        /// <param name="other">Vector to check</param>
        /// <returns>Vectors are equal</returns>
        public bool Equals(Vector2 other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y);
        }

        /// <summary>
        ///  rotate a vector by a given angle (in radians).
        /// </summary>
        /// <param name="vec">vector</param>
        /// <param name="angleRadians">angle in radians</param>
        /// <returns>rotated vector</returns>
        public static Vector2 Rotate(Vector2 vector, float radians)
        {
            Vector2 ret = new Vector2();
            float c = Mathf.Cos(radians);
            float s = Mathf.Sin(radians);
            ret.X = (c * vector.X) - (s * vector.Y);
            ret.Y = (c * vector.Y) + (s * vector.X);

            return ret;
        }

        /// <summary>
        /// rotate a vector by a given angle (reference type version)
        /// </summary>
        /// <param name="vecIn">vector to rotate</param>
        /// <param name="angleRadians">angle in radians</param>
        /// <param name="vecOut">rotated vector</param>
        public static void Rotate(ref Vector2 vectorIn, float radians, ref Vector2 vectorOut)
        {
            float c = Mathf.Cos(radians);
            float s = Mathf.Sin(radians);
            vectorOut.X = (c * vectorIn.X) - (s * vectorIn.Y);
            vectorOut.Y = (c * vectorIn.Y) + (s * vectorIn.X);
        }

        /// <summary>
        /// rotate a given vector by a given angle (reference type version)
        /// </summary>
        /// <param name="vecIn">vector to rotate</param>
        /// <param name="angleRadians">angle in radians</param>
        /// <param name="vecOut">rotated vector</param>
        public static void Rotate(ref Vector2 vector, float radians)
        {
            float originalX = vector.X;
            float originalY = vector.Y;
            float c = Mathf.Cos(radians);
            float s = Mathf.Sin(radians);
            vector.X = (c * originalX) - (s * originalY);
            vector.Y = (c * originalY) + (s * originalX);
        }


        /// <summary>
        /// get a vector perpendicular to this vector.
        /// </summary>
        /// <param name="vec">vector</param>
        /// <returns>perpendicular vector</returns>
        public static Vector2 Perpendicular(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }


        /// <summary>
        /// get a vector perpendicular to this vector (reference type version)
        /// </summary>
        /// <param name="vIn">vector int</param>
        /// <param name="vOut">perpendicular vector out</param>
        public static void Perpendicular(ref Vector2 vectorIn, ref Vector2 vectorOut)
        {
            vectorOut.X = -vectorIn.Y;
            vectorOut.Y = vectorIn.X;
        }

        /// <summary>
        /// make this vector perpendicular to itself
        /// </summary>
        /// <param name="vIn">vector in / out</param>
        public static void Perpendicular(ref Vector2 vector)
        {
            float tempX = vector.X;
            vector.X = -vector.Y;
            vector.Y = tempX;
        }

        /// <summary>
        /// is rotating from A to B Counter-clockwise?
        /// </summary>
        /// <param name="A">vector A</param>
        /// <param name="B">vector B</param>
        /// <returns>true = CCW or opposite (180 degrees), false = CW</returns>
        public static bool IsCCW(Vector2 a, Vector2 b)
        {
            Vector2 perp = Perpendicular(a);
            float dot;
            Vector2.Dot(ref b, ref perp, out dot);
            return (dot >= 0.0f);
        }

        /// <summary>
        /// is rotating from A to B Counter-Clockwise?
        /// </summary>
        /// <param name="A">vector A</param>
        /// <param name="B">vector B</param>
        /// <returns>true = CCW or opposite (180 degrees), false = CW</returns>
        public static bool IsCCW(ref Vector2 A, ref Vector2 B)
        {
            Vector2 perp = new Vector2();
            Perpendicular(ref A, ref perp);

            Vector2.Dot(ref B, ref perp, out float dot);
            return (dot >= 0.0f);
        }

        /// <summary>
        /// turn a Vector2 into a Vector3 (sets Z component to zero)
        /// </summary>
        /// <param name="vec">input Vector2</param>
        /// <returns>result Vector3</returns>
        public static Vector3 Vector3FromVector2(Vector2 vector)
        {
            return new Vector3(vector.X, vector.Y, 0);
        }

        /// <summary>
        /// turn a Vector2 into a Vector3 (sets Z component to zero) (reference type version)
        /// </summary>
        /// <param name="vec">input Vector2</param>
        /// <returns>result Vector3</returns>
        public static Vector3 Vector3FromVector2(ref Vector2 vector)
        {
            return new Vector3(vector.X, vector.Y, 0);
        }

        /// <summary>
        /// turn a Vector2 into a Vector3, specifying the Z component to use.
        /// </summary>
        /// <param name="vec">input Vector2</param>
        /// <param name="Z">Z component</param>
        /// <returns>result Vector3</returns>
        public static Vector3 Vector3FromVector2(Vector2 vector, float z)
        {
            return new Vector3(vector.X, vector.Y, z);
        }

        /// <summary>
        /// turn a Vector2 into a Vector3, specifying the Z component to use.
        /// </summary>
        /// <param name="vec">input Vector2</param>
        /// <param name="Z">Z component</param>
        /// <returns>result Vector3</returns>
        public static Vector3 Vector3FromVector2(ref Vector2 vector, float z)
        {
            return new Vector3(vector.X, vector.Y, z);
        }


        /// <summary>
        /// see if 2 line segments intersect. (line AB collides with line CD)
        /// </summary>
        /// <param name="ptA">first point on line AB</param>
        /// <param name="ptB">second point on line AB</param>
        /// <param name="ptC">first point on line CD</param>
        /// <param name="ptD">second point on line CD</param>
        /// <param name="hitPt">resulting point of intersection</param>
        /// <param name="Ua">distance along AB to intersection [0,1]</param>
        /// <param name="Ub">distance long CD to intersection [0,1]</param>
        /// <returns>true / false</returns>
        public static bool LineIntersect(Vector2 ptA, Vector2 ptB, Vector2 ptC, Vector2 ptD, out Vector2 hitPt, out float Ua, out float Ub)
        {
            hitPt = Vector2.Zero;
            Ua = 0f;
            Ub = 0f;

            float denom = ((ptD.Y - ptC.Y) * (ptB.X - ptA.X)) - ((ptD.X - ptC.X) * (ptB.Y - ptA.Y));
            if (Mathf.Abs(denom) < Mathf.Epsilon)
            {
                return false;
            }

            float UaTop = ((ptD.X - ptC.X) * (ptA.Y - ptC.Y)) - ((ptD.Y - ptC.Y) * (ptA.X - ptC.X));
            float UbTop = ((ptB.X - ptA.X) * (ptA.Y - ptC.Y)) - ((ptB.Y - ptA.Y) * (ptA.X - ptC.X));

            Ua = UaTop / denom;
            Ub = UbTop / denom;

            if ((Ua >= 0f) && (Ua <= 1f) && (Ub >= 0f) && (Ub <= 1f))
            {
                hitPt = ptA + ((ptB - ptA) * Ua);
                return true;
            }

            return false;
        }

        /// <summary>
        /// see if 2 line segments intersect. (line AB collides with line CD) (reference type version)
        /// </summary>
        /// <param name="ptA">first point on line AB</param>
        /// <param name="ptB">second point on line AB</param>
        /// <param name="ptC">first point on line CD</param>
        /// <param name="ptD">second point on line CD</param>
        /// <param name="hitPt">resulting point of intersection</param>
        /// <param name="Ua">distance along AB to intersection [0,1]</param>
        /// <param name="Ub">distance long CD to intersection [0,1]</param>
        /// <returns>true / false</returns>
        public static bool LineIntersect(ref Vector2 ptA, ref Vector2 ptB, ref Vector2 ptC, ref Vector2 ptD, out Vector2 hitPt, out float Ua, out float Ub)
        {
            hitPt = Vector2.Zero;
            Ua = 0f;
            Ub = 0f;

            float denom = ((ptD.Y - ptC.Y) * (ptB.X - ptA.X)) - ((ptD.X - ptC.X) * (ptB.Y - ptA.Y));
            if (Mathf.Abs(denom) < Mathf.Epsilon)
            {
                return false;
            }

            float UaTop = ((ptD.X - ptC.X) * (ptA.Y - ptC.Y)) - ((ptD.Y - ptC.Y) * (ptA.X - ptC.X));
            float UbTop = ((ptB.X - ptA.X) * (ptA.Y - ptC.Y)) - ((ptB.Y - ptA.Y) * (ptA.X - ptC.X));

            Ua = UaTop / denom;
            Ub = UbTop / denom;

            if ((Ua >= 0f) && (Ua <= 1f) && (Ub >= 0f) && (Ub <= 1f))
            {
                hitPt = ptA + ((ptB - ptA) * Ua);
                return true;
            }

            return false;
        }

        /// <summary>
        /// see if 2 line segments intersect. (line AB collides with line CD) - simplified version
        /// </summary>
        /// <param name="ptA">first point on line AB</param>
        /// <param name="ptB">second point on line AB</param>
        /// <param name="ptC">first point on line CD</param>
        /// <param name="ptD">second point on line CD</param>
        /// <param name="hitPt">resulting point of intersection</param>
        /// <returns>true / false</returns>
        public static bool LineIntersect(Vector2 ptA, Vector2 ptB, Vector2 ptC, Vector2 ptD, out Vector2 hitPt)
        {
            return LineIntersect(ptA, ptB, ptC, ptD, out hitPt, out float _, out float _);
        }

        /// <summary>
        /// see if 2 line segments intersect. (line AB collides with line CD) - simplified version (reference type version)
        /// </summary>
        /// <param name="ptA">first point on line AB</param>
        /// <param name="ptB">second point on line AB</param>
        /// <param name="ptC">first point on line CD</param>
        /// <param name="ptD">second point on line CD</param>
        /// <param name="hitPt">resulting point of intersection</param>
        /// <returns>true / false</returns>
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
            return _x.GetHashCode() ^ _y.GetHashCode();
        }

        /// <summary>
        /// Provide a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString()
        {
            return $"[Vector2] X({_x}) Y({_y})";
        }
    }
}