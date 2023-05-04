using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    [SerializeField]
    private int _xSize, _ySize;

    private void Awake()
    {
        Generate();
    }

    private Vector3[] _vertices;
    private void Generate()
    {
        int veticeCount = (_xSize + 1) * (_ySize + 1);
        _vertices = new Vector3[veticeCount];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var vertex in _vertices)
        {
            Gizmos.DrawSphere(vertex, 0.1f);
        }
    }
}