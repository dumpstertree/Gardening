using System.Collections.Generic;
using UnityEngine;

namespace Model {
	
	[System.Serializable]
	public class PartInventory {

		public PartInventory ( int numOfItems )	 {
		
			_numOfItems = numOfItems;
			_parts = new List<Gun.Part>();
		}

		// ************ PUBLIC *******************

		public delegate void PartListChangedEvent();
		public PartListChangedEvent OnPartListChanged;

		public List<Gun.Part> Parts {
			get { return _parts; }
		}
		public bool AddPart ( Gun.Part part ) {

			if ( _parts.Count < _numOfItems ) {
				_parts.Add( part );
				HandleOnItemsChanged();
				return true;
			}

			return false;
		}
		public void RemovePart ( Gun.Part part ) {

			if ( _parts.Contains( part ) ) {
				_parts.Remove( part );
				HandleOnItemsChanged();
			}
		}

		// ************ PRIVATE ******************

		private int _numOfItems;
		[SerializeField] private List<Gun.Part> _parts;

		private void HandleOnItemsChanged () {

			if ( OnPartListChanged != null ) {
				OnPartListChanged();
			}
		}
	}
}