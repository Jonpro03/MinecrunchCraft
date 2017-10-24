﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Chunks;
using Assets.Scripts.World;

public class Raycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit objectHit;
            if (Physics.Raycast(inputRay, out objectHit))
            {
                Debug.Log("ObjectHit: " + objectHit.collider.gameObject.name);
                
                Chunk chunk = objectHit.collider.GetComponent<Chunk>();
                Debug.Log("Chunk: " + chunk);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit objectHit;
            if (Physics.Raycast(inputRay, out objectHit))
            {
                Vector3 placePos;
                getPlacePos(objectHit.point, Camera.main.transform.forward, out placePos);

                GameObject debugBlockGo = new GameObject("debugBlock(" + placePos.x + ", " + placePos.y + ", " + placePos.z + ")");
                IBlock debugBlock = new GrassBlock(placePos);
                debugBlock.SetAllSidesVisible();
                IEntity debugBlockentity = debugBlockGo.AddComponent<BlockEntity>();
                debugBlockentity.Block = debugBlock;            
            }
        }

    }

    private void getPlacePos(Vector3 objectHitPos, Vector3 cameraForward, out Vector3 placePos)
    {
        /* 
         if raycast hits the the face of a block when the cameraForward is facing in negative position,
         then the objectHitPos only needs to be floor to have appropriate vector for new block     
         */
        if (isWhole(objectHitPos.x))
        {
            if (cameraForward.x > 0)
                objectHitPos.x -= 1;
        }
        else if (isWhole(objectHitPos.y))
        {
            if (cameraForward.y > 0)               
                objectHitPos.y -= 1;
        }
        else
        {
            if (cameraForward.z > 0)
                objectHitPos.z -= 1;
        }

        placePos = new Vector3(
            Mathf.Floor(objectHitPos.x),
            Mathf.Floor(objectHitPos.y),
            Mathf.Floor(objectHitPos.z));
    }

    private bool isWhole(float vectorCoord)
    {
        return vectorCoord == (int)vectorCoord;
    }
}
