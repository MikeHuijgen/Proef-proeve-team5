public abstract class Timer 
{
    protected float timer_duration;
    protected float current_time;
    protected bool is_timer_done;
    protected bool is_timer_paused;

    public void RestartTimer()
    {
        is_timer_done = false;
        current_time = 0;
    }

    public void UnPauseTimer() => is_timer_paused = false;
    public void PauseTimer() => is_timer_paused = true;

    public abstract void Tick(float deltaTime);

    public bool IsTimerDone => is_timer_done;
    public float GetCurrentTime => current_time;
}
