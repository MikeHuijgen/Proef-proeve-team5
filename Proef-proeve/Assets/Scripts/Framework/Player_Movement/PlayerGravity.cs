using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private float _gravityStrength = 20f;

    public float GravityStrength => _gravityStrength;

    public float DownSpeed { get; private set; }

    public Vector3 GravityVelocity { get; private set; }

    public void UpdateGravity(Vector3 gravityDown, bool isGrounded)
    {
        if (gravityDown.sqrMagnitude < 0.0001f)
            return;

        gravityDown.Normalize();

        if (isGrounded)
        {
            if (DownSpeed > 0f) DownSpeed = 0f;
        }
        else
        {
            DownSpeed += _gravityStrength * Time.deltaTime;
        }

        GravityVelocity = gravityDown * DownSpeed;
    }

    public void Jump(float jumpSpeed)
    {
        DownSpeed = -jumpSpeed;
    }

    public void ResetGravity()
    {
        DownSpeed = 0f;
        GravityVelocity = Vector3.zero;
    }
}