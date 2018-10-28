using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Model.Constants {
	
	[System.Serializable]
	public class UIContexts {
		
		public string None {
			get { return _none; }
		}
		public string Player {
			get { return _player; }
		}
		public string Dialog {
			get { return _dialog; }
		}
		public string Inventory {
			get { return _inventory; }
		}
		public string Building {
			get { return _building; }
		}

		[SerializeField] private string _none = "NONE";
		[SerializeField] private string _player = "PLAYER";
		[SerializeField] private string _dialog = "DIALOG";
		[SerializeField] private string _inventory = "INVENTORY";
		[SerializeField] private string _building = "BUILDING";
	}
}
