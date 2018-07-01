using UnityEngine;
using Eden.UI.Elements.Building;

namespace Eden.UI.Elements.Building {

	public class Part : MonoBehaviour {

		// ****************** Public *************************

		public delegate void GrabEvent ();
		public GrabEvent OnGrab;

		public delegate void ReleaseEvent ();
		public ReleaseEvent OnRelease;

		public void SetPart ( Eden.Model.Building.Parts.Gun part, int startRow, int startCollumn, Eden.UI.Panels.Building building ) {
				
			_targetPos = building.PositionInWorldSpace( startRow, startCollumn );
			_targetPos.z = GRABBED_HEIGHT;

			transform.position = _targetPos;

			for( int x=0; x<part.Blocks.GetLength(0); x++) {
				
				for( int y=0; y<part.Blocks.GetLength(1); y++) {
					
					var inst = Block.GetInstance( part.Blocks[ x, y ] );
					
					if ( inst != null ) {

						var worldSpacePos = building.PositionInWorldSpace( startRow + x, startCollumn + y );
						var blockPos = new Vector3( worldSpacePos.x, worldSpacePos.y, transform.position.z );

						inst.transform.SetParent( transform, false );
						inst.transform.position = blockPos;
					}
				}
			}

			_stats = part.BuilderStats;
			_blocks = GetComponentsInChildren<Block>();
			_projectors = GetComponentsInChildren<Projector>();
			_lights = GetComponentsInChildren<ActivationLight>( true );
			_building = building;

			foreach ( Reciever r in  GetComponentsInChildren<Reciever>() ) {
				r.OnRecieve += HandleRecieveEvent;
			}

			foreach ( Block b in _blocks ) {
				b.OnAction += HandlePerformActionEvent;
			}
		}
		public void RequestRelease () {

			if ( _isValid ) {
				Release ();
			}
		}
		public void RequestRotate ( float rotation ) {

			Rotate( rotation );
		}
		public void RequestShift ( int row, int collumn ) {

			Shift( row, collumn );
		}
		public void RequestReturnToOrigin () {
		}

		
		// ****************** Private *************************
		
		[SerializeField] private Material _grabbedMaterial;
		[SerializeField] private Material _idleMaterial;
		[SerializeField] private Material _invalidMaterial;

		private const float IDLE_HEIGHT = 0;
		private const float GRABBED_HEIGHT = -1;

		private Eden.Model.Building.Stats.Gun _stats;
		private bool _isGrabbed;
		private Vector3 _grabPoint;
		private Vector3 _grabOffset;


		private Vector3 _targetPos;
		private Quaternion _targetRotation = Quaternion.identity;

		private Eden.UI.Panels.Building _building;
		private Projector[] _projectors;
		private Block[] _blocks;
		private ActivationLight[] _lights;

		private bool _isValid {
			get{ foreach ( Block b in _blocks ) { if ( !b.IsValid ) return false; } 
				return true;
			}
		}
		
		private void Update () {

			transform.position = Vector3.Lerp( transform.position, _targetPos, 0.5f );
			transform.rotation = Quaternion.Slerp( transform.rotation, _targetRotation, 0.5f );
			
			ActivateLights( false );
		}
		private void Grab ( Block block ) {
			
			_isGrabbed = true;
			_grabPoint = block.transform.position;
			_grabOffset = transform.position - _grabPoint;
			_targetPos.z = GRABBED_HEIGHT;

			FireGrabEvent ();
		}
		private void Release () {

			_isGrabbed = false;
			_targetPos.z = IDLE_HEIGHT;

			FireReleaseEvent ();
		}
		private void Shift ( int row, int collumn ) {

			var pos = _building.PositionInWorldSpace( row, collumn );
			_targetPos = new Vector3( pos.x, pos.y, _targetPos.z ) + _grabOffset;
		}
		private void Rotate ( float rotation ) {
			
			_targetRotation = Quaternion.AngleAxis( rotation, Vector3.forward ) * _targetRotation; 
		}
		private void ActivateLights ( bool activation ) {
			
			foreach ( ActivationLight l in _lights ) {

				if  ( activation ) {
					l.Activate();
				} else {
					l.Deactivate();
				}
			}
		}

		
		// ****************** Block Events *************************
		
		private void HandlePerformActionEvent ( Block block ) {

			if ( !_isGrabbed ) {
				Grab( block );
			} else {
				Release();
			}
		}
		

		// ****************** Reciever Events *************************
		
		private void HandleRecieveEvent ( Eden.Model.Building.Stats.Gun stats ) {

			stats.Add( _stats );

			foreach ( Projector p in _projectors ) {
				p.Project( stats );
			}

			ActivateLights( true );
		}


		// ****************** Fire Events *************************

		private void FireGrabEvent () {

			if ( OnGrab != null ) {
				OnGrab ();
			}
		}
		private void FireReleaseEvent () {

			if ( OnRelease != null ) {
				OnRelease ();
			}
		}
	}
}