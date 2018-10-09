using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarVisuals : MonoBehaviour
{
	private Text readyText;
	private Slider meterSlider;
	public Image fillObject;
	public Color emptyColor, fullColor;
	
	// Use this for initialization
	void Start ()
	{
		readyText = GetComponentInChildren<Text>();
		meterSlider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		fillObject.color = Color.Lerp(emptyColor, fullColor, meterSlider.normalizedValue);

		if (meterSlider.value == meterSlider.maxValue)
		{
			readyText.enabled = true;
		}
		else
		{
			readyText.enabled = false;
		}
	}
}
