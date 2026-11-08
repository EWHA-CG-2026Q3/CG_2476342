using UnityEngine;

public class MantaDiagonalSwim : MonoBehaviour
{
    public Vector3 bottomCorner = new Vector3(3.3f, 1.05f, -2.7f);
    public Vector3 topCorner = new Vector3(7.0f, 2.85f, 2.8f);

    [Min(1f)] public float travelSeconds = 22f;
    [Min(0.1f)] public float turnSeconds = 2.5f;

    void Start()
    {
        transform.position = bottomCorner;
        StartCoroutine(Swim());
    }

    System.Collections.IEnumerator Swim()
    {
        Vector3 from = bottomCorner;
        Vector3 to = topCorner;

        while (true)
        {
            Quaternion facing = Quaternion.LookRotation(to - from);
            transform.rotation = facing;

            for (float t = 0f; t < travelSeconds; t += Time.deltaTime)
            {
                float progress = Mathf.SmoothStep(0f, 1f, t / travelSeconds);
                transform.position = Vector3.Lerp(from, to, progress);
                yield return null;
            }

            transform.position = to;

            Quaternion returnFacing = Quaternion.LookRotation(from - to);
            for (float t = 0f; t < turnSeconds; t += Time.deltaTime)
            {
                float progress = Mathf.SmoothStep(0f, 1f, t / turnSeconds);
                // 수평으로 방향을 돌리면서 오르내리는 기울기도 바꾼다.
                float yaw = facing.eulerAngles.y + 180f * progress;
                float pitch = Mathf.LerpAngle(
                    facing.eulerAngles.x, returnFacing.eulerAngles.x, progress
                );
                transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
                yield return null;
            }

            Vector3 previousStart = from;
            from = to;
            to = previousStart;
        }
    }
}