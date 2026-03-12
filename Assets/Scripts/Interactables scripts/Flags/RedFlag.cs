using System;
using UnityEngine;

public class RedFlag : MonoBehaviour, IInteractable
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
