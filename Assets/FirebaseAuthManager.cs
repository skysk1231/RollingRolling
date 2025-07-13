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
                Debug.Log("익명 로그인 성공, UID: " + auth.CurrentUser.UserId);
            }
            else
            {
                Debug.LogError("익명 로그인 실패: " + task.Exception);
            }
        });
    }
}

