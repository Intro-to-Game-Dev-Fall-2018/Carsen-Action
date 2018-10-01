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
	private AudioSource aso;

	public int pointsPlayer1 = 0;
	public int pointsPlayer2 = 0;
	public int maxScore = 10;
	public GameObject leftBallSpawn, rightBallSpawn, leftConfetti, rightConfetti;
	public Text player1Score, player2Score, announcementText;
	
	
	
	// Use this for initialization
	void Start () {
		ball = GameObject.FindWithTag("Ball");
		ballDetection = ball.GetComponent<BallScript>();
		aso = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		PlayerScoreCheck();
		ChangeScoreText();
	

		if (betweenRounds)
		{
			ballDetection.timesHit = 0;
		}

		if (Input.GetKeyUp(KeyCode.R))
		{
			ResetGame();
		}
			
	}

	
	IEnumerator JustScored(int playerNumber)
	{
		aso.pitch = UnityEngine.Random.Range(0.93f, 1.11f);
		aso.Play();
		
		yield return new WaitForSeconds(2.5f);
		
		if (playerNumber == 1)
		{
			ball.transform.position = rightBallSpawn.transform.position;
			ballDetection.Player1Scored = false;
			leftConfetti.SetActive(false);
		}
		else if (playerNumber == 2)
		{
			ball.transform.position = leftBallSpawn.transform.position;
			ballDetection.Player2Scored = false;
			rightConfetti.SetActive(false);
		}

		ball.GetComponent<Rigidbody2D>().isKinematic = true;
		ball.GetComponent<Rigidbody2D>().freezeRotation = true;
		ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		ballDetection.Player1Scored = false;
		ballDetection.Player2Scored = false;
		betweenRounds = false;
		
		aso.Stop();
	}

	
	void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	
	void PlayerScoreCheck()
	{
		if (ballDetection.Player2Scored && !betweenRounds)
		{
			pointsPlayer2 += 1;
			betweenRounds = true;
			//rightConfetti.SetActive(true);
			if (pointsPlayer2 >= maxScore)
			{
				announcementText.text = "PLAYER 2 WINS!";
				announcementText.color = new Color(0.23f, 0.41f, 1f);
				announcementText.enabled = true;
				
				aso.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
				aso.Play();
				
			}
			else
				StartCoroutine(JustScored(2));

		}
		else if (ballDetection.Player1Scored && !betweenRounds)
		{
			pointsPlayer1 += 1;
			betweenRounds = true;
			//leftConfetti.SetActive(true);
			if (pointsPlayer1 >= maxScore)
			{
				announcementText.text = "PLAYER 1 WINS!";
				announcementText.color = new Color(0.8f, 0.22f, 0.25f);
				announcementText.enabled = true;
				
				aso.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
				aso.Play();
			
			}
			else
				StartCoroutine(JustScored(1));
			
		}
	}

	void ChangeScoreText()
	{
		if (pointsPlayer1 < 10) player1Score.text = "0" + pointsPlayer1;
		else player1Score.text = "" + pointsPlayer1;
		
		if(pointsPlayer2 < 10) 	player2Score.text = "0" + pointsPlayer2;
		else 	player2Score.text = "" + pointsPlayer2;
	}

}
