using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        SignInAnonymously();
    }

    void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("�͸� �α��� ����, UID: " + auth.CurrentUser.UserId);
            }
            else
            {
                Debug.LogError("�͸� �α��� ����: " + task.Exception);
            }
        });
    }
}

