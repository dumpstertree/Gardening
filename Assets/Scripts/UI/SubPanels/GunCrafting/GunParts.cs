using UnityEngine;

namespace UI.Subpanels.GunCrafting {
	
	public class GunParts : MonoBehaviour {

		[SerializeField] private GunPartsRow _gunPartRowPrefab;
		[SerializeField] private Transform _content;

		public delegate void DragEvent( Model.Gun.Part part );
		public DragEvent OnDragBegin;
		public DragEvent OnDragEnd;

		private Player _player {
			get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Player>(); }		
		}

		// *************************
		
		public void Reload () {
			
			Clear();

			foreach  ( Model.Gun.Part part in _player.GunParts.Parts ) {
				
				AddRow( part );
			}
		}
		
		// *************************

		private void AddRow ( Model.Gun.Part part ) {
			
			var row = Instantiate( _gunPartRowPrefab );
			row.transform.SetParent( _content, false );

			row.SetPart( part );

			row.OnDragBegin += HandleOnDragBegin;
			row.OnDragEnd += HandleOnDragEnd;
		}
		private void Clear () {
			
			foreach( Transform t in _content ) {
				if ( t == transform ){ continue; }
				Destroy( t.gameObject );
			}
		}

		// *************************

		private void HandleOnDragBegin ( Model.Gun.Part part ) {
	    	
	    	if ( OnDragBegin != null ) {
	    		OnDragBegin( part );
	    	}
	    }
	    private void HandleOnDragEnd ( Model.Gun.Part part ) {
	    	
	    	if ( OnDragEnd != null ) {
	    		OnDragEnd( part );
	    	}
	    }
	}
}
