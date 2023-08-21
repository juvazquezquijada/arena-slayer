using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class PlayerManager : MonoBehaviour
{
	PhotonView PV;

	GameObject controller;
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip fallDeathSound;
	[SerializeField] int kills;
	[SerializeField] int currentKillStreak = 0;
	[SerializeField] int deaths;
	[SerializeField] TMP_Text streakText;
	public float respawnTime = 3f;
	public string playerName;

	private Coroutine clearTextCoroutine; // Add this field
	[SerializeField] GameObject killTextPrefab;
	[SerializeField] GameObject deathText;
	[SerializeField] string[] killStreakMessages = { "Double Kill", "Killing Spree", "Multi Kill", "Rampage!", "Ultra Kill", "Monster Kill", "Unstoppable", "Godlike", "HOLY SHIT"};
	[SerializeField] AudioClip[] killStreakAnnouncerClips;


	void Awake()
	{
		PV = GetComponent<PhotonView>();
		playerName = PhotonNetwork.NickName; // Assuming PhotonNetwork.NickName contains the player's name
		kills = 0;
		deaths = 0;
	}

	void Start()
	{
		if (PV.IsMine)
		{
			CreateController();
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
		Die();
		PV.RPC("RPC_PlayDeathSound", RpcTarget.All);
	}

	public void Die()
	{
		PhotonNetwork.Destroy(controller);
		deaths++;
		currentKillStreak = 0;
		// Instantiate the Kill +1 text UI prefab
		GameObject deathTextPrefab = Instantiate(deathText, transform.position + Vector3.up * 2, Quaternion.identity);
		
		Hashtable hash = new Hashtable();
		hash.Add("deaths", deaths);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		// Create a new controller after a delay
		StartCoroutine(RespawnAfterDelay());
	}

	public void GetKill()
	{
		PV.RPC(nameof(RPC_GetKill), PV.Owner);
	}

	[PunRPC]
	void RPC_GetKill()
	{
		kills++;
		currentKillStreak++;

		Hashtable hash = new Hashtable();
		hash.Add("kills", kills);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

		GameObject killText = Instantiate(killTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
		Destroy(killText, 2f);

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

		streakText.text = streakMessage + "!";

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

}