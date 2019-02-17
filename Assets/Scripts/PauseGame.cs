using UnityEngine;

public class PauseGame : MonoBehaviour {

    public Transform canvas;
    public Transform Player;

	// Use this for initialization
	void Start () {
        //Starts off game with pause menu down
        canvas.gameObject.SetActive(false);
        CaptureMouse();
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
        //Time.timeScale = 0;
        ReleaseMouse();
    }

    public void Resume()
    {
        canvas.gameObject.SetActive(false);
        //Sets time back to default value
        //Time.timeScale = 1;
        CaptureMouse();
    }

    private void CaptureMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ReleaseMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
