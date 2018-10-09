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
	public float minSpeedX, minSpeedY, maxSpeedX, maxSpeedY;
	public AudioClip hitPlayerSound, stopBallSound;
	public float maxCooldown;
	public Slider p1CooldownMeter, p2CooldownMeter;
	public float ballFreezeTime;

	private int currentHitter = 0;
	private bool betweenHits = false;

	private Rigidbody2D rb;
	private AudioSource aso;
	private ParticleSystem particles;

	[HideInInspector] public bool Player2Scored = false;
	[HideInInspector] public bool Player1Scored = false;
	[HideInInspector] public int timesHit = 0;
	
//	public Vector2 rbvel;
	
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		aso = GetComponent<AudioSource>();
		particles = GetComponent<ParticleSystem>();
		
		player1Hits.text = "";
		player2Hits.text = "";

		p1CooldownMeter.maxValue = maxCooldown;
		p1CooldownMeter.value = maxCooldown;
		
		p2CooldownMeter.maxValue = maxCooldown;
		p2CooldownMeter.value = maxCooldown;

	}
	
	// Update is called once per frame
	void Update () {
		
		StopTimePower();
		
		p1CooldownMeter.value += Time.deltaTime;
		p2CooldownMeter.value += Time.deltaTime;
		
		
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

	private void FixedUpdate()
	{
//		For velocity debugging:
//		rbvel = rb.velocity;
		
		
		//Limit minimum speed so it doesn't go too slow
		if (!rb.isKinematic)
		{
			if ((rb.velocity.x < minSpeedX && rb.velocity.x > 0) || (rb.velocity.x > -minSpeedX && rb.velocity.x < 0))
			{
				if (rb.velocity.x > 0)
				{
					rb.velocity = new Vector2(minSpeedX, rb.velocity.y);
				}
				else if (rb.velocity.x < 0)
				{
					rb.velocity = new Vector2(-minSpeedX, rb.velocity.y);
				}
			}

			if ((rb.velocity.y < minSpeedY && rb.velocity.y > 0) || (rb.velocity.y > -minSpeedY && rb.velocity.y <= 0))
			{
				if (rb.velocity.y > 0)
				{
					rb.velocity = new Vector2(rb.velocity.x, minSpeedY);
				}
				else if (rb.velocity.y <= 0)
				{
					rb.velocity = new Vector2(rb.velocity.x, -minSpeedY);
				}
			}
		}


		//Limit max speed so it doesn't think its Speed Racer
		if ((rb.velocity.x > maxSpeedX && rb.velocity.x > 0) || (rb.velocity.x < -maxSpeedX && rb.velocity.x < 0))
		{
			if (rb.velocity.x > 0)
			{
				rb.velocity = new Vector2(maxSpeedX, rb.velocity.y);
			}
			else if (rb.velocity.x < 0)
			{
				rb.velocity = new Vector2(-maxSpeedX, rb.velocity.y);
			}
		}
		
		if ((rb.velocity.y > maxSpeedY && rb.velocity.y >= 0) || (rb.velocity.y < -maxSpeedY && rb.velocity.y < 0))
		{
			if (rb.velocity.y > 0)
			{
				rb.velocity = new Vector2(rb.velocity.x, maxSpeedY);
			}
			else if (rb.velocity.y < 0)
			{
				rb.velocity = new Vector2(rb.velocity.x, -maxSpeedY);
			}
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

	/*If the ball hits a player, it will increase the hit count by 1. If the hit count > 3, the other player scores.
		 It also plays a different noise if the player hits the ball. Displays a small counter for the amount of hits.*/
	private void OnCollisionEnter2D(Collision2D other)
	{
		
		if (other.gameObject.CompareTag("Player"))
		{
			GameObject player = other.gameObject;
			aso.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
			aso.PlayOneShot(hitPlayerSound);


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

		else
		{
			aso.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
			aso.Play();
		}
	}

	
	//Small timer between hits so two super fast hits in succession will not instantly make you lose the round.
	IEnumerator HitTimer()
	{
		betweenHits = true;
		yield return new WaitForSeconds(0.5f);
		betweenHits = false;
	}

	
	//A player ability which stops the ball for a short time!
	void StopTimePower()
	{
		
		if (Input.GetKeyDown(KeyCode.E) && p1CooldownMeter.value == maxCooldown)
		{
			rb.isKinematic = true;
			rb.velocity = new Vector2(0f, 0f);
			p1CooldownMeter.value = 0;
			StartCoroutine(BallIsFrozen());
		}
		if (Input.GetKeyDown(KeyCode.RightShift) && p2CooldownMeter.value == maxCooldown)
		{
			rb.isKinematic = true;
			rb.velocity = new Vector2(0f, 0f);
			p2CooldownMeter.value = 0;
			StartCoroutine(BallIsFrozen());
		}
		
	}

	IEnumerator BallIsFrozen()
	{
		aso.PlayOneShot(stopBallSound);
		particles.Play();
		yield return new WaitForSeconds(ballFreezeTime);
		rb.isKinematic = false;
	}

	
}
