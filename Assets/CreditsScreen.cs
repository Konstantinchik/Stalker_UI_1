using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    [SerializeField] GameObject credits;
    [SerializeField] GameObject new_screen;
    [SerializeField] GameObject resume_screen;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            credits.SetActive(false);
            if (GameManager.Instance.GameState == GAME_STATE.init) new_screen.SetActive(true);
            if (GameManager.Instance.GameState == GAME_STATE.in_progress) resume_screen.SetActive(true);
            //MenuMusic.SetActive(true);
            GameManager.Instance.PlayMenuMusic(true);
        }
    }
}
