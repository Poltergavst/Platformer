using System;
using UnityEngine;

public class ICollectable : MonoBehaviour
{
    public static event Action<ICollectable> Collected;

    public void Collect()
    {
        Collected?.Invoke(this);
    }
}
