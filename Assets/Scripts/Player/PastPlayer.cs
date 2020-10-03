#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Past
{
	public List<Vector2> v2History = new List<Vector2>();
	public bool bManualDeath = true;
}