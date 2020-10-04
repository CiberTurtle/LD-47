#pragma warning disable 649
using UnityEngine;
using Discord;

namespace Ciber_Turtle.Discord_
{
	public class DiscordManager : MonoBehaviour
	{
		Discord.Discord discord;

		public bool enableDiscord;
		[Space]
		public long clientID;
		[Space]
		// [SerializeField] bool forceOnlyOneInstance = true;

		long startTimestamp;

		void Awake()
		{
			startTimestamp = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;

			if (discord != null) discord.Dispose();
			discord = new Discord.Discord(clientID, (System.UInt64)Discord.CreateFlags.Default);
			Application.quitting += () => { Debug.Log("> Discord: Disposing..."); discord.Dispose(); };

			var activityManager = discord.GetActivityManager();

			var activity = new Discord.Activity
			{
				State = $"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}",
				Details = $"I Died {Game.iNumberOfDeaths.ToString()} times!",
				Timestamps = { Start = startTimestamp },
			};

			activityManager.UpdateActivity(activity, (res) =>
			{
				if (res == Discord.Result.Ok)
				{
					Debug.Log($"> Discord: ON");
				}
				else
				{
					Debug.Log("> Discord: OFF");
				}
			});
		}

		void Update()
		{
			discord.RunCallbacks();

			var activity = new Discord.Activity
			{
				State = $"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}",
				Details = $"I Died {Game.iNumberOfDeaths.ToString()} times!",
				Timestamps = { Start = startTimestamp, },
			};

			var activityManager = discord.GetActivityManager();

			activityManager.UpdateActivity(activity, (res) =>
			{

			});
		}
	}
}