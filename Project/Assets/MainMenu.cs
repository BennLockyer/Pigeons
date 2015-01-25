using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    public AudioClip m_buttonPress;

    public void PlayGame()
    {
        Application.LoadLevel(1);
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            audio.PlayOneShot(m_buttonPress);
            PlayGame();
        }
    }
}
