using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    GameObject[] InitiativeOrder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitiativeSetup()
    {
        // get everyone in room. 

        // sort by speed score or something 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.GetComponent<Player>())
        {
            //StartBattle
            print("starting Combat Operations");
        }
    }
}
