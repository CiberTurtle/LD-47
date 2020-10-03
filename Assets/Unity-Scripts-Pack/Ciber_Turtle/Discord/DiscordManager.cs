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
			if (discord != null) discord.Dispose();
			discord = new Discord.Discord(clientID, (System.UInt64)Discord.CreateFlags.Default);
			Application.quitting += () => { Debug.Log("> Discord: Disposing..."); discord.Dispose(); };

			var activityManager = discord.GetActivityManager();

			var activity = new Discord.Activity
			{
				State = "Working on a LD47 Game",
				Details = "Making with Ciber_Turtle and HappyGamer500",
				Timestamps = { Start = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds, },
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
		}
	}
}