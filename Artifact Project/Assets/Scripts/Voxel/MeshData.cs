﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxel
{
    public class MeshData
    {
        public bool useRenderDataForCol;

        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uv = new List<Vector2>();

        public List<Vector3> colVertices = new List<Vector3>();
        public List<int> colTriangles = new List<int>();

        public MeshData()
        {

        }

        public void AddTrianglesFromQuad()
        {
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);

            if(useRenderDataForCol)
            {
                colTriangles.Add(vertices.Count - 4);
                colTriangles.Add(vertices.Count - 3);
                colTriangles.Add(vertices.Count - 2);

                colTriangles.Add(vertices.Count - 4);
                colTriangles.Add(vertices.Count - 2);
                colTriangles.Add(vertices.Count - 1);
            }
        }

        public void AddVertex(Vector3 vertex)
        {
            vertices.Add(vertex);

            if (useRenderDataForCol)
                colVertices.Add(vertex);
        }

        public void AddTriangle(int tri)
        {
            triangles.Add(tri);

            if (useRenderDataForCol)
                colTriangles.Add(tri - (vertices.Count - colVertices.Count));
        }
    }
}
