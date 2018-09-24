using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	private bool landed = true;
	private Rigidbody2D rb;
	private CircleCollider2D floorSensor;
	
	public int playerNumber = 0;
	public GameObject subPlayer;
	public float moveSpeed = 1;
	public float jumpHeight;
	private Vector3 playerChainDistance;
	
	// Use this for initialization
	void Start () {
		if (playerNumber == 1)
		{
			GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.22f, 0.25f);
		}
		else if (playerNumber == 2)
		{
			GetComponent<SpriteRenderer>().color = new Color(0.23f, 0.41f, 1f);;
		}

		rb = GetComponent<Rigidbody2D>();
		playerChainDistance = transform.position - subPlayer.transform.position;

	}
	
	// Update is called once per frame
	void Update ()
	{	
		rb.position = subPlayer.transform.position + playerChainDistance;
	}

	private void FixedUpdate()
	{
		move();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Floor")
		{
			landed = true;
		}
	}
	

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Floor")
		{
			landed = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Floor")
		{
			landed = false;
		}
	}


	void move()
	{
		float moveHorizontal = Input.GetAxis("Horizontal" + playerNumber);
		rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);

		
		//Weird jump stuff, holding down the button gives a slightly higher jump than tapping it
		if (Input.GetAxis("Jump" + playerNumber) > 0 && landed)
		{
			rb.AddForce(new Vector2(0.0f, jumpHeight));
		}

		if (Input.GetAxis("Jump" + playerNumber) == 0 && !landed && rb.velocity.y > 0)
		{
			rb.gravityScale = 5;
		}
		else if (!landed && rb.velocity.y < 0)
		{
			rb.gravityScale = 4;
		}
		else
		{
			rb.gravityScale = 3;
		}
		
		
	}
	
	
}
