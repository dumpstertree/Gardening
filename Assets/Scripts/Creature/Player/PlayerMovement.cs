using UnityEngine;

public class PlayerMovement : Brain, IInputReciever {
	
	void IInputReciever.OnConfirmDown () {}
	void IInputReciever.OnConfirmUp () {}
	void IInputReciever.OnCancelDown () {}
	void IInputReciever.OnCancelUp () {}
	void IInputReciever.OnStartDown (){}
	void IInputReciever.OnStartUp (){}
	void IInputReciever.HorizontalChanged ( float horizontal ) {
		
		_horizontal = horizontal;
	}
	void IInputReciever.VerticalChanged ( float vertical ) {

		_vertical = vertical;
	}

	[SerializeField] private Player _player;
	[SerializeField] private PlayerAgressiveSubBrain _agressive;
	[SerializeField] private PlayerPassiveSubBrain _passive;
	
	private CameraType _cameraType;
	private float _horizontal;
	private float _vertical;
	
	public override void Think () {

		switch ( _cameraType ) {
			
			case CameraType.Passive:
				_passive.Think( _horizontal, _vertical );
				break;
			
			case CameraType.Agressive:
				_agressive.Think( _horizontal, _vertical );
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
