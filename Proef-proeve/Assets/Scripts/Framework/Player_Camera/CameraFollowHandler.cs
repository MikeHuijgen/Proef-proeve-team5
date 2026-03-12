using UnityEngine;

public class CameraFollowHandler : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset = new Vector3(0, 1.7f, -3f);
    public float followSpeed = 10f;

    [ExecuteAlways]
    void LateUpdate()
    {
        Vector3 desiredPos = followTarget.TransformPoint(offset);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );

        transform.LookAt(followTarget);
    }

}
