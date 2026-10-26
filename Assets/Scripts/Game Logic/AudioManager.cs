using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource engineSource; // Motor sesi için loop kullanan özel kaynak

    [Header("Music Tracks")]
    public AudioClip mainMenuBGM;
    public AudioClip gameplayBGM;

    [Header("SFX Clips")]
    public AudioClip asteroidImpactSFX;
    public AudioClip launchChargeSFX;
    public AudioClip launchReleaseSFX;
    public AudioClip scoreGoalSFX;
    public AudioClip countdownWarningSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Tek seferlik efektler için
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    // Müzik değiştirme ve çalma
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PauseBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying) bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        if (bgmSource != null && !bgmSource.isPlaying) bgmSource.UnPause();
    }

    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }

    // Motor sesi kontrolü (İlerleme esnasında)
    public void SetEngineSound(bool isMoving)
    {
        if (engineSource == null) return;

        if (isMoving && !engineSource.isPlaying)
        {
            engineSource.Play();
        }
        else if (!isMoving && engineSource.isPlaying)
        {
            engineSource.Stop();
        }
    }
}