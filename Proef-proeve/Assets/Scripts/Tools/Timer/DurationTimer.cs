using UnityEngine;

public class DurationTimer : Timer
{
    public DurationTimer(){}
    public DurationTimer(float timeDuration)
    {
        timer_duration = timeDuration;
        RestartTimer();
    }

    public override void Tick(float deltaTime)
    {
        if (is_timer_done || is_timer_paused) return;

        current_time += deltaTime;

        if (timer_duration == 0 &&current_time > timer_duration) is_timer_done = true;
    }
}
