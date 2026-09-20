using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S04_DiamondMesh : MonoBehaviour
{
    void Start()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),     // 0
            new Vector3(1f, 0f, 0f),     // 1
            new Vector3(1f, 0f, 1f),     // 2
            new Vector3(0f, 0f, 1f),     // 3
            new Vector3(0.5f, 1f, 0.5f), // 4: 위 꼭짓점
            new Vector3(0.5f, -1f, 0.5f), // 5: 아래 꼭짓점
        };

        // TODO: 위쪽 삼각형 4개(정점 4 + 허리띠 인접 두 점)와
        //       아래쪽 삼각형 4개(정점 5 + 허리띠 인접 두 점)를 채우세요.
        // 힌트: 허리띠는 0→1→2→3→(다시 0) 순서로 이어짐
        int[] triangles = new int[]
        {

            // 위쪽 4면
            0, 4, 1,
            1, 4, 2,
            2, 4, 3,
            3, 4, 0,

            // 아래쪽 4면
            0, 1, 5,
            1, 2, 5,
            2, 3, 5,
            3, 0, 5,

        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}