using UnityEngine;

public interface IHUDObserver
{
    void OnLifeChanged(int currentLife);
    void OnScoreChanged(int newScore);
}
