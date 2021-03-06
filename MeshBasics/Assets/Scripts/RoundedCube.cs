using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoundedCube : MonoBehaviour
{
    [SerializeField] private int _xSize, _ySize, _zSize;
    [SerializeField] private int _roundness;

    private Vector3[] _vertices;
    private Vector3[] _normals;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GenerateVertices();
        GenerateTriangles();
    }


    private void GenerateTriangles()
    {
        int[] trianglesZ = new int[(_xSize * _ySize) * 12];
        int[] trianglesX = new int[(_ySize * _zSize) * 12];
        int[] trianglesY = new int[(_xSize * _zSize) * 12];

        int quads = (_xSize * _ySize + _xSize * _zSize + _ySize * _zSize) * 2;
        int tz = 0, tx = 0, ty = 0, v = 0;
        int ring = (_xSize + _zSize) * 2;
        for (int y = 0; y < _ySize; y++, v++)
        {
            for (int q = 0; q < _xSize; q++, v++)
            {
                tz = SetQuad(trianglesZ, tz, v, v + 1, v + ring, v + ring + 1);
            }

            for (int q = 0; q < _zSize; q++, v++)
            {
                tx = SetQuad(trianglesX, tx, v, v + 1, v + ring, v + ring + 1);
            }

            for (int q = 0; q < _xSize; q++, v++)
            {
                tz = SetQuad(trianglesZ, tz, v, v + 1, v + ring, v + ring + 1);
            }

            for (int q = 0; q < _zSize - 1; q++, v++)
            {
                tx = SetQuad(trianglesX, tx, v, v + 1, v + ring, v + ring + 1);
            }


            tx = SetQuad(trianglesX, tx, v, v - ring + 1, v + ring, v + 1);
        }

        ty = CreateTopFace(trianglesY, ty, ring);
        ty = CreateBottomFace(trianglesY, ty, ring);
        _mesh.subMeshCount = 3;
        _mesh.SetTriangles(trianglesZ, 0);
        _mesh.SetTriangles(trianglesX, 1);
        _mesh.SetTriangles(trianglesY, 2);
    }

    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = _vertices.Length - (_xSize - 1) * (_zSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < _xSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }

        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= _xSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < _zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + _xSize - 1, vMin + 1, vMid);
            for (int x = 1; x < _xSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid + _xSize - 1, vMid + _xSize, vMid, vMid + 1);
            }

            t = SetQuad(triangles, t, vMid + _xSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);

        for (int x = 1; x < _xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }

        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
        return t;
    }

    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        int v = ring * _ySize;
        for (int x = 0; x < _xSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }

        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);


        int vMin = ring * (_ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;
        for (int z = 1; z < _zSize - 1; vMin--, vMid++, vMax++, z++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + _xSize - 1);

            for (int x = 1; x < _xSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, vMid + _xSize - 1, vMid + _xSize);
            }

            t = SetQuad(triangles, t, vMid, vMax, vMid + _xSize - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < _xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }

        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

        return t;
    }

    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private Mesh _mesh;

    private void GenerateVertices()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Cube";
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        int cornerVertices = 8;
        int edgeVertices = (_xSize + _ySize + _zSize - 3) * 4;
        int faceVertices = ((_xSize - 1) * (_ySize - 1) + (_xSize - 1) * (_zSize - 1) + (_ySize - 1) * (_zSize - 1)) * 2;

        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        _normals = new Vector3[_vertices.Length];

        int v = 0;

        for (int y = 0; y <= _ySize; y++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                SetVertex(v++, x, y, 0);
            }

            for (int z = 1; z <= _zSize; z++)
            {
                SetVertex(v++, _xSize, y, z);
            }

            for (int x = _xSize - 1; x > -1; x--)
            {
                SetVertex(v++, x, y, _zSize);
            }

            for (int z = _zSize - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
            }
        }

        for (int z = 1; z < _zSize; z++)
        {
            for (int x = 1; x < _xSize; x++)
            {
                SetVertex(v++, x, _ySize, z);
            }
        }

        for (int z = 1; z < _zSize; z++)
        {
            for (int x = 1; x < _xSize; x++)
            {
                SetVertex(v++, x, 0, z);
            }
        }

        _mesh.vertices = _vertices;
        _mesh.normals = _normals;
    }

    private void SetVertex(int i, int x, int y, int z)
    {
        Vector3 inner = _vertices[i] = new Vector3(x, y, z);
        if (x < _roundness)
        {
            inner.x = _roundness;
        }
        else if (x > _xSize - _roundness)
        {
            inner.x = _xSize - _roundness;
        }

        if (y < _roundness)
        {
            inner.y = _roundness;
        }
        else if (y > _ySize - _roundness)
        {
            inner.y = _ySize - _roundness;
        }

        if (z < _roundness)
        {
            inner.z = _roundness;
        }
        else if (z > _zSize - _roundness)
        {
            inner.z = _zSize - _roundness;
        }

        _normals[i] = (_vertices[i] - inner).normalized;
        _vertices[i] = inner + _normals[i] * _roundness;
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }

        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(_vertices[i], _normals[i]);
        }
    }
}