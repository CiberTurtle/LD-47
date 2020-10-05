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
	[Space]
	public Sprite[] spCosmetics;
	[Space]
	[SerializeField] GameObject pfPlayer;
	[SerializeField] GameObject pfGhostPlayer;
	[SerializeField] Sprite spClosedDoor;
	[SerializeField] Sprite spOpenDoor;
	[Space]
	[SerializeField] Transform tSpawnPoint;
	[SerializeField] Transform tEndPoint;
	[SerializeField] float fTimeToExit;
	[SerializeField] LayerMask lmPlayer;
	[SerializeField] GameObject goDevUI;
	[SerializeField] UIBitText textTitle;
	[Space]
	[SerializeField] GameObject goPauseMenu;
	[Space]
	[SerializeField] float fRestartTime;
	[SerializeField] UIProgressBar barRestart;
	[SerializeField] UIBitText textRestartText;
	[SerializeField] GameObject goShadow;
	[Space]
	[SerializeField] float fCamSmoothing;
	[SerializeField] float fCamAmp;
	[SerializeField] Transform cam;

	[HideInInspector] public static int iNumberOfDeaths;

	[SerializeField] public bool IS_PAUSED = false;
	float fTimeOnExit;
	float fTimeRestarted;

	void Awake()
	{
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();
		if (PlayerPrefs.GetString("game.beat", "false") == "true" && SceneManager.GetActiveScene().name != "The End")
			activePlayer.past.iCosmeticIndex = 0;
		else
			activePlayer.past.iCosmeticIndex = Mathf.RoundToInt(Random.Range(1, spCosmetics.Length));
	}

	void Start()
	{
		textTitle.text = SceneManager.GetActiveScene().name;
	}

	void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) IS_PAUSED = !IS_PAUSED;
		if (Input.GetKeyDown(KeyCode.F3)) goDevUI.SetActive(!goDevUI.activeInHierarchy);
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

		if (Input.GetKeyDown(KeyCode.C) && Random.value > 0.95f)
		{
			PlayerPrefs.DeleteKey("game.beat");
			Debug.Log("Clear");
		}

		goPauseMenu.SetActive(IS_PAUSED);
		Time.timeScale = IS_PAUSED ? 0 : 1;
		Ciber_Turtle.Input.BasicInput.enabled = !IS_PAUSED;

		barRestart.maxValue = fRestartTime;
		barRestart.value = fTimeRestarted;
		textRestartText.color = fTimeRestarted == 0 ? Color.clear : Color.red;
		goShadow.SetActive(fTimeRestarted != 0);

		if (activePlayer.GetComponent<PlayerMovement>().bGrounded && Physics2D.OverlapBox(tEndPoint.position, tEndPoint.localScale, 0, lmPlayer))
		{
			fTimeOnExit += Time.deltaTime;
			tEndPoint.GetComponent<SpriteRenderer>().sprite = spOpenDoor;
		}
		else
		{
			fTimeOnExit = 0;
			tEndPoint.GetComponent<SpriteRenderer>().sprite = spClosedDoor;
		}

		if (fTimeOnExit > fTimeToExit) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

		cam.position = Vector2.Lerp(cam.position, activePlayer.transform.position * fCamAmp, fCamSmoothing);
	}

	public void Redo()
	{
		iNumberOfDeaths++;

		pasts.Add(activePlayer.past);
		activePlayer.Redo();
		Destroy(activePlayer.gameObject);
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();
		if (PlayerPrefs.GetString("game.beat", "false") == "true" && SceneManager.GetActiveScene().name != "The End")
			activePlayer.past.iCosmeticIndex = 0;
		else
			activePlayer.past.iCosmeticIndex = Mathf.RoundToInt(Random.Range(1, spCosmetics.Length));

		ghostPlayers.ForEach(x => Destroy(x.gameObject));
		ghostPlayers = new List<PlayerGhost>();
		pasts.ForEach(x => ghostPlayers.Add(Instantiate(pfGhostPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<PlayerGhost>().SetPast(x)));
	}
}