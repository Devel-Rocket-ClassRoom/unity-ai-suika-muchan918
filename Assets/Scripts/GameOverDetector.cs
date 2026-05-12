using System.Collections.Generic;
using UnityEngine;

public class GameOverDetector : MonoBehaviour
{
    [SerializeField] private float dangerTime = 3f;

    public static event System.Action OnGameOver;

    // DropController가 드롭 직후 호출해 오탐 방지
    public static float IgnoreUntilTime;

    private readonly Dictionary<Fruit, float> _enterTimes = new();
    private bool _triggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        var f = other.GetComponent<Fruit>();
        if (f != null && !_enterTimes.ContainsKey(f))
            _enterTimes[f] = Time.time;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (_triggered) return;
        if (Time.time < IgnoreUntilTime) return;

        var f = other.GetComponent<Fruit>();
        if (f == null || !_enterTimes.TryGetValue(f, out float enterTime)) return;

        if (Time.time - enterTime >= dangerTime)
        {
            _triggered = true;
            OnGameOver?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var f = other.GetComponent<Fruit>();
        if (f != null) _enterTimes.Remove(f);
    }

    public void ResetState()
    {
        _enterTimes.Clear();
        _triggered = false;
    }
}
