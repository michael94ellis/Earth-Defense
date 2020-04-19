using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
	public TextMeshProUGUI countdownDisplay;
	private GameObject alienShip;

	AlienSpawner alienSpawner;

	float countdown;
	float firstWaveCountdown = 5f;
	float defaultCountdown = 30.5f;
	int waveIndex = 0;

	void Awake()
	{
		alienSpawner = GetComponent<AlienSpawner>();
	}

	void Update()
	{
		if (waveIndex == 0)
		{
			NewWave();
			countdown = firstWaveCountdown;
		}
		else
		{
			if (countdown <= 0)
			{
				NewWave();
				countdown = defaultCountdown;
			}
		}

		countdown -= Time.deltaTime;

		countdownDisplay.text = Mathf.RoundToInt(countdown).ToString();
	}

	void NewWave()
	{
		for (int i = 0; i < waveIndex; i++)
		{
			alienSpawner.NewAlienShip();
		}

		waveIndex++;
	}
}
