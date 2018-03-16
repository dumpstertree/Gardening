using UnityEngine;

public class PlayerMovement : Brain {
	
	[SerializeField] private Player _player;
	[SerializeField] private PlayerAgressiveSubBrain _agressive;
	[SerializeField] private PlayerPassiveSubBrain _passive;
	
	private CameraType _cameraType;

	public override void Think () {

		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		switch ( _cameraType ) {
			
			case CameraType.Passive:
				_passive.Think( h, v );
				break;
			
			case CameraType.Agressive:
				_agressive.Think( h, v );
				break;
		}
	}

	private void Start () {

		_player.QuickSlot.OnInputChanged += newSlotID => {
			
			var index = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( newSlotID );
			var item = _player.QuickslotInventory.GetInventoryItem( index );

			if ( item.CanShoot ) {
				_cameraType = CameraType.Agressive;
			} else {
				_cameraType = CameraType.Passive;
			}
		};
	}

	private enum CameraType {
		Passive,
		Agressive
	}
}
