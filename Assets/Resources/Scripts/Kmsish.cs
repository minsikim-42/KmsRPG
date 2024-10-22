using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Kms
{
	public class Kmsish// : MonoBehaviour
	{
		public int lv;
		public int num;
		virtual public void initRune() { }
		virtual public void SetText() { }
		virtual public void upgradeSetText() { }
		virtual public void setBlind() { }
		virtual public void upgrade() { }
		virtual public PlayerStateDTO getState() { return new PlayerStateDTO(); }
		// public int getNum() { return num; }
		public int getLv()
		{
			return lv;
		}
	}
}