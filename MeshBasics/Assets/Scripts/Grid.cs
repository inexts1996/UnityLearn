using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    private Vector3[] _vertices;
    public int xSize, ySize;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        var wait = new WaitForSeconds(0.5f);
        _vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (var x = 0; x <= xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null) return;
        Gizmos.color = Color.black;
        for (var i = 0; i < _vertices.Length; i++) Gizmos.DrawSphere(_vertices[i], 0.1f);
    }
}