using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip bgMusic;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main_Menu")
        {
            audioSource.Stop();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.clip = bgMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopBGMusic()
    {
        audioSource.Stop();
    }

    public void PlayBGMusic()
    {
        audioSource.clip = bgMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
