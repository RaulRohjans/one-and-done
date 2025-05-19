using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip gameMusic, deathMusic, winMusic;

    private void Awake()
    {
        // keep the gameObject across scene reloads
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGameMusic()
    {
        PlayTrack(gameMusic, loop: true, volume: 0.15f);
    }

    public void PlayDeathMusic()
    {
        PlayTrack(deathMusic, loop: false);
    }

    public void PlayWinMusic()
    {
        PlayTrack(winMusic, loop: true);
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    
    private void PlayTrack(AudioClip clip, bool loop, float volume = 1f)
    {
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
