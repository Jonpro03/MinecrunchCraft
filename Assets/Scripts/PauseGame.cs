using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public Transform canvas;
    public Transform Player;

	// Use this for initialization
	void Start () {
        //Starts off game with pause menu down
        canvas.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Brings up pause menu if not already active otherwhys it is closed
            if (canvas.gameObject.activeInHierarchy == false)
                Pause();
            else
                Resume();
            
        }
	}

    public void Pause()
    {
        canvas.gameObject.SetActive(true);
        //Stops time which stops physics
        Time.timeScale = 0;
        //Stop ability for player to move person and camera
        Player.GetComponent<PlayerController>().enabled = false;
        Player.GetComponent<MousePosition>().enabled = false;
    }

    public void Resume()
    {
        canvas.gameObject.SetActive(false);
        //Sets time back to default value
        Time.timeScale = 1;
        //Re-enable ability for player to move person and camera
        Player.GetComponent<PlayerController>().enabled = true;
        Player.GetComponent<MousePosition>().enabled = true;
    }
}
