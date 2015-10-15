using UnityEngine;

public class Timer {

    private float startTime = 0;
    private float timeout = 1;
    private bool markedForReset = false;

    private static float TotalElapsedTime = 0;

    public static void Tick(float deltaTime) {
        TotalElapsedTime += deltaTime;
    }

    public Timer(float timeout = 1) {
        this.timeout = timeout;
        startTime = Timer.TotalElapsedTime;
    }

    public bool ReadyWithReset(float timeout) {
        this.timeout = timeout;
        if (markedForReset) {
            Reset(timeout);
            return false;
        } else if (Timer.TotalElapsedTime - startTime > timeout) {
            markedForReset = true;
            return true;
        } else {
            return false;
        }
    }

    public void Reset(float timeout = -1) {
        markedForReset = false;
        startTime = Timer.TotalElapsedTime;
        if (timeout >= 0) {
            this.timeout = timeout;
        }
    }

    public bool Ready {
        get { return Timer.TotalElapsedTime - startTime > timeout; }
    }

    public float Timeout {
        get { return timeout; }
        set { this.timeout = value; }
    }

    public float ElapsedTime {
        get { return Timer.TotalElapsedTime - startTime; }
    }

    public float Timestamp {
        get { return ElapsedTime; }
    }
}