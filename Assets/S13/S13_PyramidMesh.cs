using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S13_PyramidMesh : MonoBehaviour
{
    void Awake()
    {
        Mesh mesh = new Mesh();
        mesh.name = "S12 Pyramid";

        // 바닥의 네 꼭짓점과 꼭대기
        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.5f, 0f, -0.5f), // 0
            new Vector3( 0.5f, 0f, -0.5f), // 1
            new Vector3( 0.5f, 0f,  0.5f), // 2
            new Vector3(-0.5f, 0f,  0.5f), // 3
            new Vector3( 0f,   1f, 0f)    // 4
        };

        mesh.triangles = new int[]
        {
            // 옆면 네 개
            0, 4, 1,
            1, 4, 2,
            2, 4, 3,
            3, 4, 0,

            // 바닥
            0, 1, 2,
            0, 2, 3
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}