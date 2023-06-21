﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
	NetPlayerController playerController;

	void Awake()
	{
		playerController = GetComponentInParent<NetPlayerController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == playerController.gameObject)
			return;

		playerController.SetGroundedState(true);
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == playerController.gameObject)
			return;

		playerController.SetGroundedState(false);
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == playerController.gameObject)
			return;

		playerController.SetGroundedState(true);
	}
}