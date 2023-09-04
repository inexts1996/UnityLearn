using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    private Vector3[] _vertices;
    public int xSize, ySize;

    private void Awake()
    {
        Generate();
    }

    private Mesh mesh;

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        _vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[_vertices.Length];
        Vector4[] tangents = new Vector4[_vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (var x = 0; x <= xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2(x * 1f / xSize, y * 1f / ySize);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = _vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[xSize * ySize * 6];

        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 1] = triangles[ti + 4] = xSize + 1 + vi;
                triangles[ti + 5] = xSize + vi + 2;
            }
        }
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}