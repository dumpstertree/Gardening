using UnityEngine;

public class PlayerPassiveSubBrain : MonoBehaviour {

	public void Think ( float horizontal, float vertical ){

		print( horizontal + " : " + vertical );
		_horizontal = horizontal;
		_vertical   = vertical;
		
		if ( _player.Animator.GetCurrentAnimatorStateInfo(0).IsTag( RESTRICTED_INPUT_TAG ) ) {
			return;
		}

		Animate();

		if (_horizontal != 0 || _vertical != 0){
			Rotate();
			Move();
		}
	}


	//***************************

	[SerializeField] private Player _player;
	[SerializeField] private float _speed;

	private const string RESTRICTED_INPUT_TAG = "InputRestricted";
	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private float _horizontal;
	private float _vertical;

	//***************************

	private void Rotate () {

		var rads = Mathf.Atan2( _vertical,_horizontal );
		var degrees = rads * Mathf.Rad2Deg;
		var adjusted = ( degrees - 90 ) * -1;
		var y = Camera.main.transform.eulerAngles.y + adjusted;

		_player.transform.rotation = Quaternion.Euler( new Vector3( 0, y, 0 ) );
	}
	private void Move () {
	
		var h = Mathf.Abs(_horizontal);
		var v = Mathf.Abs(_vertical);
		var speed = ( ( h > v ) ? h : v) * Time.deltaTime * _speed;
		
		_player.Physics.MovePosition ( transform.forward * speed ) ;
	}
	private void Animate () {
		
		var absH = Mathf.Abs(_horizontal);
		var absV = Mathf.Abs(_vertical);
		
		_player.Animator.SetFloat( VERTICAL_ANIMATION_NAME, absH > absV ? _horizontal : _vertical );
	}
}
