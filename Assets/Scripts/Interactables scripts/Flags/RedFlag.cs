using System;
using UnityEngine;

public class RedFlag : MonoBehaviour
{
    public static event Action collectRedFlag;

    private bool isCollected = false;

    public void OnInteraction()
    {
        if (isCollected) return;

        collectRedFlag?.Invoke();

        isCollected = true;
    }
}
