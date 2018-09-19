using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundSystem : MonoBehaviour
{
	private bool betweenRounds = false;
	//private bool startingRound = true;
	GameObject ball;
	BallScript ballDetection;

	public int pointsPlayer1 = 0;
	public int pointsPlayer2 = 0;
	public int maxScore = 10;
	public GameObject leftBallSpawn, rightBallSpawn, leftConfetti, rightConfetti;
	public Text player1Score, player2Score, announcementText;
	
	
	
	// Use this for initialization
	void Start () {
		ball = GameObject.FindWithTag("Ball");
		ballDetection = ball.GetComponent<BallScript>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ballDetection.scoredLeft && !betweenRounds)
		{
			pointsPlayer2 += 1;
			betweenRounds = true;
			rightConfetti.SetActive(true);
			if (pointsPlayer2 >= maxScore)
			{
				announcementText.text = "Player 2 Wins!";
				announcementText.color = Color.blue;
				announcementText.enabled = true;
				
			}
			else
				StartCoroutine(JustScored(2));

		}
		else if (ballDetection.scoredRight && !betweenRounds)
		{
			pointsPlayer1 += 1;
			betweenRounds = true;
			leftConfetti.SetActive(true);
			if (pointsPlayer1 >= maxScore)
			{
				announcementText.text = "Player 1 Wins!";
				announcementText.color = Color.red;
				announcementText.enabled = true;
			
			}
			else
				StartCoroutine(JustScored(1));
			
		}

		player1Score.text = "" + pointsPlayer1;
		player2Score.text = "" + pointsPlayer2;

		if (Input.GetKeyUp(KeyCode.R))
		{
			ResetGame();
		}
			
	}

	IEnumerator JustScored(int playerNumber)
	{
		yield return new WaitForSeconds(2.5f);
		if (playerNumber == 1)
		{
			ball.transform.position = rightBallSpawn.transform.position;
			ballDetection.scoredRight = false;
			leftConfetti.SetActive(false);
		}
		else if (playerNumber == 2)
		{
			ball.transform.position = leftBallSpawn.transform.position;
			ballDetection.scoredLeft = false;
			rightConfetti.SetActive(false);
		}

		ball.GetComponent<Rigidbody2D>().isKinematic = true;
		ball.GetComponent<Rigidbody2D>().freezeRotation = true;
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		betweenRounds = false;
	}

	void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

}
