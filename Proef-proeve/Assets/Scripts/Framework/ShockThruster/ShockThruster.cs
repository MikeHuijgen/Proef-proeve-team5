using UnityEngine;

public class ShockThruster : MonoBehaviour
{
    [SerializeField] private float shockInterval;
    [SerializeField] private BoxCollider shockBoxCollider;
    private CountdownTimer _shockTimer;

    private void Awake()
    {
        _shockTimer = new CountdownTimer(shockInterval);
        _shockTimer.StartTimer();
        DisableBoxCollider();
    }

    private void Update()
    {
        _shockTimer.Tick(Time.deltaTime);

        if (!_shockTimer.IsTimerDone) return;

        _shockTimer.StopTimer();

        EnableBoxCollider();

        Invoke("DisableBoxCollider", 1f);

        _shockTimer.StartTimer();
    }

    private void EnableBoxCollider() => shockBoxCollider.enabled = true;
    private void DisableBoxCollider() => shockBoxCollider.enabled = false;
}
