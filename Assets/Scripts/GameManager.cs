using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	void Start ()
    {
        TerrainManager.Init(gameObject);
	}

    void Update()
    {
        TerrainManager.Update();
    }


}
