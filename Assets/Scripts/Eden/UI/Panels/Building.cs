using Eden.UI.Elements.Building;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.UI.Panels {
	
	public class Building : Eden.UI.InteractivePanel {

		public enum State {
			Invalid,
			PickingPartFromList,
			PickingPartFromGrid,
			MovingPartOnGrid
		}

		public void SetItemToEdit ( Eden.Model.Item item ) {

			_item = item;
			Load();
		}
		public override void ReciveInput( Input.Package package ){
			
			if ( package.Face.Right_Down ) {
				EdensGarden.Instance.UI.Dismiss( 
					EdensGarden.Constants.NewUILayers.Midground,
					EdensGarden.Constants.UIContexts.Building 
				);
				return;
			}

			_grid.ReciveInput( package );
			_partList.ReciveInput( package );
			_partOverlay.ReciveInput( package );
		}

		public Vector3 PositionInWorldSpace ( int row, int collumn ) {

			var xSpacing = (_camera.orthographicSize*2)/_grid.Collumns;
			var ySpacing = (_camera.orthographicSize*2)/_grid.Rows;
			var startPos = new Vector3( -_camera.orthographicSize, -_camera.orthographicSize );
			var edgePadding = new Vector3( xSpacing/2, ySpacing/2 );

			return _camera.transform.TransformPoint( startPos + edgePadding + new Vector3( xSpacing * collumn, ((_grid.Rows-1) - row ) * ySpacing, _camera.transform.position.z ) );
		}
		public int RowForPosition ( Vector3 position ) {

			var ySpacing = (_camera.orthographicSize*2)/_grid.Rows;
			var startPos =  -_camera.orthographicSize;
			var edgePadding = ySpacing/2f;

			return (_grid.Rows-1) - Mathf.RoundToInt( position.y - (startPos + edgePadding) );
		}
		public int CollumnForPosition ( Vector3 position ) {

			var xSpacing = (_camera.orthographicSize*2)/_grid.Collumns;
			var startPos = -_camera.orthographicSize;
			var edgePadding = xSpacing/2f;

			return Mathf.RoundToInt( position.x - (startPos + edgePadding) );
		}

		public void Save () {

			
			// project through to get all the active projectors
			var activeParts = new List<Eden.Model.Building.Parts.Gun>();
			_rootProjector.Project( activeParts );

			
			// update ranged weapon data
			var rangedWeapondata = new Eden.Model.Building.RangedWeapon();
			rangedWeapondata.ModularParts = activeParts;

			
			// update building data for this ranged weapon
			var partsOnGrid = GetComponentsInChildren<Elements.Building.Part>();
			var buildableData = new List<Eden.Model.Building.Grid.Part>();
			foreach ( Elements.Building.Part p in partsOnGrid ) {
				var d = new Eden.Model.Building.Grid.Part(  
					RowForPosition( p.transform.localPosition ),
					CollumnForPosition( p.transform.localPosition ),
					p.transform.localRotation.eulerAngles,
					p.PartData.PrefabID
				);
				buildableData.Add( d );
			}
			var saveData = new Eden.Model.Building.Grid.SaveData( buildableData.ToArray() );


			// save all data
			EdensGarden.Instance.Data.Save( Data.Controller.Path.RangedWeapon, _item.UniqueID, rangedWeapondata );
			EdensGarden.Instance.Data.Save( Data.Controller.Path.Buildable, _item.UniqueID, saveData );
		}
		public void Load () {

			// look for save data
			var saveData = EdensGarden.Instance.Data.Load<Eden.Model.Building.Grid.SaveData>( Data.Controller.Path.Buildable, _item.UniqueID );
			if( saveData == null ) { saveData = new Eden.Model.Building.Grid.SaveData( new Eden.Model.Building.Grid.Part[]{} ); }
			
			// if save data is found create parts
			foreach( Eden.Model.Building.Grid.Part p in saveData.Parts ) {

				// get template for saved item
				var gridPart = new GameObject( "" ).AddComponent<Part>();
				var partDataTemplate = Eden.Templates.Item.GetTemplate( p.PartPrefabID ) as Eden.Templates.GunBuildableItem;
				if ( partDataTemplate != null ) {

					// create and instance of the template and make sure it is a buildable item
					var partData = partDataTemplate.CreateInstance();
					if ( partData.IsGunBuildable ) {

						// set transform
						gridPart.transform.SetParent( _workspaceContent, false );
						
						// set data 
						gridPart.SetPart( partData.AsGunBuildable.Part, p.Row, p.Collumn, p.Rotation, this );

						// listen for events
						gridPart.OnGrab += () => { HandleGrabEvent( gridPart ); };
						gridPart.OnRelease += () => { HandleReleaseEvent( gridPart ); };
					}
				}
			}
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
		private Eden.Model.Item _item;

		private State _state;
		private Part _grabbedPiece;
		private Eden.Model.Building.Stats.Gun _stats;


		protected override void OnDismiss () {
			Save();
		}

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
			
			var activeParts = new List<Eden.Model.Building.Parts.Gun>();
			_rootProjector.Project( activeParts );
		}
		private void CreatePart ( Eden.Model.GunBuildableItem part ) {

			var piece = new GameObject( "" ).AddComponent<Part>();
			
			piece.transform.SetParent( _workspaceContent, false );
			
			piece.SetPart( part.Part, _spawnRow, _spawnCollumn, Vector3.zero, this );
			piece.OnGrab += () => { HandleGrabEvent( piece ); };
			piece.OnRelease += () => { HandleReleaseEvent( piece ); };

			_grid.SetCollumnAndRow( _spawnRow, _spawnCollumn );

			HandleActionEvent( _spawnRow, _spawnCollumn );
		}


		// **************** States *******************

		private void ChangeState ( State state ) {
			
			if ( _state != state ) {
				
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
		private void EnterPickingFromPartList () {
		
			_grid.Enabled = false;
			_partList.Enabled = true;
			_partOverlay.Enabled = true;
		}
		private void EnterPickingFromGrid () {

			_grid.Enabled = true;
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
		public virtual void ReciveInput( Input.Package package ) {}

		private bool _enabled = true;

		protected virtual void Enable () {}
		protected virtual void Disable () {}
	}
}