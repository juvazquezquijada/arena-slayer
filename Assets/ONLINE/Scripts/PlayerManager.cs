using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class PlayerManager : MonoBehaviourPunCallbacks
{
	PhotonView PV;

	GameObject controller;
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip fallDeathSound, humiliationSound;
	[SerializeField] int kills;
	[SerializeField] int currentKillStreak = 0;
	[SerializeField] int currentDeathStreak = 0;
	[SerializeField] int deaths;
	[SerializeField] TMP_Text streakText, notificationText;
	public float respawnTime = 3f;
	public string playerName;

	private Coroutine clearTextCoroutine; // Add this field
	[SerializeField] GameObject killTextPrefab;
	[SerializeField] GameObject deathText;
	[SerializeField] string[] killStreakMessages = { "Double Kill!", "Killing Spree!", "Multi Kill!", "Rampage!", "Ultra Kill!", "Monster Kill!!!", "Unstoppable!", "GODLIKE!", "HOLY SHIT!!!!!!!"};
	[SerializeField] AudioClip[] killStreakAnnouncerClips;


	void Awake()
	{
		PV = GetComponent<PhotonView>();
		playerName = PhotonNetwork.NickName; // Assuming PhotonNetwork.NickName contains the player's name
		Hashtable hash = new Hashtable();
		hash.Add("kills", 0);
		hash.Add("deaths", 0);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
	}

	void Start()
	{
		if (PV.IsMine)
		{
			CreateController();
			playerName = PhotonNetwork.NickName; // Store the player's name
			
		}
	}


	void CreateController()
	{
		Transform spawnpoint = PlayerSpawner.Instance.GetSpawnPoint();
		controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
		if (!deathText == null)
		{
			Destroy(deathText); // Destroy the text
		}

		
	}

	public void Fall()
	{
		string killerName = "the void";
		Die(killerName);
		PV.RPC("RPC_PlayDeathSound", RpcTarget.All);
	}

	public void Die(string killerName)
	{
		PhotonNetwork.Destroy(controller);
		deaths++;
		currentKillStreak = 0;
		currentDeathStreak++;
		if (currentDeathStreak >= 3)
		{
			Debug.Log("Calling PlayHumiliation");
			PlayHumiliation();
		}
		if (PV.IsMine)
		{
			// Display the "killed by [KillerName]" message
			string deathMessage = "You were killed by " + killerName;
			Debug.Log(deathMessage);

			// Set the TMP text to display the notification
			notificationText.text = deathMessage;
			GameObject deathTextPrefab = Instantiate(deathText, transform.position + Vector3.up * 2, Quaternion.identity);
		}

		FFAGameManager.Instance.ShowCamera();

		
		// Create a new controller after a delay
		StartCoroutine(RespawnAfterDelay());

		// Start a coroutine to clear the notification after 2 seconds
		StartCoroutine(ClearNotificationText());
		Hashtable hash = new Hashtable();
		hash.Add("deaths", deaths);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		
	}

	public void GetKill(string killedPlayerName)
	{
		PV.RPC(nameof(RPC_GetKill), RpcTarget.All, killedPlayerName);
	}


	[PunRPC]
	void RPC_GetKill(string killedPlayerName)
	{

		// award a point to the player
		if (PV.IsMine)
		{
			currentKillStreak++;
			currentDeathStreak = 0;
			// Display the "killed [PlayerName]" text
			string killMessage = "You killed " + killedPlayerName;
			Debug.Log(killMessage);

			// Set the TMP text to display the notification
			notificationText.text = killMessage;

			// Start a coroutine to clear the notification after 2 seconds
			StartCoroutine(ClearNotificationText());

			// Update only the local player's kills
			kills++;

			// Update the local player's kills using Player.CustomProperties
			Hashtable playerProps = PhotonNetwork.LocalPlayer.CustomProperties;
			if (playerProps.ContainsKey("kills"))
			{
				playerProps["kills"] = kills;
			}
			else
			{
				playerProps.Add("kills", kills);
			}

			// Apply the updated properties
			PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
		}


		string streakMessage = "";

		if (currentKillStreak >= 10)
		{
			streakMessage = "HOLY SHIT!";
			streakText.GetComponent<AudioSource>().PlayOneShot(killStreakAnnouncerClips[killStreakAnnouncerClips.Length - 1]); // Play the last clip
		}
		else if (currentKillStreak >= 2 && currentKillStreak - 2 <= killStreakMessages.Length)
		{
			streakMessage = killStreakMessages[currentKillStreak - 2]; // -2 because arrays are zero-based
		}

		streakText.text = streakMessage;

		// Stop any ongoing coroutine and start a new one
		if (clearTextCoroutine != null)
		{
			StopCoroutine(clearTextCoroutine);
		}
		clearTextCoroutine = StartCoroutine(ClearStreakText());

		if (currentKillStreak >= 2 && currentKillStreak - 2 < killStreakAnnouncerClips.Length)
		{
			streakText.GetComponent<AudioSource>().PlayOneShot(killStreakAnnouncerClips[currentKillStreak - 2]);
		}
	}

	void PlayHumiliation()
	{
		Debug.Log("PlayHumiliation method called");
		audioSource.PlayOneShot(humiliationSound);
		// Set streak text to "Humiliation"
		streakText.text = "Humiliation!";

		// Clear the text after a delay
		StartCoroutine(ClearStreakText());
	}

	[PunRPC]
	void RPC_PlayDeathSound()
	{
		audioSource.PlayOneShot(fallDeathSound);
	}

	public static PlayerManager Find(Player player)
	{
		return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
	}

	

	IEnumerator RespawnAfterDelay()
	{
		yield return new WaitForSeconds(respawnTime); // Adjust this delay as needed
		CreateController();
		
	}

	IEnumerator ClearStreakText()
	{
		yield return new WaitForSeconds(3f); // Wait for 3 seconds
		streakText.text = ""; // Clear the text
	}

	IEnumerator ClearNotificationText()
	{
		// Wait for 2 seconds
		yield return new WaitForSeconds(2f);

		// Clear the TMP text
		notificationText.text = "";
	}



}