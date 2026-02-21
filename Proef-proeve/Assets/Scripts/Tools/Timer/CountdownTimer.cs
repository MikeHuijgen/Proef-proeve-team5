public class CountdownTimer : Timer
{
    public CountdownTimer(float timeDuration)
    {
        timer_duration = timeDuration;
        RestartTimer();
    }

    public override void RestartTimer()
    {
        base.RestartTimer();
        current_time = timer_duration;
    }

    public override void Tick(float deltaTime)
    {
        if (is_timer_done || is_timer_paused || !is_timer_active) return;

        current_time -= deltaTime;

        if (current_time <= 0) is_timer_done = true;
    }
}
