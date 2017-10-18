using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using UnityEngine;

public class Raycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("Fire"))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit objectHit;
            if (Physics.Raycast(inputRay, out objectHit))
            {
                Block block = objectHit.collider.GetComponent<Block>();
                block.OnTakeDamage(1.0f);
            }
        }
        
    }
}
