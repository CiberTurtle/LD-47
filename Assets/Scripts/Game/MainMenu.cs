#pragma warning disable 649
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] int defaultQuality = 1;
	[SerializeField] Toggle togQuality;

	void Awake()
	{
		if (PlayerPrefs.GetInt("quality.level", defaultQuality) != QualitySettings.GetQualityLevel())
		{
			QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality.level", defaultQuality));
		}

		togQuality.isOn = PlayerPrefs.GetInt("quality.level", defaultQuality) == 0 ? false : true;
	}

	public void Play()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void Open(string url)
	{
		Application.OpenURL(url);
	}

	public void Pause(bool state)
	{
		FindObjectOfType<Game>().IS_PAUSED = state;
	}

	public void SetQuility(bool high)
	{
		QualitySettings.SetQualityLevel(high ? 1 : 0, true);
		PlayerPrefs.SetInt("quality.level", high ? 1 : 0);
	}
}