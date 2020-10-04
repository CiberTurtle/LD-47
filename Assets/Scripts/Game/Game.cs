#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ciber_Turtle.UI;

public class Game : MonoBehaviour
{
	public Player activePlayer;
	public List<PlayerGhost> ghostPlayers = new List<PlayerGhost>();
	public List<Past> pasts = new List<Past>();

	[SerializeField] GameObject pfPlayer;
	[SerializeField] GameObject pfGhostPlayer;
	[Space]
	[SerializeField] Transform tSpawnPoint;
	[SerializeField] Transform tEndPoint;
	[SerializeField] float fTimeToExit;
	[SerializeField] LayerMask lmPlayer;
	[SerializeField] GameObject goDevUI;
	[Space]
	[SerializeField] float fRestartTime;
	[SerializeField] UIProgressBar barRestart;
	[SerializeField] UIBitText goRestartText;
	[Space]
	[SerializeField] float fCamSmoothing;
	[SerializeField] float fCamAmp;
	[SerializeField] Transform cam;

	float fTimeOnExit;
	float fTimeRestarted;

	void Awake()
	{
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();
	}

	void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
		else if (Input.GetKeyDown(KeyCode.F3)) goDevUI.SetActive(!goDevUI.activeInHierarchy);

		if (Input.GetKey(KeyCode.R))
		{
			fTimeRestarted += Time.deltaTime;
			if (fTimeRestarted > fRestartTime)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
		else
			fTimeRestarted = 0;

		if (Input.GetKeyDown(KeyCode.Space))
			Redo();

		barRestart.maxValue = fRestartTime;
		barRestart.value = fTimeRestarted;
		goRestartText.color = fTimeRestarted == 0 ? Color.clear : Color.red;

		if (Physics2D.OverlapBox(tEndPoint.position, tEndPoint.localScale, 0, lmPlayer)) fTimeOnExit += Time.deltaTime; else fTimeOnExit = 0;
		if (fTimeOnExit > fTimeToExit) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

		cam.position = Vector2.Lerp(cam.position, activePlayer.transform.position * fCamAmp, fCamSmoothing);
	}

	public void Redo()
	{
		pasts.Add(activePlayer.past);
		activePlayer.Redo();
		Destroy(activePlayer.gameObject);
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();

		ghostPlayers.ForEach(x => Destroy(x.gameObject));
		ghostPlayers = new List<PlayerGhost>();
		pasts.ForEach(x => ghostPlayers.Add(Instantiate(pfGhostPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<PlayerGhost>().SetPast(x)));
	}
}