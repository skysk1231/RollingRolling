using UnityEngine;
using UnityEngine.SceneManagement;


public class bgmManager : MonoBehaviour
{
    public static bgmManager instance;

    public AudioClip titleBGM;   // 타이틀 화면 BGM
    public AudioClip gameBGM;    // 게임 플레이 BGM
    public AudioClip gameoverBGM; // 루프 해제할 특정 씬의 BGM

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시 삭제되지 않음
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // 기본적으로 루프 활성화
        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 변경 감지
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            PlayBGM(titleBGM, true); // 타이틀 화면 BGM (루프 O)
        }
        else if (scene.name == "GameScene")
        {
            PlayBGM(gameBGM, true); // 게임 진행 중 BGM (루프 O)
        }
        else if (scene.name == "GameOver") // 루프 해제할 씬
        {
            PlayBGM(gameoverBGM, false); // 루프 해제
        }
    }

    public void PlayBGM(AudioClip clip, bool loop)
    {
        if (audioSource.clip == clip) return; // 같은 음악이면 변경하지 않음
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop; // 씬에 따라 루프 여부 설정
        audioSource.Play();
    }
}
