using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility;
using System;
using Assets.Scripts.Chunks;

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
                IBlock block = chunk.GetBlock(objectHit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 placePos = Camera.main.transform.position + Camera.main.transform.forward * 2;
            placePos.x = (float)Math.Floor(placePos.x);
            placePos.y = (float)Math.Floor(placePos.y);
            placePos.z = (float)Math.Floor(placePos.z);
            Debug.Log(placePos);


            GameObject debugBlockGo = new GameObject("debugBlock");
            Vector2 chunkLoc;
            Vector3 chunkPos;
            Coordinates.WorldPosToChunkPos(placePos, out chunkPos, out chunkLoc);
            IBlock debugBlock = new GrassBlock(chunkPos, chunkLoc);
            debugBlock.SetAllSidesVisible();
            IEntity debugBlockentity = debugBlockGo.AddComponent<BlockEntity>();
            debugBlockentity.Block = debugBlock;

            Debug.Log("Created new block at: " + debugBlock.PositionInWorld);
            Debug.Log("Created new block at: " + debugBlock.PositionInChunk);
            Debug.Log("Mouse Position: " + Input.mousePosition);
        }

    }
}
