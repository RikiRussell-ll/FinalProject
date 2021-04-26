using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public RubyController rScript;
    public AudioClip DefaultMusic;
    public AudioClip WinMusic;
    public AudioClip LoseMusic;
    public AudioSource musicSource;
    bool FinishedGame;
    // Start is called before the first frame update
    void Start()
    {
        FinishedGame = false;
        musicSource.clip = DefaultMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(rScript.gameWin == true)
        {
            if(FinishedGame == false)
            {
            musicSource.clip = WinMusic;
            musicSource.Play();
            FinishedGame = true;
            }
        }
        if(rScript.gameOver == true)
        {
            if(FinishedGame == false)
            {
            musicSource.clip = LoseMusic;
            musicSource.Play();
            FinishedGame = true;
            }
        }
    }
}
