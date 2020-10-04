#pragma warning disable 649
using UnityEngine;
using DG.Tweening;
using Ciber_Turtle.UI;

public class PlayerGod : MonoBehaviour
{
	[SerializeField] UIBitText text;

	DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> tweener;

	public void GoToPosAndSay(Vector3 position, float flyTime, string message, float messageTime)
	{
		if (tweener != null) tweener.Kill(false);
		text.text = " ";
		transform.localScale = new Vector3(position.x - transform.position.x < 0 ? -1 : 1, 1, 1);
		tweener = transform.DOMove(position, flyTime, false).OnComplete(() => Say(message, messageTime));
	}

	public void Say(string message, float time)
	{
		text.text = message;
	}
}