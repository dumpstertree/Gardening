using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Model.Constants {

	[System.Serializable]
	public class Paths {

		public string PlayerPath {
			get { return _playerPath; }
		}
		public string BuildablePath {
			get { return _buildablePath; }
		}
		public string RangedWeaponPath {
			get { return _rangedWeaponPath; }
		}	

		[SerializeField] private string _playerPath = "/player/";
		[SerializeField] private string _buildablePath = "/buildable/";
		[SerializeField] private string _rangedWeaponPath = "/ranged/";
	}
}