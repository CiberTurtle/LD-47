#pragma warning disable 649
using UnityEngine;

public class BeatGame : MonoBehaviour
{
	void Awake()
	{
		PlayerPrefs.SetString("game.beat", "true");
	}
}