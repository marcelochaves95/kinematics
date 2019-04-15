using System;
using System.ComponentModel;
using CollisionModule.Shapes;
using UnityEngine;

namespace CollisionModule.Meshes
{
    [Serializable]
    public abstract class PrimitiveMeshSettings
    {

        public abstract Mesh Build();
    }

    [Serializable]
    public class UserMeshSettings : PrimitiveMeshSettings
    {

        [SerializeField]
        private Mesh _userMesh;
        public Mesh UserMesh
        {
            get { return _userMesh; }
            set { _userMesh = value; }
        }

        [Header("Mesh post processing")]
        public bool autoWeldVertices = false;
        public float autoWeldThreshold = 0.001f; //TODO
        [Tooltip("Should use this if autoWeldVertices is selected.")]
        public bool recalculateNormals = false;
        public bool addBackFaceTriangles = false;
        public bool recalculateBounds = true;
        public bool optimize = true;

        public override Mesh Build()
        {
            if (UserMesh == null) // Fill in something
            {
                Debug.Log("Must provide a mesh or create one!");
                return null;
            }

            Mesh mesh = UnityEngine.Object.Instantiate(UserMesh); // Create a copy of UserMesh, dont overwrite prefabs

            mesh.ApplyMeshPostProcessing(autoWeldVertices, autoWeldThreshold, addBackFaceTriangles,
                recalculateNormals, recalculateBounds, optimize);

            return mesh;
        }

    }

    [Serializable]
    public class BoxMeshSettings : PrimitiveMeshSettings
    {
        [Header("Box Mesh settings:")]
        public Vector3 extents = Vector3.one;

        public override Mesh Build()
        {
            Mesh mesh = ProceduralPrimitives.CreateMeshBox(extents.x, extents.y, extents.z);
            return mesh;
        }
    }

    [Serializable]
    public class ConeMeshSettings : PrimitiveMeshSettings
    {
        [Header("Cone Mesh settings:")]
        [Range(0, 1000)]
        public float height = 1f;
        [Range(0, 1000)]
        public float radius = 0.5f;
        [Range(2, 100)]
        public int nbSides = 18;

        public override Mesh Build()
        {
            Mesh mesh = ProceduralPrimitives.CreateMeshCone(height, radius, 0f, nbSides);
            return mesh;
        }

    }

    [Serializable]
    public class CapsuleMeshSettings : PrimitiveMeshSettings
    {
        [Header("Capsule Mesh settings:")]
        [Range(0, 1000)]
        public float height = 1f;
        [Range(0, 1000)]
        public float radius = 0.5f;
        [Range(2, 100)]
        public int nbSides = 18;
        public CapsuleShape.CapsuleAxis upAxis = CapsuleShape.CapsuleAxis.y;

        public Vector3 HalfExtent
        {
            get { return new Vector3(radius, height / 2, radius); }
        }

        public override Mesh Build()
        {
            Mesh mesh = ProceduralPrimitives.CreateMeshCapsule(height, radius, nbSides, (int)upAxis);
            return mesh;
        }

    }

    [Serializable]
    public class CylinderMeshSettings : PrimitiveMeshSettings
    {
        [Header("Cylinder Mesh settings:")]
        [Range(0, 1000)]
        public float height = 1f;
        [Range(0, 1000)]
        public float radius = 0.5f;
        [Range(2, 100)]
        public int nbSides = 18;

        public Vector3 HalfExtent
        {
            get { return new Vector3(radius, height / 2, radius); }
        }

        public override Mesh Build()
        {
            Mesh mesh = ProceduralPrimitives.CreateMeshCylinder(height, radius, nbSides);
            return mesh;
        }

    }

    [Serializable]
    public class SphereMeshSettings : PrimitiveMeshSettings
    {
        [Header("Sphere Mesh settings:")]
        [Range(0, 1000)]
        public float radius = 0.5f;
        [Range(2, 100)]
        public int numLongitudeLines = 24;
        [Range(2, 100)]
        public int numLatitudeLines = 16;

        public override Mesh Build()
        {
            Mesh mesh = ProceduralPrimitives.CreateMeshSphere(radius, numLongitudeLines, numLatitudeLines);
            return mesh;
        }

    }

    /// <summary>
    /// Useful for creating something random for examples in the editor
    /// Instance remebers last settings
    /// </summary>
    [Serializable]
    public class AnyMeshSettings : PrimitiveMeshSettings
    {

        [Header("Bunny Mesh settings:")]
        public PrimitiveMeshOptions meshType = PrimitiveMeshOptions.Bunny;

        //Unity wont allow switching classes in editor, so this class has all parameters in one pile, ick!
        public Vector3 extents = Vector3.one; // Cube
        [Range(0, 1000)]
        public float radius = 0.5f; // Sphere, cone and cylinder
        [Range(0, 1000)]
        public float height = 1f; // Cone and cylinder
        [Range(0, 1000)]
        public float length = 5f;
        [Range(0, 1000)]
        public float width = 5f; // Plane
        [Range(2, 100)]
        public int numLongitudeLines = 10; // Sphere
        [Range(2, 100)]
        public int numLatitudeLines = 8; // Sphere
        [Range(2, 100)]
        public int nbSides = 18; // Cone and cylinder sides
        [Range(2, 1000)]
        public int resX = 5;
        [Range(2, 1000)]
        public int resZ = 5;

        public Mesh userMesh;
        //Unity cant display this due to serialization, figure it out later
        //public PrimitiveMeshSettings meshSettings = new PrimitiveMeshSettings();

        [Header("Mesh post processing")]
        public bool autoWeldVertices = false;
        public float autoWeldThreshold = 0.001f; //TODO
        [Tooltip("Should use this if autoWeldVertices is selected.")]
        public bool recalculateNormals = false;
        public bool addBackFaceTriangles = false;
        public bool recalculateBounds = true;
        public bool optimize = true;

        public override Mesh Build()
        {
            AnyMeshSettings settings = this;
            Mesh mesh = null;
            switch (settings.meshType)
            {
                case PrimitiveMeshOptions.UserDefinedMesh:
                    //Need to copy mesh from sharedMesh or we cant modify the mesh!
                    if (settings.userMesh == null) //fill in something
                    {
                        settings.userMesh = ProceduralPrimitives.CreateMeshBox(settings.extents.x, settings.extents.x, settings.extents.x);
                        Debug.Log("Must provide a mesh for UserDefinedMesh setting.");
                    }

                    mesh = UnityEngine.Object.Instantiate(settings.userMesh);
                    break;
                case PrimitiveMeshOptions.Box:
                    mesh = ProceduralPrimitives.CreateMeshBox(settings.extents.x, settings.extents.y, settings.extents.z);
                    break;
                case PrimitiveMeshOptions.Sphere:
                    mesh = ProceduralPrimitives.CreateMeshSphere(settings.radius, settings.numLongitudeLines, settings.numLatitudeLines);
                    break;
                case PrimitiveMeshOptions.Cylinder:
                    mesh = ProceduralPrimitives.CreateMeshCylinder(settings.height, settings.radius, settings.nbSides);
                    break;
                case PrimitiveMeshOptions.Cone:
                    mesh = ProceduralPrimitives.CreateMeshCone(settings.height, settings.radius, 0f, settings.nbSides);
                    break;
                case PrimitiveMeshOptions.Bunny:
                    mesh = ProceduralPrimitives.BuildMeshFromData(SoftDemo.BunnyMesh.Vertices, SoftDemo.BunnyMesh.Indices);
                    break;
                case PrimitiveMeshOptions.Plane:
                    mesh = ProceduralPrimitives.CreateMeshPlane(settings.length, settings.width, settings.resX, settings.resZ);
                    break;
                default:
                    break;
            }

            return mesh;
        }

    }

    /// <summary>
    /// For editor configurations
    /// </summary>
    [Serializable]
    public class AnyMeshSettingsForEditor : AnyMeshSettings
    {

        public bool imediateUpdate = true;

        protected static AnyMeshSettingsForEditor instance;

        public static AnyMeshSettingsForEditor Instance
        {
            get { return instance = instance ?? new AnyMeshSettingsForEditor(); }
        }

    }

    [Serializable]
    public class PlaneMeshSettings : PrimitiveMeshSettings
    {
        [Range(0, 1000)]
        public float length = 1f;
        [Range(0, 1000)]
        public float width = 1f;
        [Range(2, 1000)]
        public int resX = 5;
        [Range(2, 1000)]
        public int resZ = 5;

        public override Mesh Build()
        {
            Mesh mesh = ProceduralPrimitives.CreateMeshPlane(length, width, resX, resZ);
            return mesh;
        }
    }

    [Flags]
    public enum PrimitiveMeshOptions
    {
        [Description("User needs to provide a mesh in MeshFilter")]
        UserDefinedMesh,
        Box,
        Sphere,
        Cylinder,
        Cone,
        Bunny,
        Plane,
    }
}