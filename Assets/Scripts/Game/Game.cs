#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public Player activePlayer;
	public List<PlayerGhost> ghostPlayers = new List<PlayerGhost>();
	public List<Past> pasts = new List<Past>();

	[SerializeField] GameObject pfPlayer;
	[SerializeField] GameObject pfGhostPlayer;
	[Space]
	[SerializeField] Transform tSpawnPoint;

	void Awake()
	{
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();
	}

	void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			Redo();
	}

	public void Redo()
	{
		pasts.Add(activePlayer.past);
		Destroy(activePlayer.gameObject);
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();

		ghostPlayers.ForEach(x => Destroy(x.gameObject));
		ghostPlayers = new List<PlayerGhost>();
		pasts.ForEach(x => ghostPlayers.Add(Instantiate(pfGhostPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<PlayerGhost>().SetPast(x)));
	}
}