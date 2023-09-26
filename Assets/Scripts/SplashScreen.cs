using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup splashScreen;

    [SerializeField]
    private float fadespeed;

    private bool fadein;
    private bool fadeout;

    void Start()
    {
        fadein = true;
        fadeout = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadein == true)
        {
            splashScreen.alpha += fadespeed * Time.deltaTime;
            if (splashScreen.alpha >= 1)
            {
                fadein = false;
                fadeout = true;
            }
        }
        if (fadeout == true)
        {
            splashScreen.alpha -= fadespeed * Time.deltaTime;
            if (splashScreen.alpha <= 0)
            {
                fadeout = false;
                SceneManager.LoadScene(1);
            }
        }
    }
}
