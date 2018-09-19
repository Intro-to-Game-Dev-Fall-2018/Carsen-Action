using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	public BoxCollider2D leftScore, rightScore;

	private Rigidbody2D rb;
	//public GameObject player1, player2;

	public bool scoredLeft = false;
	public bool scoredRight = false;
	//private int timesBumped = 0;
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other == leftScore)
		{
			scoredLeft = true;
		}
		else if (other == rightScore)
		{
			scoredRight = true;
		}
		else if (rb.isKinematic && other.tag == "Player")
		{
			rb.isKinematic = false;
			rb.freezeRotation = false;
		}
	}

	
}
