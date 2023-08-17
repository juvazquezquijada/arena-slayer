using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
	PhotonView PV;

	GameObject controller;

	[SerializeField] int kills;
	[SerializeField] int deaths;
	public float respawnTime = 3f;
	public string playerName;

	[SerializeField] GameObject killTextPrefab;
	[SerializeField] GameObject deathText;

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

	public void Die()
	{
		PhotonNetwork.Destroy(controller);
		deaths++;

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

		Hashtable hash = new Hashtable();
		hash.Add("kills", kills);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

		// Instantiate the Kill +1 text UI prefab
		GameObject killText = Instantiate(killTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
		Destroy(killText, 2f); // Destroy the text after 2 seconds
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
}