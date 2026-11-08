using UnityEngine;

public class FishSchoolSwim : MonoBehaviour
{
    [Min(1f)] public float lapSeconds = 12f;

    private Vector3 startPosition;
    private float angle;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        angle += 2f * Mathf.PI * Time.deltaTime / lapSeconds;

        // 작은 창 앞에서 출발해서 수조 안쪽으로 타원을 그린다.
        float x = 1.1f * (1f - Mathf.Cos(angle));
        float z = 1.6f * Mathf.Sin(angle);
        transform.position = startPosition + new Vector3(x, 0f, z);

        // 타원의 진행 방향으로 무리 전체가 고개를 돌린다.
        Vector3 direction = new Vector3(
            1.1f * Mathf.Sin(angle),
            0f,
            1.6f * Mathf.Cos(angle)
        );
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
