#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Past
{
	public List<Vector2> v2History;
	public bool bManualDeath;
	public int iCosmeticIndex;

	public Past(int iCosmeticIndex)
	{
		v2History = new List<Vector2>();
		bManualDeath = true;
		this.iCosmeticIndex = iCosmeticIndex;
	}
}