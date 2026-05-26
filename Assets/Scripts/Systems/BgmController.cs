using System.Collections;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip normalBgm;
    [SerializeField] private AudioClip battleBgm;

    [Header("Distance Settings")]
    [SerializeField] private float encounterDistance = 12f;
    [SerializeField] private float exitDistance = 18f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float maxVolume = 0.6f;
    [SerializeField] private float fadeDuration = 1f;

    private bool _isBattleMode;
    private Coroutine _fadeRoutine;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = maxVolume;
    }

    private void Start()
    {
        PlayBgm(normalBgm, immediate: true);
    }

    private void Update()
    {
        if (player == null || boss == null)
            return;

        float distance = Vector3.Distance(player.position, boss.position);

        if (!_isBattleMode && distance <= encounterDistance)
        {
            _isBattleMode = true;
            PlayBgm(battleBgm, immediate: false);
            return;
        }

        if (_isBattleMode && distance >= exitDistance)
        {
            _isBattleMode = false;
            PlayBgm(normalBgm, immediate: false);
        }
    }

    private void PlayBgm(AudioClip clip, bool immediate)
    {
        if (clip == null)
            return;

        if (audioSource.clip == clip)
            return;

        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        if (immediate)
        {
            audioSource.clip = clip;
            audioSource.volume = maxVolume;
            audioSource.Play();
            return;
        }

        _fadeRoutine = StartCoroutine(FadeToClip(clip));
    }

    private IEnumerator FadeToClip(AudioClip nextClip)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = nextClip;
        audioSource.Play();

        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, maxVolume, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = maxVolume;
        _fadeRoutine = null;
    }
}