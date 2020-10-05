#pragma warning disable 649
using UnityEngine;

public class GodTrigger : MonoBehaviour
{
	[SerializeField] PlayerGod triggers;
	[SerializeField] Transform position;
	[SerializeField] float flyTime;
	[SerializeField, TextArea] string text;
	[SerializeField] int minGhostAmount = 0;

	bool hasBeenTriggered;

	void OnTriggerStay2D(Collider2D other)
	{
		if (!hasBeenTriggered && other.CompareTag("Player") && FindObjectOfType<Game>().ghostPlayers.Count >= minGhostAmount)
		{
			hasBeenTriggered = true;
			triggers.GoToPosAndSay(position.position, flyTime, text, 1);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.1f);

		Gizmos.DrawWireCube(transform.position, transform.localScale);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1, 0, 0, 0.5f);

		Gizmos.DrawWireCube(transform.position, transform.localScale);

		Gizmos.DrawLine(transform.position, position.position);
	}
}