using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility;
using UnityEngine;

public class PlaceBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameObject debugBlockGo = new GameObject("debugBLock");
            Vector2 chunkLoc;
            Vector3 chunkPos;
            Coordinates.WorldPosToChunkPos(Input.mousePosition, out chunkPos, out chunkLoc);
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
