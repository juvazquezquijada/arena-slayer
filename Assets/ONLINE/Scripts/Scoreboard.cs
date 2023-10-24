using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Scoreboard : MonoBehaviourPunCallbacks
{
	[SerializeField] Transform container;
	[SerializeField] GameObject scoreboardItemPrefab;
	[SerializeField] CanvasGroup canvasGroup;
	public FFAGameManager gameManager;
	private List<Player> sortedPlayers = new List<Player>();
	Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();

	void Start()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			AddScoreboardItem(player);
		}

		gameManager = FFAGameManager.Instance;
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		AddScoreboardItem(newPlayer);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RemoveScoreboardItem(otherPlayer);
	}

	void AddScoreboardItem(Player player)
	{
		ScoreboardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
		item.Initialize(player);
		scoreboardItems[player] = item;
	}

	void RemoveScoreboardItem(Player player)
	{
		Destroy(scoreboardItems[player].gameObject);
		scoreboardItems.Remove(player);
		
	}


	void UpdateLeaderboard()
	{
		sortedPlayers.Clear();
		sortedPlayers.AddRange(scoreboardItems.Keys);

		// Sort the players based on their kills in descending order
		sortedPlayers.Sort((player1, player2) =>
		{
			int kills1 = GetPlayerKills(player1);
			int kills2 = GetPlayerKills(player2);
			return kills2.CompareTo(kills1); // Compare kills in descending order
		});

		// Reorder the scoreboard items based on the sorted players list
		foreach (var player in sortedPlayers)
		{
			scoreboardItems[player].transform.SetAsLastSibling();
		}
	}
	public void ResetStats()
	{
		foreach (var scoreboardItem in scoreboardItems.Values)
		{
			scoreboardItem.ResetStats();
		}
	}
	int GetPlayerKills(Player player)
	{
		if (player.CustomProperties.TryGetValue("kills", out object kills))
		{
			return (int)kills;
		}
		return 0;
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			canvasGroup.alpha = 1;
			UpdateLeaderboard(); // Update the leaderboard when showing the scoreboard
		}
		else if (Input.GetKeyUp(KeyCode.Tab) && !FFAGameManager.Instance.isGameOver)
		{
			canvasGroup.alpha = 0;
		}

		UpdateLeaderboard(); // Update the leaderboard when showing the scoreboard
	}
}