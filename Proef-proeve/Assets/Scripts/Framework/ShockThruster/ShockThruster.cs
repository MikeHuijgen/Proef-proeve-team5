using UnityEngine;

public class ShockThruster : MonoBehaviour
{
    [SerializeField] private float shockInterval;
    [SerializeField] private SphereCollider shockSphereCollider;
    [SerializeField] private GameObject particle;
    private CountdownTimer _shockTimer;

    private void Awake()
    {
        _shockTimer = new CountdownTimer(shockInterval);
        _shockTimer.StartTimer();
        DisableBoxCollider();
        DisableParticle();
    }

    private void Update()
    {
        _shockTimer.Tick(Time.deltaTime);

        if (!_shockTimer.IsTimerDone) return;

        _shockTimer.StopTimer();
        EnableParticle();
        EnableBoxCollider();

        Invoke("DisableBoxCollider", 1f);
        Invoke("DisableParticle", 1f);

        _shockTimer.StartTimer();
    }

    private void EnableBoxCollider() => shockSphereCollider.enabled = true;
    private void DisableBoxCollider() => shockSphereCollider.enabled = false;
    private void EnableParticle() => particle.SetActive(true);
    private void DisableParticle() => particle.SetActive(false);
}
