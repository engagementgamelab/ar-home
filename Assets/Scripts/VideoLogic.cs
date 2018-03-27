﻿/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Windows.Speech;

public class VideoLogic : MonoBehaviour
{
	public VideoPlayer videoPlayer;
	public GameObject videoPlayerTexture;
	public GameObject playButton;
	public GameObject restartButton;
	public GameObject stopButton;
	public GameObject promptText;

	public GameObject sampleModel;

	public bool videoPlayed;
	
	private VideoPlayer _video;
	private Camera mainCamera;

	private float videoCurrentTime;
	private bool videoIsPlaying;

	// Use this for initialization
	void Start ()
	{
		
		mainCamera = Camera.main;

		videoPlayer.started += VideoStarted;
		videoPlayer.loopPointReached += ShowPrompt;

		promptText.SetActive(false);		
		
		if(sampleModel != null)
			sampleModel.SetActive(false);

//		videoPlayerTexture.SetActive(false);
		
	}

	private void Update()
	{
		if(videoIsPlaying)
			videoCurrentTime += Time.deltaTime;

		if (videoCurrentTime > 5 && !sampleModel.active)
		{
			if(sampleModel != null)
				sampleModel.SetActive(true);

		}
	}

	private void OnDestroy()
	{
		videoPlayer.started -= VideoStarted;
		videoPlayer.loopPointReached -= ShowPrompt;
	}

	private void VideoStarted(VideoPlayer source)
	{
		// Disable billboard when watching
		GetComponent<Billboard>().enabled = false;
	
		videoIsPlaying = true;
	}

	void ShowPrompt(VideoPlayer source)
	{
		
		promptText.SetActive(true);
		
//		videoPlayerTexture.SetActive(false);
		stopButton.SetActive(false);

		videoPlayed = true;
		videoIsPlaying = false;

		// Re-enable billboard
		GetComponent<Billboard>().enabled = true;
	}

	// Called via OnDownEvrnt on caller interactive object
	public void StartVideo()
	{
        
		Vector3 playerPos = mainCamera.transform.position;
		Vector3 playerDirection = mainCamera.transform.forward;
		transform.position = playerPos + (playerDirection * 2);
		
		videoPlayer.Play();
		videoPlayerTexture.SetActive(true);
		
		playButton.SetActive(false);
		GetComponent<VideoTagalong>().VideoStart();

		// Hide caller object
		Debug.Log("Hide "  + EventSystem.current.currentSelectedGameObject.name);
		EventSystem.current.currentSelectedGameObject.SetActive(false);

	}
	
	public void StopVideo()
	{
		videoPlayerTexture.SetActive(false);
		stopButton.SetActive(false);
		
		if (videoPlayed)
		{
			restartButton.SetActive(true);
			promptText.SetActive(true);
			GetComponent<VideoTagalong>().SphereRadius = 4;
		}
		else
		{
			playButton.SetActive(true);
			GetComponent<VideoTagalong>().VideoStop();
		}
	}
}
