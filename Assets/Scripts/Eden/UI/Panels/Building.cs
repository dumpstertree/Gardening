using UnityEngine;
using Eden.UI.Elements.Building;

namespace Eden.UI.Panels {
	
	public class Building : MonoBehaviour {

		public enum State {
			Invalid,
			PickingPartFromList,
			PickingPartFromGrid,
			MovingPartOnGrid
		}

		public Vector2 PositionInCameraSpaceForRowAndCollumn ( int row, int collumn ) {

			var xSpacing = (_camera.orthographicSize*2)/_grid.Rows;
			var ySpacing = (_camera.orthographicSize*2)/_grid.Collumns;
			var startPos = new Vector2( -_camera.orthographicSize, -_camera.orthographicSize );
			var edgePadding = new Vector2( xSpacing/2, ySpacing/2 );

			return startPos + edgePadding + new Vector2( xSpacing * collumn, ySpacing * row );
		}
		public Vector3 PositionInWorldSpace ( int row, int collumn ) {

			var xSpacing = (_camera.orthographicSize*2)/_grid.Rows;
			var ySpacing = (_camera.orthographicSize*2)/_grid.Collumns;
			var startPos = new Vector3( -_camera.orthographicSize, -_camera.orthographicSize );
			var edgePadding = new Vector3( xSpacing/2, ySpacing/2 );

			return _camera.transform.TransformPoint( startPos + edgePadding + new Vector3( xSpacing * collumn, ((_grid.Rows-1) - row ) * ySpacing, _camera.transform.position.z ) );
		}
		private Block GetBlockAtCameraPos ( int row, int collumn ) {
			
			RaycastHit hit;
			var worldSpace = PositionInWorldSpace( row, collumn );
			var forward = _camera.transform.forward;

	        if (Physics.Raycast( worldSpace, forward, out hit, Mathf.Infinity )) {

	        	var block = hit.collider.GetComponent<Block>();
	        	if ( block != null ) {
		        	
		        	return block;
		    	}
	        } 

	        return null;
		}


		// **************** Private *******************
		
		[Header( "Subpanels" )]
		[SerializeField] private Eden.UI.Subpanels.Building.Grid _grid;
		[SerializeField] private Eden.UI.Subpanels.Building.PartList _partList;
		[SerializeField] private Eden.UI.Subpanels.Building.PartOverlay _partOverlay;
		
		[Header( "Elements" )]
		[SerializeField] private Eden.UI.Elements.Building.Projector _rootProjector;
		[SerializeField] private Eden.UI.Elements.Building.StatsList _gunStatsBlock;
		[SerializeField] private Camera _camera;
		[SerializeField] private Transform _workspaceContent;

		private int _spawnRow = 5;
		private int _spawnCollumn = 5;

		private State _state;
		private Part _grabbedPiece;
		private Eden.Model.Building.Stats.Gun _stats;


		private void Awake () {

			// grid events
			_grid.OnClicked += HandleActionEvent;
			_grid.OnShift += HandleShiftEvent;
			_grid.OnRotate += HandleRotateEvent;
			_grid.OnTriedToBreakFreeLeft += HandleTriedToBreakFreeLeft;

			// part list events
			_partList.OnPartClicked += HandlePartClickedEvent;
			_partList.OnTriedToBreakFreeRight += HandleTriedToBreakFreeRight;
			_partList.OnShift += HandlePartListShift;
		}
		private void Start () {

			ChangeState( State.PickingPartFromGrid );
		}
		private void LateUpdate () {
			
			_stats = new Eden.Model.Building.Stats.Gun ();
			_rootProjector.Project( _stats );

			_gunStatsBlock.SetBlock( _stats );
		}
		private void CreatePart ( Eden.Model.Building.Parts.Gun part ) {

			var piece = new GameObject( part.Name ).AddComponent<Part>();
			
			piece.transform.SetParent( _workspaceContent, false );
			
			piece.SetPart( part, _spawnRow, _spawnCollumn, this );
			piece.OnGrab += () => { HandleGrabEvent( piece ); };
			piece.OnRelease += () => { HandleReleaseEvent( piece ); };

			_grid.SetCollumnAndRow( _spawnRow, _spawnCollumn );

			HandleActionEvent( _spawnRow, _spawnCollumn );
		}


		// **************** States *******************

		private void ChangeState ( State state ) {
			
			if ( _state != state ) {
			
				switch ( _state ) {
			
					case State.PickingPartFromList:
						ExitPickingFromPartList ();
						break;
			
					case State.PickingPartFromGrid:
						ExitPickingFromGrid ();
						break;
			
					case State.MovingPartOnGrid:
						ExitMovingPartOnGrid ();
						break;
				}
				
				_state = state;
			
				switch ( _state ) {
			
					case State.PickingPartFromList:
						EnterPickingFromPartList ();
						break;
			
					case State.PickingPartFromGrid:
						EnterPickingFromGrid ();
						break;
			
					case State.MovingPartOnGrid:
						EnterMovingPartOnGrid ();
						break;
				}
			}
		}
		private void ExitPickingFromPartList () {
		}
		private void ExitPickingFromGrid () {
		}
		private void ExitMovingPartOnGrid () {
		}
		private void EnterPickingFromPartList () {
		
			_grid.Enabled = false;
			_partList.Enabled = true;
			_partOverlay.Enabled = true;
		}
		private void EnterPickingFromGrid () {

			_partList.Enabled = false;
			_partOverlay.Enabled = false;
		}
		private void EnterMovingPartOnGrid () {
				
			_grid.Enabled = true;
			_partList.Enabled = false;
			_partOverlay.Enabled = false;
		}
		
		
		// **************** Grid Events *******************

		private void HandleShiftEvent ( int row, int collumn ) {

			if ( _state == State.MovingPartOnGrid) {
				_grabbedPiece.RequestShift( row, collumn );
			}
		}
		private void HandleRotateEvent ( float rotation ) {

			if ( _state == State.MovingPartOnGrid) {
				_grabbedPiece.RequestRotate( rotation );
			}
		}
		private void HandleActionEvent ( int row, int collumn ) {
					
			if ( _state == State.MovingPartOnGrid) {
				_grabbedPiece.RequestRelease();
				return;
			}

			var block = GetBlockAtCameraPos( row, collumn );
			if ( block != null ) {
				block.PerformAction();
			}
		}
		private void HandleTriedToBreakFreeLeft () {

			if ( _state == State.PickingPartFromGrid ) {
				
				ChangeState( State.PickingPartFromList );
			}
		}

		
		// **************** Piece Events *******************

		private void HandleGrabEvent ( Part piece ) {

			// add refrence
			_grabbedPiece = piece;

			// change state
			ChangeState( State.MovingPartOnGrid );
		}
		private void HandleReleaseEvent ( Part piece ) {

			// remove refrence
			if( _grabbedPiece == piece ) { 
				_grabbedPiece = null; 
			}

			// change state
			ChangeState( State.PickingPartFromGrid );
		}


		// **************** Part List Events *******************

		private void HandlePartListShift ( int newIndex ) {

			_partOverlay.SetPart( _partList.PeakAtPart( newIndex ) );
		}
		private void HandlePartClickedEvent ( int atIndex )  {

			var part = _partList.TakePart( atIndex );
			CreatePart( part );
		}
		private void HandleTriedToBreakFreeRight() {

			ChangeState( State.PickingPartFromGrid );
		}
	}
}

namespace Eden.UI.Subpanels.Building {

	public class BuildSubpanel : MonoBehaviour {

		public bool Enabled {
			get {
				return _enabled;
			}
			set { 
				if ( _enabled != value ) {
					_enabled = value;
					if ( value ) { Enable();
					} else { Disable(); }
				} 
			}
		}

		private bool _enabled = true;

		protected virtual void Enable () {}
		protected virtual void Disable () {}
	}
}