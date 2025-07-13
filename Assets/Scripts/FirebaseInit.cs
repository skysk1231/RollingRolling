using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine;

public static class FirebaseInit
{
    public static FirebaseApp App { get; private set; }
    public static DatabaseReference DB { get; private set; }
    public static FirebaseAuth Auth { get; private set; }
    public static bool IsInitialized { get; private set; } = false;

    private static bool isInitStarted = false;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        if (IsInitialized || isInitStarted)
        {
            Debug.Log("FirebaseInit.Init() �̹� �ʱ�ȭ�� �Ǵ� ���۵�");
            return;
        }

        isInitStarted = true;
        Debug.Log("FirebaseInit.Init() ȣ���");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("CheckAndFixDependenciesAsync �Ϸ��");

            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Firebase dependency check failed.");
                return;
            }

            var dependencyStatus = task.Result;
            Debug.Log($"Firebase dependency status: {dependencyStatus}");

            if (dependencyStatus == DependencyStatus.Available)
            {
                App = FirebaseApp.DefaultInstance;

             
                string dbUrl = "https://rollup-rollingrolling-default-rtdb.firebaseio.com/";
                DB = FirebaseDatabase.GetInstance(dbUrl).RootReference;

                Auth = FirebaseAuth.DefaultInstance;
                IsInitialized = true;

                Debug.Log("Firebase �ʱ�ȭ �Ϸ�");
            }
            else
            {
                Debug.LogError($"Firebase dependency error: {dependencyStatus}");
            }
        });
    }
}
