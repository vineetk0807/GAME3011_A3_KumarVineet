using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public enum MusicTracks
    {
        MAINMENU,
        GAME,
        TOTAL_TYPES
    }

    private AudioSource audioSrc;

    public List<AudioClip> musictrack;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSrc.clip = musictrack[(int)MusicTracks.MAINMENU];
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            audioSrc.clip = musictrack[(int)MusicTracks.GAME];
        }

        audioSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
