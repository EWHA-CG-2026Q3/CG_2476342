using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class StarfishMesh : MonoBehaviour
{
    void Awake()
    {
        const int outlineCount = 10;
        const int front = 10;
        const int back = 11;

        Vector3[] vertices = new Vector3[12];
        int[] triangles = new int[outlineCount * 6];

        // 가운데만 관람객 쪽(+Z)으로 도톰하게 솟도록 한다.
        vertices[front] = new Vector3(0f, 0f, 0.12f);
        vertices[back] = new Vector3(0f, 0f, -0.04f);

        for (int i = 0; i < outlineCount; i++)
        {
            // 짝수 번째 점은 팔 끝, 홀수 번째 점은 팔 사이의 골이다.
            float radius = (i % 2 == 0) ? 0.43f : 0.20f;
            float angle = Mathf.PI / 2f + i * 2f * Mathf.PI / outlineCount;
            vertices[i] = new Vector3(
                radius * Mathf.Cos(angle),
                radius * Mathf.Sin(angle),
                0f
            );

            int next = (i + 1) % outlineCount;
            int t = i * 6;

            triangles[t]     = front;
            triangles[t + 1] = i;
            triangles[t + 2] = next;

            triangles[t + 3] = back;
            triangles[t + 4] = next;
            triangles[t + 5] = i;
        }

        Mesh mesh = new Mesh();
        mesh.name = "Five-Armed Starfish";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
