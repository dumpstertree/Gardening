using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatVisual : MonoBehaviour {

	[SerializeField] Text _text;

	public void SetStat ( Stat stat ) {

		_text.text = stat.Name + " : " + stat.Level;
	}
}