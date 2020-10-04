#pragma warning disable 649
using UnityEngine;

public class GodTrigger : MonoBehaviour
{
	[SerializeField] PlayerGod triggers;
	[SerializeField] Transform position;
	[SerializeField] float flyTime;
	[SerializeField, TextArea] string text;

	bool hasBeenTriggered;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!hasBeenTriggered && other.CompareTag("Player"))
		{
			hasBeenTriggered = true;
			triggers.GoToPosAndSay(position.position, flyTime, text, 1);
		}
	}
}