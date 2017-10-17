using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Stop()
    {
#if UNITY_EDITOR
        //Stops game if it is being run in the unity editor
        if (UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        //Need to add functionality for game when actually running that will properly exit/save
    }
}
