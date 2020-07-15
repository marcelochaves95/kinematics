using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kinematics.Math
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

        /// <summary>
        /// Construct the vector from its coordinates
        /// </summary>
        /// <param name="v">Vector3</param>
        public Vector2(Vector3 v)
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
        /// Scales a vector by the given value
        /// </summary>
        /// <param name="v1">The vector to scale</param>
        /// <param name="v2">The amount by which to scale the vector</param>
        /// <returns>The scaled vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
        }

        /// <summary>
        /// Operator / overload ; divide a vector by a scalar value
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="x">Scalar value</param>
        /// <returns>v / x</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 v, float x)
        {
            return new Vector2(v.X / x, v.Y / x);
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

        /// <summary>
        /// Performs an explicit conversion from Vector2 to Vector3
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>The result of the conversion</returns>
        public static explicit operator Vector3(Vector2 v)
        {
            return new Vector3(v);
        }

        #endregion

        #region Overrides

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle
        /// </summary>
        /// <param name="v1">The first vector of 2d-triangle</param>
        /// <param name="v2">The second vector of 2d-triangle</param>
        /// <param name="v3">The third vector of 2d-triangle</param>
        /// <param name="a1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle</param>
        /// <param name="a2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle</param>
        /// <returns>The cartesian translation of barycentric coordinates</returns>
        public static Vector2 Barycentric(Vector2 v1, Vector2 v2, Vector2 v3, float a1, float a2)
        {
            return new Vector2(Mathf.Barycentric(v1.X, v2.X, v3.X, a1, a2), Mathf.Barycentric(v1.Y, v2.Y, v3.Y, a1, a2));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains CatmullRom interpolation of the specified vectors
        /// </summary>
        /// <param name="v1">The first vector in interpolation</param>
        /// <param name="v2">The second vector in interpolation</param>
        /// <param name="v3">The third vector in interpolation</param>
        /// <param name="v4">The fourth vector in interpolation</param>
        /// <param name="a">Weighting factor</param>
        /// <returns>The result of CatmullRom interpolation</returns>
        public static Vector2 CatmullRom(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, float a)
        {
            return new Vector2(Mathf.CatmullRom(v1.X, v2.X, v3.X, v4.X, a), Mathf.CatmullRom(v1.Y, v2.Y, v3.Y, v4.Y, a));
        }

        /// <summary>
        /// Round the members of this <see cref="Vector2"/> towards positive infinity
        /// </summary>
        public void Ceiling()
        {
            X = Mathf.Ceiling(X);
            Y = Mathf.Ceiling(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded towards positive infinity
        /// </summary>
        /// <param name="v">Source <see cref="Vector2"/></param>
        /// <returns>The rounded <see cref="Vector2"/></returns>
        public static Vector2 Ceiling(Vector2 v)
        {
            v.X = Mathf.Ceiling(v.X);
            v.Y = Mathf.Ceiling(v.Y);
            return v;
        }

        /// <summary>
        /// Clamps the specified value within a range
        /// </summary>
        /// <param name="v1">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns>The clamped value</returns>
        public static Vector2 Clamp(Vector2 v1, Vector2 min, Vector2 max)
        {
            return new Vector2(Mathf.Clamp(v1.X, min.X, max.X), Mathf.Clamp(v1.Y, min.Y, max.Y));
        }

        /// <summary>
        /// Returns the distance between two vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>The distance between two vectors</returns>
        public static float Distance(Vector2 v1, Vector2 v2)
        {
            float v1X = v1.X - v2.X;
            float v2Y = v1.Y - v2.Y;
            return Mathf.Sqrt(v1X * v1X + v2Y * v2Y);
        }

        /// <summary>
        /// Returns the squared distance between two vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>The squared distance between two vectors</returns>
        public static float DistanceSquared(Vector2 v1, Vector2 v2)
        {
            float v1X = v1.X - v2.X;
            float v2Y = v1.Y - v2.Y;
            return v1X * v1X + v2Y * v2Y;
        }

        /// <summary>
        /// Returns a dot product of two vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>The dot product of two vectors</returns>
        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /// <summary>
        /// Round the members of this <see cref="Vector2"/> towards negative infinity
        /// </summary>
        public void Floor()
        {
            X = Mathf.Floor(X);
            Y = Mathf.Floor(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded towards negative infinity
        /// </summary>
        /// <param name="v">Source <see cref="Vector2"/></param>
        /// <returns>The rounded <see cref="Vector2"/></returns>
        public static Vector2 Floor(Vector2 v)
        {
            v.X = Mathf.Floor(v.X);
            v.Y = Mathf.Floor(v.Y);
            return v;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains hermite spline interpolation
        /// </summary>
        /// <param name="v1">The first position vector</param>
        /// <param name="t1">The first tangent vector</param>
        /// <param name="v2">The second position vector</param>
        /// <param name="t2">The second tangent vector</param>
        /// <param name="a">Weighting factor</param>
        /// <returns>The hermite spline interpolation vector</returns>
        public static Vector2 Hermite(Vector2 v1, Vector2 t1, Vector2 v2, Vector2 t2, float a)
        {
            return new Vector2(Mathf.Hermite(v1.X, t1.X, v2.X, t2.X, a), Mathf.Hermite(v1.Y, t1.Y, v2.Y, t2.Y, a));
        }

        /// <summary>
        /// Returns the length of this <see cref="Vector2"/>
        /// </summary>
        /// <returns>The length of this <see cref="Vector2"/></returns>
        public float Length()
        {
            return Mathf.Sqrt(X * X + Y * Y);
        }
        
        /// <summary>
        /// Returns the length of this <see cref="Vector2"/>
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>The length of this <see cref="Vector2"/></returns>
        public static float Length(Vector2 v)
        {
            return Mathf.Sqrt(Mathf.Pow(v.X, 2) + Mathf.Pow(v.Y, 2));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vector2"/>
        /// </summary>
        /// <returns>The squared length of this <see cref="Vector2"/></returns>
        public float LengthSquared()
        {
            return Mathf.Pow(X, 2) + Mathf.Pow(Y, 2);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains linear interpolation of the specified vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <param name="a">Weighting value (between 0.0 and 1.0)</param>
        /// <returns>The result of linear interpolation of the specified vectors</returns>
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float a)
        {
            return new Vector2(Mathf.Lerp(v1.X, v2.X, a), Mathf.Lerp(v1.Y, v2.Y, a));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains linear interpolation of the specified vectors
        /// Uses <see cref="Mathf.LerpPrecise"/> on Mathf for the interpolation
        /// Less efficient but more precise compared to <see cref="Vector2.Lerp(Vector2, Vector2, float)"/>
        /// See remarks section of <see cref="Mathf.LerpPrecise"/> on Mathf for more info
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <param name="a">Weighting value (between 0.0 and 1.0)</param>
        /// <returns>The result of linear interpolation of the specified vectors</returns>
        public static Vector2 LerpPrecise(Vector2 v1, Vector2 v2, float a)
        {
            return new Vector2(Mathf.LerpPrecise(v1.X, v2.X, a), Mathf.LerpPrecise(v1.Y, v2.Y, a));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a maximal values from the two vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>The <see cref="Vector2"/> with maximal values from the two vectors</returns>
        public static Vector2 Max(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X > v2.X ? v1.X : v2.X, v1.Y > v2.Y ? v1.Y : v2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a minimal values from the two vectors
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>The <see cref="Vector2"/> with minimal values from the two vectors</returns>
        public static Vector2 Min(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X < v2.X ? v1.X : v2.X, v1.Y < v2.Y ? v1.Y : v2.Y);
        }

        /// <summary>
        /// Turns this <see cref="Vector2"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            float val = 1.0f / Length();
            X *= val;
            Y *= val;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a normalized values from another vector
        /// </summary>
        /// <param name="v">Source <see cref="Vector2"/></param>
        /// <returns>Unit vector</returns>
        public static Vector2 Normalize(Vector2 v)
        {
            float val = 1.0f / Length(v);
            v.X *= val;
            v.Y *= val;
            return v;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains reflect vector of the given vector and normal
        /// </summary>
        /// <param name="v">Source <see cref="Vector2"/></param>
        /// <param name="n">Reflection normal</param>
        /// <returns>Reflected vector</returns>
        public static Vector2 Reflect(Vector2 v, Vector2 n)
        {
            Vector2 result;
            float val = 2.0f * (v.X * n.X + v.Y * n.Y);
            result.X = v.X - n.X * val;
            result.Y = v.Y - n.Y * val;
            return result;
        }

        /// <summary>
        /// Round the members of this <see cref="Vector2"/> to the nearest integer value
        /// </summary>
        public void Round()
        {
            X = Mathf.Round(X);
            Y = Mathf.Round(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded to the nearest integer value
        /// </summary>
        /// <param name="v">Source <see cref="Vector2"/></param>
        /// <returns>The rounded <see cref="Vector2"/></returns>
        public static Vector2 Round(Vector2 v)
        {
            v.X = Mathf.Round(v.X);
            v.Y = Mathf.Round(v.Y);
            return v;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains cubic interpolation of the specified vectors
        /// </summary>
        /// <param name="v1">Source <see cref="Vector2"/></param>
        /// <param name="v2">Source <see cref="Vector2"/></param>
        /// <param name="a">Weighting value</param>
        /// <returns>Cubic interpolation of the specified vectors</returns>
        public static Vector2 SmoothStep(Vector2 v1, Vector2 v2, float a)
        {
            return new Vector2(Mathf.SmoothStep(v1.X, v2.X, a), Mathf.SmoothStep(v1.Y, v2.Y, a));
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
        /// Get a vector perpendicular to this vector
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
            Vector2 perpendicular = Perpendicular(v1);
            float dot = Dot(v2, perpendicular);

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
            Vector2 perpendicular = new Vector2();
            Perpendicular(ref v1, ref perpendicular);
            float dot = Dot(v2, perpendicular);

            return dot >= 0.0f;
        }

        /// <summary>
        /// See if 2 line segments intersect (line AB collides with line CD)
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
        /// See if 2 line segments intersect (line AB collides with line CD) (reference type version)
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
        /// See if 2 line segments intersect (line AB collides with line CD) - simplified version
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
        /// See if 2 line segments intersect (line AB collides with line CD) - simplified version (reference type version)
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

        #endregion
    }
}