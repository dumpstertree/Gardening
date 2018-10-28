using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Model.Constants {

	[System.Serializable]
	public class InputLayers {

		public string Player  {
			get{ return _player; }
		}
		public string ForegroundUI {
			get{ return _foregroundUI; }
		}
		public string MidgroundUI  {
			get{ return _midgroundUI; }
		}
		public string BackgroundUI  {
			get{ return _backgroundUI; }
		}

		[SerializeField] private string _player = "PLAYER";
		[SerializeField] private string _foregroundUI = "FOREGROUND";
		[SerializeField] private string _midgroundUI = "MIDGROUND";
		[SerializeField] private string _backgroundUI = "BACKGROUND";
	}
}