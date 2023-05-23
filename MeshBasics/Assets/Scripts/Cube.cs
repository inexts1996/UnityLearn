using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] public int _xSize, _ySize, _zSize;

    private Vector3[] _vertices;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private Mesh _mesh;

    private IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Cube";
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        int cornerVertices = 8;
        int edgeVertices = (_xSize + _ySize + _zSize - 3) * 4;
        int faceVertices = ((_xSize - 1) * (_ySize - 1) + (_xSize - 1) * (_zSize - 1) + (_ySize - 1) * (_zSize - 1)) * 2;

        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;

        for (int y = 0; y <= _ySize; y++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                _vertices[v++] = new Vector3(x, y, 0);
                yield return wait;
            }

            for (int z = 1; z <= _zSize; z++)
            {
                _vertices[v++] = new Vector3(_xSize, y, z);
                yield return wait;
            }

            for (int x = _xSize - 1; x > -1; x--)
            {
                _vertices[v++] = new Vector3(x, y, _zSize);
                yield return wait;
            }

            for (int z = _zSize - 1; z > 0; z--)
            {
                _vertices[v++] = new Vector3(0, y, z);
                yield return wait;
            }
        }

        for (int z = 1; z < _zSize; z++)
        {
            for (int x = 1; x < _xSize; x++)
            {
                _vertices[v++] = new Vector3(x, 0, z);
                yield return wait;
            }
        }

        for (int z = 1; z < _zSize; z++)
        {
            for (int x = 1; x < _xSize; x++)
            {
                _vertices[v++] = new Vector3(x, _ySize, z);
                yield return wait;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
}