using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineControl : FadeScript
{
    public PlayableDirector playableDirector;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            playableDirector.Pause();
            Fade();
        }    
    }
}
