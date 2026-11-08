using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoundWindowMesh : MonoBehaviour
{
    [Range(0.30f, 0.60f)] public float radius = 0.54f;

    private Mesh windowMesh;
    private float builtRadius;

    void OnEnable()
    {
        windowMesh = new Mesh
        {
            name = "Aquarium Porthole",
            hideFlags = HideFlags.DontSave
        };

        GetComponent<MeshFilter>().sharedMesh = windowMesh;
        BuildWindow();
    }

    void Update()
    {
        if (windowMesh != null && !Mathf.Approximately(radius, builtRadius))
            BuildWindow();
    }

    void BuildWindow()
    {
        const int segments = 64;
        const int rings = 5;
        var vertices = new Vector3[segments * rings];
        var uv = new Vector2[vertices.Length];
        var wallFaces = new List<int>();
        var rimFaces = new List<int>();

        for (int i = 0; i < segments; i++)
        {
            float angle = i * 2f * Mathf.PI / segments;
            float y = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);

            // 기존 직사각형 개구부의 네 모서리까지 덮는 바깥판
            float edge = 1f / Mathf.Max(Mathf.Abs(y) / 0.75f,
                                        Mathf.Abs(z) / 1.06f);

            int k = i * rings;

            // 복도 쪽은 -X. 가운데 단면을 앞으로 내밀어 둥근 턱을 만든다.
            vertices[k]     = new Vector3( 0.06f, y * radius,           z * radius);
            vertices[k + 1] = new Vector3(-0.05f, y * radius,           z * radius);
            vertices[k + 2] = new Vector3(-0.08f, y * (radius + .04f), z * (radius + .04f));
            vertices[k + 3] = new Vector3( 0.00f, y * (radius + .10f), z * (radius + .10f));
            vertices[k + 4] = new Vector3( 0.00f, y * edge,             z * edge);

            for (int j = 0; j < rings; j++)
                uv[k + j] = new Vector2(vertices[k + j].z, vertices[k + j].y);

            int next = ((i + 1) % segments) * rings;
            for (int j = 0; j < rings - 1; j++)
            {
                // 가장 바깥 띠는 벽 재질, 안쪽 세 띠는 금속 재질
                var faces = j == rings - 2 ? wallFaces : rimFaces;
                faces.Add(k + j);
                faces.Add(k + j + 1);
                faces.Add(next + j + 1);
                faces.Add(k + j);
                faces.Add(next + j + 1);
                faces.Add(next + j);
            }
        }

        windowMesh.Clear();
        windowMesh.vertices = vertices;
        windowMesh.uv = uv;
        windowMesh.subMeshCount = 2;
        windowMesh.SetTriangles(wallFaces, 0);
        windowMesh.SetTriangles(rimFaces, 1);
        windowMesh.RecalculateNormals();
        windowMesh.RecalculateTangents();
        windowMesh.RecalculateBounds();
        builtRadius = radius;
    }

    void OnDisable()
    {
        if (windowMesh == null) return;

        GetComponent<MeshFilter>().sharedMesh = null;
        if (Application.isPlaying) Destroy(windowMesh);
        else DestroyImmediate(windowMesh);
    }
}