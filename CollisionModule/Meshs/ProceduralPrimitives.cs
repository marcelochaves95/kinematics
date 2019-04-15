using System;
using System.Collections.Generic;

using UnityEngine;

//http://wiki.unity3d.com/index.php/ProceduralPrimitives

namespace Mesh
{
    /// <summary>
    /// Class to generate procedural Unity Meshes 
    /// </summary>
    public static class ProceduralPrimitives
    {
        public static Mesh CreateMeshPlane(float length = 1f, float width = 1f, int resX = 10, int resZ = 10)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Plane";

            /* Start here vertices */
            Vector3[] vertices = new Vector3[resX * resZ];
            for (int z = 0; z < resZ; z++)
            {
                // [ -length / 2, length / 2 ]
                float zPos = ((float)z / (resZ - 1) - .5f) * length;
                for (int x = 0; x < resX; x++)
                {
                    // [ -width / 2, width / 2 ]
                    float xPos = ((float)x / (resX - 1) - .5f) * width;
                    vertices[x + z * resX] = new Vector3(xPos, 0f, zPos);
                }
            }
            /* End here UVs */

            /* Start here normales */
            Vector3[] normales = new Vector3[vertices.Length];
            for (int n = 0; n < normales.Length; n++)
                normales[n] = Vector3.up;
            /* End here normales */

            /* Start here UVs */
            Vector2[] uvs = new Vector2[vertices.Length];
            for (int v = 0; v < resZ; v++)
            {
                for (int u = 0; u < resX; u++)
                {
                    uvs[u + v * resX] = new Vector2((float)u / (resX - 1), (float)v / (resZ - 1));
                }
            }
            /* End here UVs */

            /* Start here triangles */
            int nbFaces = (resX - 1) * (resZ - 1);
            int[] triangles = new int[nbFaces * 6];
            int t = 0;
            for (int face = 0; face < nbFaces; face++)
            {
                // Retrieve lower left corner from face ind
                int i = face % (resX - 1) + (face / (resZ - 1) * resX);

                triangles[t++] = i + resX;
                triangles[t++] = i + 1;
                triangles[t++] = i;

                triangles[t++] = i + resX;
                triangles[t++] = i + resX + 1;
                triangles[t++] = i + 1;
            }
            /* End here triangles */

            mesh.vertices = vertices;
            mesh.normals = normales;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            return mesh;

        }

        public static Mesh CreateMeshCube(float sides)
        {
            Mesh mesh = CreateMeshBox(sides, sides, sides);
            mesh.name = "Cube";
            return mesh;

        }

        public static Mesh CreateMeshBox(float length = 1f, float width = 1f, float height = 1f)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Box";

            /* Start here vetices */
            Vector3 p0 = new Vector3(-length * .5f, -width * .5f, height * .5f);
            Vector3 p1 = new Vector3(length * .5f, -width * .5f, height * .5f);
            Vector3 p2 = new Vector3(length * .5f, -width * .5f, -height * .5f);
            Vector3 p3 = new Vector3(-length * .5f, -width * .5f, -height * .5f);

            Vector3 p4 = new Vector3(-length * .5f, width * .5f, height * .5f);
            Vector3 p5 = new Vector3(length * .5f, width * .5f, height * .5f);
            Vector3 p6 = new Vector3(length * .5f, width * .5f, -height * .5f);
            Vector3 p7 = new Vector3(-length * .5f, width * .5f, -height * .5f);

            Vector3[] vertices = new Vector3[]
            {
                // Bottom
                p0, p1, p2, p3,

                // Left
                p7, p4, p0, p3,

                // Front
                p4, p5, p1, p0,

                // Back
                p6, p7, p3, p2,

                // Right
                p5, p6, p2, p1,

                // Top
                p7, p6, p5, p4
            };
            /* End here vetices */

            /* Start here normales */
            Vector3 up = Vector3.up;
            Vector3 down = Vector3.down;
            Vector3 front = Vector3.forward;
            Vector3 back = Vector3.back;
            Vector3 left = Vector3.left;
            Vector3 right = Vector3.right;

            Vector3[] normales = new Vector3[]
            {
                // Bottom
                down, down, down, down,

                // Left
                left, left, left, left,

                // Front
                front, front, front, front,

                // Back
                back, back, back, back,

                // Right
                right, right, right, right,

                // Top
                up, up, up, up
            };
            /* End here normales */

            /* Start here UVs */
            Vector2 _00 = new Vector2(0f, 0f);
            Vector2 _10 = new Vector2(1f, 0f);
            Vector2 _01 = new Vector2(0f, 1f);
            Vector2 _11 = new Vector2(1f, 1f);

            Vector2[] uvs = new Vector2[]
            {
                // Bottom
                _11, _01, _00, _10,

                // Left
                _11, _01, _00, _10,

                // Front
                _11, _01, _00, _10,

                // Back
                _11, _01, _00, _10,

                // Right
                _11, _01, _00, _10,

                // Top
                _11, _01, _00, _10,
            };
            /* End here UVs */

            /* Start here triangles */
            int[] triangles = new int[]
            {
                // Bottom
                3, 1, 0, 3, 2, 1,

                // Left
                3 + 4 * 1,
                1 + 4 * 1,
                0 + 4 * 1,
                3 + 4 * 1,
                2 + 4 * 1,
                1 + 4 * 1,

                // Front
                3 + 4 * 2,
                1 + 4 * 2,
                0 + 4 * 2,
                3 + 4 * 2,
                2 + 4 * 2,
                1 + 4 * 2,

                // Back
                3 + 4 * 3,
                1 + 4 * 3,
                0 + 4 * 3,
                3 + 4 * 3,
                2 + 4 * 3,
                1 + 4 * 3,

                // Right
                3 + 4 * 4,
                1 + 4 * 4,
                0 + 4 * 4,
                3 + 4 * 4,
                2 + 4 * 4,
                1 + 4 * 4,

                // Top
                3 + 4 * 5,
                1 + 4 * 5,
                0 + 4 * 5,
                3 + 4 * 5,
                2 + 4 * 5,
                1 + 4 * 5,

            };
            /* End here UVs */

            mesh.vertices = vertices;
            mesh.normals = normales;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            return mesh;
        }

        public static Mesh CreateMeshCapsule(float height = 1f, float radius = 1f, int nbSides = 18, int axis = 1)
        {
            Mesh mesh = new Mesh();
            CreateMeshCapsule(mesh, height, radius, radius, nbSides, axis);
            mesh.name = "Capsule";
            return mesh;
        }

        public static Mesh CreateMeshCylinder(float height = 1f, float radius = 1f, int nbSides = 18)
        {
            Mesh mesh = CreateMeshCone(height, radius, radius, nbSides);
            mesh.name = "Cylinder";
            return mesh;

        }

        //Note that cylinders(bottomRadius == topRadius) and pyramids(4 sides, topRadius == 0) are types of cones, and can be created with this script
        public static Mesh CreateMeshCone(float height = 1f, float bottomRadius = 1f, float topRadius = 0f, int nbSides = 18)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Cone";
            mesh.Clear();
            CreateMeshCone(mesh, height, bottomRadius, topRadius, nbSides);
            return mesh;
        }

        public static void CreateMeshCapsule(Mesh mesh, float height = 1, float bottomRadius = 1f, float topRadius = 0f, int nbSides = 18, int axis = 1)
        {
            int nbHeightSeg = 1; // Not implemented yet
            int nbVerticesCap = nbSides + 1;

            /* Start here vertices */

            // bottom + top + sides
            Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * nbHeightSeg * 2 + 2];
            int vert = 0;
            float _2pi = Mathematics.PI * 2f;

            // Bottom cap
            vertices[vert++] = new Vector3(0f, -bottomRadius, 0f);
            while (vert <= nbSides)
            {
                float rad = (float)vert / nbSides * _2pi;
                vertices[vert] = new Vector3(Mathematics.Cos(rad) * bottomRadius, 0f, Mathematics.Sin(rad) * bottomRadius);
                vert++;
            }

            // Top cap
            vertices[vert++] = new Vector3(0f, height + topRadius, 0f);
            while (vert <= nbSides * 2 + 1)
            {
                float rad = (float)(vert - nbSides - 1) / nbSides * _2pi;
                vertices[vert] = new Vector3(Mathematics.Cos(rad) * topRadius, height, Mathematics.Sin(rad) * topRadius);
                vert++;
            }

            // Sides
            int v = 0;
            while (vert <= vertices.Length - 4)
            {
                float rad = (float)v / nbSides * _2pi;
                vertices[vert] = new Vector3(Mathematics.Cos(rad) * topRadius, height, Mathematics.Sin(rad) * topRadius);
                vertices[vert + 1] = new Vector3(Mathematics.Cos(rad) * bottomRadius, 0, Mathematics.Sin(rad) * bottomRadius);
                vert += 2;
                v++;
            }
            vertices[vert] = vertices[nbSides * 2 + 2];
            vertices[vert + 1] = vertices[nbSides * 2 + 3];
            /* End here vertices */

            //Apply offset to vertices to shift pivot to center (Bullet default)

            //TODO: User settable offset
            Vector3 offset = new Vector3(0f, -1f, 0f) * (height / 2);

            for (int ii = 0; ii < vertices.Length; ii++)
            {
                vertices[ii] += offset;
                //offset
            }

            /* Start here normales */

            // bottom + top + sides
            Vector3[] normales = new Vector3[vertices.Length];
            vert = 0;

            // Bottom cap
            while (vert <= nbSides)
            {
                normales[vert++] = Vector3.down;
            }

            // Top cap
            while (vert <= nbSides * 2 + 1)
            {
                normales[vert++] = Vector3.up;
            }

            // Sides
            v = 0;
            while (vert <= vertices.Length - 4)
            {
                float rad = (float)v / nbSides * _2pi;
                float cos = Mathematics.Cos(rad);
                float sin = Mathematics.Sin(rad);

                normales[vert] = new Vector3(cos, 0f, sin);
                normales[vert + 1] = normales[vert];

                vert += 2;
                v++;
            }
            normales[vert] = normales[nbSides * 2 + 2];
            normales[vert + 1] = normales[nbSides * 2 + 3];
            /* End here normales */

            Quaternion q = Quaternion.identity;
            if (axis == 0)
            {
                q = Quaternion.AngleAxis(90, Vector3.forward);
            }
            else if (axis == 2)
            {
                q = Quaternion.AngleAxis(90, Vector3.right);
            }

            if (axis == 0 || axis == 2)
            {
                for (int ii = 0; ii < vertices.Length; ii++)
                {
                    vertices[ii] = q * vertices[ii];
                }

                for (int ii = 0; ii < normales.Length; ii++)
                {
                    normales[ii] = q * normales[ii];
                }
            }

            /* Start here UVs */
            Vector2[] uvs = new Vector2[vertices.Length];

            // Bottom cap
            int u = 0;
            uvs[u++] = new Vector2(0.5f, 0.5f);
            while (u <= nbSides)
            {
                float rad = (float)u / nbSides * _2pi;
                uvs[u] = new Vector2(Mathematics.Cos(rad) * .5f + .5f, Mathematics.Sin(rad) * .5f + .5f);
                u++;
            }

            // Top cap
            uvs[u++] = new Vector2(0.5f, 0.5f);
            while (u <= nbSides * 2 + 1)
            {
                float rad = (float)u / nbSides * _2pi;
                uvs[u] = new Vector2(Mathematics.Cos(rad) * .5f + .5f, Mathematics.Sin(rad) * .5f + .5f);
                u++;
            }

            // Sides
            int u_sides = 0;
            while (u <= uvs.Length - 4)
            {
                float t = (float)u_sides / nbSides;
                uvs[u] = new Vector3(t, 1f);
                uvs[u + 1] = new Vector3(t, 0f);
                u += 2;
                u_sides++;
            }
            uvs[u] = new Vector2(1f, 1f);
            uvs[u + 1] = new Vector2(1f, 0f);
            /* End here UVs */

            /* Start here triangles */
            int nbTriangles = nbSides + nbSides + nbSides * 2;
            int[] triangles = new int[nbTriangles * 3 + 3];

            // Bottom cap
            int tri = 0;
            int i = 0;
            while (tri < nbSides - 1)
            {
                triangles[i] = 0;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = tri + 2;
                tri++;
                i += 3;
            }
            triangles[i] = 0;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = 1;
            tri++;
            i += 3;

            // Top cap
            //tri++;
            while (tri < nbSides * 2)
            {
                triangles[i] = tri + 2;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = nbVerticesCap;
                tri++;
                i += 3;
            }

            triangles[i] = nbVerticesCap + 1;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = nbVerticesCap;
            tri++;
            i += 3;
            tri++;

            // Sides
            while (tri <= nbTriangles)
            {
                triangles[i] = tri + 2;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = tri + 0;
                tri++;
                i += 3;

                triangles[i] = tri + 1;
                triangles[i + 1] = tri + 2;
                triangles[i + 2] = tri + 0;
                tri++;
                i += 3;
            }
            /* End here triangles */

            mesh.vertices = vertices;
            mesh.normals = normales;
            mesh.uv = uvs;
            mesh.triangles = triangles;
        }

        public static void CreateMeshCone(Mesh mesh, float height = 1, float bottomRadius = 1f, float topRadius = 0f, int nbSides = 18)
        {
            int nbHeightSeg = 1; // Not implemented yet
            int nbVerticesCap = nbSides + 1;

            /* Start here vertices */

            // bottom + top + sides
            Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * nbHeightSeg * 2 + 2];
            int vert = 0;
            float _2pi = Mathematics.PI * 2f;

            // Bottom cap
            vertices[vert++] = new Vector3(0f, 0f, 0f);
            while (vert <= nbSides)
            {
                float rad = (float)vert / nbSides * _2pi;
                vertices[vert] = new Vector3(Mathematics.Cos(rad) * bottomRadius, 0f, Mathematics.Sin(rad) * bottomRadius);
                vert++;
            }

            // Top cap
            vertices[vert++] = new Vector3(0f, height, 0f);
            while (vert <= nbSides * 2 + 1)
            {
                float rad = (float)(vert - nbSides - 1) / nbSides * _2pi;
                vertices[vert] = new Vector3(Mathematics.Cos(rad) * topRadius, height, Mathematics.Sin(rad) * topRadius);
                vert++;
            }

            // Sides
            int v = 0;
            while (vert <= vertices.Length - 4)
            {
                float rad = (float)v / nbSides * _2pi;
                vertices[vert] = new Vector3(Mathematics.Cos(rad) * topRadius, height, Mathematics.Sin(rad) * topRadius);
                vertices[vert + 1] = new Vector3(Mathematics.Cos(rad) * bottomRadius, 0, Mathematics.Sin(rad) * bottomRadius);
                vert += 2;
                v++;
            }
            vertices[vert] = vertices[nbSides * 2 + 2];
            vertices[vert + 1] = vertices[nbSides * 2 + 3];
            /* End here vertices */

            //Apply offset to vertices to shift pivot to center (Bullet default)

            //TODO: User settable offset
            Vector3 offset = new Vector3(0f, -1f, 0f) * (height / 2);

            for (int ii = 0; ii < vertices.Length; ii++)
            {
                vertices[ii] += offset;
                //offset
            }

            /* Start here normales */

            // bottom + top + sides
            Vector3[] normales = new Vector3[vertices.Length];
            vert = 0;

            // Bottom cap
            while (vert <= nbSides)
            {
                normales[vert++] = Vector3.down;
            }

            // Top cap
            while (vert <= nbSides * 2 + 1)
            {
                normales[vert++] = Vector3.up;
            }

            // Sides
            v = 0;
            while (vert <= vertices.Length - 4)
            {
                float rad = (float)v / nbSides * _2pi;
                float cos = Mathematics.Cos(rad);
                float sin = Mathematics.Sin(rad);

                normales[vert] = new Vector3(cos, 0f, sin);
                normales[vert + 1] = normales[vert];

                vert += 2;
                v++;
            }
            normales[vert] = normales[nbSides * 2 + 2];
            normales[vert + 1] = normales[nbSides * 2 + 3];
            /* End here normales */

            /* Start here UVs */

            Vector2[] uvs = new Vector2[vertices.Length];

            // Bottom cap
            int u = 0;
            uvs[u++] = new Vector2(0.5f, 0.5f);
            while (u <= nbSides)
            {
                float rad = (float)u / nbSides * _2pi;
                uvs[u] = new Vector2(Mathematics.Cos(rad) * .5f + .5f, Mathematics.Sin(rad) * .5f + .5f);
                u++;
            }

            // Top cap
            uvs[u++] = new Vector2(0.5f, 0.5f);
            while (u <= nbSides * 2 + 1)
            {
                float rad = (float)u / nbSides * _2pi;
                uvs[u] = new Vector2(Mathematics.Cos(rad) * .5f + .5f, Mathematics.Sin(rad) * .5f + .5f);
                u++;
            }

            // Sides
            int u_sides = 0;
            while (u <= uvs.Length - 4)
            {
                float t = (float)u_sides / nbSides;
                uvs[u] = new Vector3(t, 1f);
                uvs[u + 1] = new Vector3(t, 0f);
                u += 2;
                u_sides++;
            }
            uvs[u] = new Vector2(1f, 1f);
            uvs[u + 1] = new Vector2(1f, 0f);
            /* End here UVs */

            /* Start here triangles */

            int nbTriangles = nbSides + nbSides + nbSides * 2;
            int[] triangles = new int[nbTriangles * 3 + 3];

            // Bottom cap
            int tri = 0;
            int i = 0;
            while (tri < nbSides - 1)
            {
                triangles[i] = 0;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = tri + 2;
                tri++;
                i += 3;
            }
            triangles[i] = 0;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = 1;
            tri++;
            i += 3;

            // Top cap
            //tri++;
            while (tri < nbSides * 2)
            {
                triangles[i] = tri + 2;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = nbVerticesCap;
                tri++;
                i += 3;
            }

            triangles[i] = nbVerticesCap + 1;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = nbVerticesCap;
            tri++;
            i += 3;
            tri++;

            // Sides
            while (tri <= nbTriangles)
            {
                triangles[i] = tri + 2;
                triangles[i + 1] = tri + 1;
                triangles[i + 2] = tri + 0;
                tri++;
                i += 3;

                triangles[i] = tri + 1;
                triangles[i + 1] = tri + 2;
                triangles[i + 2] = tri + 0;
                tri++;
                i += 3;
            }
            /* End here triangles */

            mesh.vertices = vertices;
            mesh.normals = normales;
            mesh.uv = uvs;
            mesh.triangles = triangles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="nbLong">number of longitude lines</param>
        /// <param name="nbLat">number of latitude lines</param>
        /// <returns></returns>
        public static Mesh CreateMeshSphere(float radius = 1f, int nbLong = 24, int nbLat = 16)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Sphere";
            mesh.Clear();

            /* Start here vertices */
            Vector3[] vertices = new Vector3[(nbLong + 1) * nbLat + 2];
            float _pi = Mathematics.PI;
            float _2pi = _pi * 2f;

            vertices[0] = Vector3.up * radius;
            for (int lat = 0; lat < nbLat; lat++)
            {
                float a1 = _pi * (float)(lat + 1) / (nbLat + 1);
                float sin1 = Mathematics.Sin(a1);
                float cos1 = Mathematics.Cos(a1);

                for (int lon = 0; lon <= nbLong; lon++)
                {
                    float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                    float sin2 = Mathematics.Sin(a2);
                    float cos2 = Mathematics.Cos(a2);

                    vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }
            vertices[vertices.Length - 1] = Vector3.up * -radius;
            /* End here vertices */

            /* Start here normales */
            Vector3[] normales = new Vector3[vertices.Length];
            for (int n = 0; n < vertices.Length; n++)
                normales[n] = vertices[n].normalized;
            /* End here normales */

            /* Start here UVs */
            Vector2[] uvs = new Vector2[vertices.Length];
            uvs[0] = Vector2.up;
            uvs[uvs.Length - 1] = Vector2.zero;
            for (int lat = 0; lat < nbLat; lat++)
                for (int lon = 0; lon <= nbLong; lon++)
                    uvs[lon + lat * (nbLong + 1) + 1] = new Vector2((float)lon / nbLong, 1f - (float)(lat + 1) / (nbLat + 1));
            /* End here UVs */

            /* Start here triangles */
            int nbFaces = vertices.Length;
            int nbTriangles = nbFaces * 2;
            int nbIndexes = nbTriangles * 3;
            int[] triangles = new int[nbIndexes];

            //Top Cap
            int i = 0;
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = lon + 2;
                triangles[i++] = lon + 1;
                triangles[i++] = 0;
            }

            //Middle
            for (int lat = 0; lat < nbLat - 1; lat++)
            {
                for (int lon = 0; lon < nbLong; lon++)
                {
                    int current = lon + lat * (nbLong + 1) + 1;
                    int next = current + nbLong + 1;

                    triangles[i++] = current;
                    triangles[i++] = current + 1;
                    triangles[i++] = next + 1;

                    triangles[i++] = current;
                    triangles[i++] = next + 1;
                    triangles[i++] = next;
                }
            }

            //Bottom Cap
            for (int lon = 0; lon < nbLong; lon++)
            {
                triangles[i++] = vertices.Length - 1;
                triangles[i++] = vertices.Length - (lon + 2) - 1;
                triangles[i++] = vertices.Length - (lon + 1) - 1;
            }
            /* End here triangles */

            mesh.vertices = vertices;
            mesh.normals = normales;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            return mesh;
        }

        public static Mesh BuildMeshFromData(float[] vertices, int[] triangles)
        {
            Mesh mesh = new Mesh();
            mesh.name = "MeshFromData";

            int numVertices = vertices.Length / 3;
            Vector3[] newVerts = new Vector3[numVertices];

            for (int i = 0, j = 0; j < numVertices; j++, i += 3)
            {
                newVerts[j] = new Vector3(vertices[i], vertices[i + 1], vertices[i + 2]);
            }

            mesh.vertices = newVerts;
            mesh.triangles = triangles;

            return mesh;
        }

        //http://answers.unity3d.com/questions/228841/dynamically-combine-verticies-that-share-the-same.html

        /// <summary>
        /// Weld close vertices together to create a closed hull
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static void AutoWeldVertices(this Mesh mesh, float threshold = 0.001f)
        {
            Vector3[] verts = mesh.vertices;

            // Build new vertex buffer and remove "duplicate" verticies
            // that are within the given threshold.
            List<Vector3> newVerts = new List<Vector3>();
            List<Vector2> newUVs = new List<Vector2>();

            int k = 0;

            foreach (Vector3 vert in verts)
            {
                // Has vertex already been added to newVerts list?
                foreach (Vector3 newVert in newVerts)
                    if (Vector3.Distance(newVert, vert) <= threshold)
                        goto skipToNext;

                // Accept new vertex!
                newVerts.Add(vert);
                if (mesh.uv.Length > 0) //some meshes dont have 
                    newUVs.Add(mesh.uv[k]);

                skipToNext: ;
                ++k;
            }

            // Rebuild triangles using new verticies
            int[] tris = mesh.triangles;
            for (int i = 0; i < tris.Length; ++i)
            {
                // Find new vertex point from buffer
                for (int j = 0; j < newVerts.Count; ++j)
                {
                    if (Vector3.Distance(newVerts[j], verts[tris[i]]) <= threshold)
                    {
                        tris[i] = j;
                        break;
                    }
                }
            }

            mesh.triangles = tris;
            mesh.vertices = newVerts.ToArray();
            mesh.uv = newUVs.ToArray();
        }

        /// <summary>
        /// Add back face triangles to this mesh. Test me
        /// </summary>
        /// <param name="mesh"></param>
        public static void AddBackFaceTriangles(this Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            Vector2[] uv = mesh.uv;
            Vector3[] normals = mesh.normals;
            int szV = vertices.Length;
            Vector3[] newVerts = new Vector3[szV * 2];
            Vector2[] newUv = new Vector2[szV * 2];
            Vector3[] newNorms = new Vector3[szV * 2];

            int j, i;

            for (j = 0; j < szV; j++)
            {
                // duplicate vertices and uvs:
                newVerts[j] = newVerts[j + szV] = vertices[j];
                newUv[j] = newUv[j + szV] = uv[j];

                // copy the original normals...
                newNorms[j] = normals[j];
                
                // and revert the new ones
                newNorms[j + szV] = -normals[j];
            }
            int[] triangles = mesh.triangles;
            int szT = triangles.Length;
            int[] newTris = new int[szT * 2]; // double the triangles
            for (i = 0; i < szT; i += 3)
            {
                // copy the original triangle
                newTris[i] = triangles[i];
                newTris[i + 1] = triangles[i + 1];
                newTris[i + 2] = triangles[i + 2];

                // save the new reversed triangle
                j = i + szT;
                newTris[j] = triangles[i] + szV;
                newTris[j + 2] = triangles[i + 1] + szV;
                newTris[j + 1] = triangles[i + 2] + szV;
            }
            mesh.Clear();
            mesh.vertices = newVerts;
            mesh.uv = newUv;
            mesh.normals = newNorms;
            mesh.triangles = newTris; // assign triangles last!
        }

        /// <summary>
        /// After mesh is created, process it based on selected options
        /// </summary>
        public static void ApplyMeshPostProcessing(this Mesh mesh,
            bool autoWeldVertices = false,
            float autoWeldThreshold = 0.001f,
            bool addBackFaceTriangles = false,
            bool recalculateNormals = false,
            bool recalculateBounds = true,
            bool optimize = true)
        {

            if (autoWeldVertices)
                mesh.AutoWeldVertices(autoWeldThreshold);

            if (addBackFaceTriangles)
                mesh.AddBackFaceTriangles();

            if (recalculateNormals)
                mesh.RecalculateNormals();

            if (recalculateBounds)
                mesh.RecalculateBounds();

#if UNITY_EDITOR
            if (optimize)
                UnityEditor.MeshUtility.Optimize(mesh);
#endif
        }
    }
}