using UnityEngine;

namespace Eden.Life.Chips.Logic {

	public class Player : Dumpster.Core.Life.LogicChip {

		[SerializeField] private Eden.Life.BlackBoxes.Player _player;
		[SerializeField] private PlayerAgressiveSubBrain _agressive;
		[SerializeField] private PlayerPassiveSubBrain _passive;
		[SerializeField] private LayerMask _mask;
		[SerializeField] private Dumpster.Core.BuiltInModules.ShoulderCameraController _shoulderCamera;

		private CameraType _cameraType;
		private float _horizontal;
		private float _vertical;
		private float _cameraHorizontal;
		private float _cameraVertical;
		private bool _jump;
		private bool _dash;

		private const float RAYCAST_INSET = 0.01f;
		private const float RAYCAST_DISTANCE = 0.1f;

		[SerializeField] private float _jumpVelocity = 10f;

		private void Update () {

			if ( _player.Physics.State.DownIsColliding && _jump ) {
				_player.Physics.AddVelocity( new Vector3( 0, _jumpVelocity, 0) );
			}
		}

		private void Awake () {
			
			_player.OnRecieveInput += RecieveInput;

			_player.QuickslotChip.OnInputChanged += QuickslotChanged;
		}
		private void RecieveInput ( Eden.Input.Package package ) {
			
			_horizontal = package.LeftAnalog.Horizontal;
			_vertical = package.LeftAnalog.Vertical;

			_cameraHorizontal = package.RightAnalog.Horizontal;
			_cameraVertical = package.RightAnalog.Vertical;

			_jump = package.Face.Down_Down;
			_dash = package.Face.Left_Down;

			if ( package.BackRight.Bumper ) {
				_player.Interactor.Use();
			}
		}
		private void QuickslotChanged ( int index ) {

			var item = _player.EquipedItems.GetInventoryItem( index );			
			
			if ( item != null && item.CanShoot ) {
				_agressive.WillBecomeActive();
				_cameraType = CameraType.Agressive;
				_passive.Think( 0, 0, 0, 0 );
			} else {
				_cameraType = CameraType.Passive;
				_agressive.Think( 0, 0, 0, 0 , false);
			}
		}
		

		private enum CameraType {
			Passive,
			Agressive
		}	
		public override void Analayze () {
			
			switch ( _cameraType ) {
				
				case CameraType.Passive:
					_passive.Think( _horizontal, _vertical, _cameraHorizontal, _cameraVertical );
					break;
				
				case CameraType.Agressive:
					_agressive.Think( _horizontal, _vertical, _cameraHorizontal, _cameraVertical, _dash );
					break;
			}
		}
	}
}