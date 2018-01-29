using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetingUI : MonoBehaviour {

	[SerializeField] private Image _crossHair;

	private void Update () {

		var pos = Camera.main.WorldToScreenPoint( Game.Area.LoadedPlayer.CameraFocus.position );
		_crossHair.transform.position = pos;
	}
}
