using UnityEngine;

namespace Kinematics.CollisionModule
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public sealed partial class BoxCollider
    {
        private Vector3 frontTopLeft;
        private Vector3 frontTopRight;
        private Vector3 frontBottomLeft;
        private Vector3 frontBottomRight;
        private Vector3 backTopLeft;
        private Vector3 backTopRight;
        private Vector3 backBottomLeft;
        private Vector3 backBottomRight;

        private readonly Color color = Color.green;

        private Mesh mesh;

        private void Update()
        {
            if (drawShape)
            {
                CalculatePosition();
                DrawCollider();
            }
        }

        private void CalculatePosition()
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
            Bounds bounds = mesh.bounds;
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;

            float xMinor = center.x - extents.x;
            float xMajor = center.x + extents.x;
            float yMinor = center.y - extents.y;
            float yMajor = center.y + extents.y;
            float zMinor = center.z - extents.z;
            float zMajor = center.z + extents.z;

            frontTopLeft = new Vector3(xMinor, yMajor, zMinor);
            frontTopRight = new Vector3(xMajor, yMajor, zMinor);
            frontBottomLeft = new Vector3(xMinor, yMinor, zMinor);
            frontBottomRight = new Vector3(xMajor, yMinor, zMinor);
            backTopLeft = new Vector3(xMinor, yMajor, zMajor);
            backTopRight = new Vector3(xMajor, yMajor, zMajor);
            backBottomLeft = new Vector3(xMinor, yMinor, zMajor);
            backBottomRight = new Vector3(xMajor, yMinor, zMajor);

            frontTopLeft = transform.TransformPoint(frontTopLeft);
            frontTopRight = transform.TransformPoint(frontTopRight);
            frontBottomLeft = transform.TransformPoint(frontBottomLeft);
            frontBottomRight = transform.TransformPoint(frontBottomRight);

            backTopLeft = transform.TransformPoint(backTopLeft);
            backTopRight = transform.TransformPoint(backTopRight);
            backBottomLeft = transform.TransformPoint(backBottomLeft);
            backBottomRight = transform.TransformPoint(backBottomRight);
        }

        private void DrawCollider()
        {
            CalculatePosition();

            Debug.DrawLine(frontTopLeft, frontTopRight, color);
            Debug.DrawLine(frontTopRight, frontBottomRight, color);
            Debug.DrawLine(frontBottomRight, frontBottomLeft, color);
            Debug.DrawLine(frontBottomLeft, frontTopLeft, color);

            Debug.DrawLine(backTopLeft, backTopRight, color);
            Debug.DrawLine(backTopRight, backBottomRight, color);
            Debug.DrawLine(backBottomRight, backBottomLeft, color);
            Debug.DrawLine(backBottomLeft, backTopLeft, color);

            Debug.DrawLine(frontTopLeft, backTopLeft, color);
            Debug.DrawLine(frontTopRight, backTopRight, color);
            Debug.DrawLine(frontBottomRight, backBottomRight, color);
            Debug.DrawLine(frontBottomLeft, backBottomLeft, color);
        }
    }
}