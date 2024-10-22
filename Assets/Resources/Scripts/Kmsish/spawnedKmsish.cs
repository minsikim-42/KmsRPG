using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnedKmsish : MonoBehaviour
{
	public int idx = -1;
	// Start is called before the first frame update
	public void setIdx(int _idx)
	{
		idx = _idx;
	}
	public int getIdx()
	{
		return idx;
	}
}
