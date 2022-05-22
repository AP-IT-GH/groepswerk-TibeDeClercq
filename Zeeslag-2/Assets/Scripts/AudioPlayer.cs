using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    private bool playing = false;

    public void Play(AudioClip audio)
    {
        source.PlayOneShot(audio);
        playing = true;
    }

    //Destroy audio player when audio is done playing
    private void Update()
    {
        if (!source.isPlaying && playing)
        {
            Destroy(gameObject);
        }
    }
}
