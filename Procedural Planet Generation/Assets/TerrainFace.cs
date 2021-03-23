using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace 
{
    Mesh mesh;
    int resolution; //number of vertices along an edge
    Vector3 localUP;
    Vector3 axisA;
    Vector3 axisB;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUP)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUP = localUP;

        this.axisA = new Vector3(localUP.y, localUP.z, localUP.x);
        this.axisB = Vector3.Cross(localUP, axisA);
    }

    public void ConstructMesh() 
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        int triIndex = 0;

        for (int y = 0; y < resolution; y++) 
        {
            for (int x = 0; x < resolution; x++) 
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUP + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                if (x != resolution - 1 && y != resolution - 1) 
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
