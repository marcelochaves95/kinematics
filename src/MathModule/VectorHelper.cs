namespace Kinematics.MathModule
{
    public static class VectorHelper
    {
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
    }
}