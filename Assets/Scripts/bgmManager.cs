using UnityEngine;
using UnityEngine.SceneManagement;


public class bgmManager : MonoBehaviour
{
    public static bgmManager instance;

    public AudioClip titleBGM;   // Ÿ��Ʋ ȭ�� BGM
    public AudioClip gameBGM;    // ���� �÷��� BGM
    public AudioClip gameoverBGM; // ���� ������ Ư�� ���� BGM

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ���� �� �������� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // �⺻������ ���� Ȱ��ȭ
        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded; // �� ���� ����
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            PlayBGM(titleBGM, true); // Ÿ��Ʋ ȭ�� BGM (���� O)
        }
        else if (scene.name == "GameScene")
        {
            PlayBGM(gameBGM, true); // ���� ���� �� BGM (���� O)
        }
        else if (scene.name == "GameOver") // ���� ������ ��
        {
            PlayBGM(gameoverBGM, false); // ���� ����
        }
    }

    public void PlayBGM(AudioClip clip, bool loop)
    {
        if (audioSource.clip == clip) return; // ���� �����̸� �������� ����
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop; // ���� ���� ���� ���� ����
        audioSource.Play();
    }
}
