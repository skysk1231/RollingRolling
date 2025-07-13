using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> executionQueue = new Queue<Action>();

    private static UnityMainThreadDispatcher instance = null;

    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            var obj = new GameObject("MainThreadDispatcher");
            instance = obj.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(obj);
        }
        return instance;
    }

    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        while (executionQueue.Count > 0)
        {
            executionQueue.Dequeue().Invoke();
        }
    }
}
