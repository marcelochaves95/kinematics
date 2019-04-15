using UnityEngine;

using CollisionModule.Meshes;
using CollisionModule.Shapes;

namespace CollisionModule.Primitives
{
    /// <summary>
    /// Cylinder
    /// </summary>
    [RequireComponent(typeof(RigidBody))]
    [RequireComponent(typeof(CylinderShape))]
    public class Cylinder : Primitive
    {
        public CylinderMeshSettings meshSettings = new CylinderMeshSettings();

        public static GameObject CreateNew(Vector3 position, Quaternion rotation)
        {
            GameObject go = new GameObject();
            Cylinder cylinder = go.AddComponent<Cylinder>();
            CreateNewBase(go, position, rotation);
            cylinder.BuildMesh();
            go.name = "Cylinder";

            return go;
        }

        public override void BuildMesh()
        {
            GetComponent<MeshFilter>().sharedMesh = meshSettings.Build();
            GetComponent<CylinderShape>().HalfExtent = meshSettings.HalfExtent;
        }
    }
}