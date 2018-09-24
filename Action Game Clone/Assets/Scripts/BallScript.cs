using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = System.Random;

public class BallScript : MonoBehaviour {

	public BoxCollider2D leftScore, rightScore;
	public float hitStrength = 1;
	public Text player1Hits, player2Hits;

	private int currentHitter = 0;
	private bool betweenHits = false;
	//public float standingHitStrength = 3;

	private Rigidbody2D rb;
	private AudioSource aso;
	//public GameObject player1, player2;

	[HideInInspector] public bool Player2Scored = false;
	[HideInInspector] public bool Player1Scored = false;
	[HideInInspector] public int timesHit = 0;
	
	//private int timesBumped = 0;
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		aso = GetComponent<AudioSource>();
		
		player1Hits.text = "";
		player2Hits.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (timesHit > 3)
		{
			if (currentHitter == 1) Player2Scored = true;
			else if (currentHitter == 2) Player1Scored = true;

			timesHit = 0;
			
			
		}

		if (timesHit == 0)
		{
			player1Hits.text = "";
			player2Hits.text = "";
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other == leftScore)
		{
			Player2Scored = true;
		}
		else if (other == rightScore)
		{
			Player1Scored = true;
		}
		else if (rb.isKinematic && other.tag == "Player")
		{
			rb.isKinematic = false;
			rb.freezeRotation = false;
		}

		
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		
		aso.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		aso.Play();
		
		if (other.gameObject.CompareTag("Player"))
		{
			GameObject player = other.gameObject;
			Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
			
			rb.AddForce(new Vector2(playerRB.velocity.x, playerRB.velocity.y) * hitStrength);

			if (!betweenHits)
			{
				StartCoroutine(HitTimer());

				if (player.GetComponent<PlayerMovement>().playerNumber == 1)
				{
					if (currentHitter == 1)
					{
						timesHit += 1;

					}
					else if (currentHitter != 1)
					{
						currentHitter = 1;
						timesHit = 1;
						player2Hits.text = "";
					}

					player1Hits.text += ".";
				}
				else if (player.GetComponent<PlayerMovement>().playerNumber == 2)
				{
					if (currentHitter == 2)
					{
						timesHit += 1;
					}
					else if (currentHitter != 2)
					{
						currentHitter = 2;
						timesHit = 1;
						player1Hits.text = "";
					}

					player2Hits.text += ".";
				}
			}


		}
	}

	IEnumerator HitTimer()
	{
		betweenHits = true;
		yield return new WaitForSeconds(0.5f);
		betweenHits = false;
	}
}
