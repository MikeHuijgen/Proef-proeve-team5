public class CountdownTimer : Timer
{
    public CountdownTimer(float timeDuration)
    {
        timer_duration = timeDuration;
        RestartTimer();
    }

    public override void Tick(float deltaTime)
    {
        if (is_timer_done || is_timer_paused) return;

        current_time -= deltaTime;

        if (current_time <= 0) is_timer_done = true;
    }
}
