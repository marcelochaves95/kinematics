using CollisionModule.Meshes;
using CollisionModule.Shapes;
using UnityEngine;

namespace CollisionModule.Primitives
{
    /// <summary>
    /// Sphere
    /// </summary>
    [RequireComponent(typeof(RigidBody))]
    [RequireComponent(typeof(SphereShape))]
    public class Sphere : Primitive
    {
        public SphereMeshSettings meshSettings = new SphereMeshSettings();

        public static GameObject CreateNew(Vector3 position, Quaternion rotation)
        {
            GameObject go = new GameObject();
            go.AddComponent<SphereShape>();
            Sphere sphere = go.AddComponent<Sphere>();
            CreateNewBase(go, position, rotation);
            sphere.BuildMesh();
            go.name = "Sphere";

            return go;
        }

        public override void BuildMesh()
        {
            GetComponent<MeshFilter>().sharedMesh = meshSettings.Build();
            GetComponent<SphereShape>().Radius = meshSettings.radius;
        }
    }
}