﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float _speed;

	private const string RESTRICTED_INPUT_TAG = "InputRestricted";
	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private Player _player;
	private float _horizontal;
	private float _vertical;

	//***************************

	private void Awake () {
		_player = GetComponent<Player>();
	}
	private void Update () {

		_horizontal = Input.GetAxis("Horizontal");
		_vertical   = Input.GetAxis("Vertical");

		NormalView();
	}

	//***************************

	private void NormalView () {

		if ( _player.Animator.GetCurrentAnimatorStateInfo(0).IsTag( RESTRICTED_INPUT_TAG ) ) {
			return;
		}

		Animate();
		MoveCameraTarget();

		if (_horizontal != 0 || _vertical != 0){
			Rotate();
			Move();
		}
	}
	private void Rotate () {

		var rads = Mathf.Atan2( _vertical,_horizontal );
		var degrees = rads * Mathf.Rad2Deg;
		var adjusted = ( degrees - 90 ) * -1;
		var y = Camera.main.transform.eulerAngles.y + adjusted;

		_player.Rigidbody.MoveRotation( Quaternion.Euler( new Vector3( 0, y, 0 ) ) );
	}
	private void Move () {
	
		var h = Mathf.Abs(_horizontal);
		var v = Mathf.Abs(_vertical);
		var speed = ( ( h > v ) ? h : v) * Time.deltaTime * _speed;

		_player.Rigidbody.MovePosition( transform.position + transform.forward * speed);
	}
	private void Animate () {
		
		var absH = Mathf.Abs(_horizontal);
		var absV = Mathf.Abs(_vertical);

		_player.Animator.SetFloat( VERTICAL_ANIMATION_NAME, absH > absV ? _horizontal : _vertical );
	}
	private void MoveCameraTarget () {
		
		var p1 = transform.position;
		var p2 = Camera.main.transform.position;
		var angle = - Mathf.Atan2(p2.x-p1.x, p2.z-p1.z) * Mathf.Rad2Deg;

		var height = 1f;

		_player.CameraTarget.transform.rotation = Quaternion.Euler( new Vector3( _player.CameraTarget.transform.rotation.eulerAngles.x, -angle, _player.CameraTarget.transform.rotation.eulerAngles.z ) );
		_player.CameraTarget.transform.position = transform.position + _player.CameraTarget.forward * 4;
		_player.CameraTarget.transform.position += new Vector3( 0f, height, 0f);
	}
	private void OnDrawGizmos(){
		
		if ( _player && _player.CameraTarget ) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere( _player.CameraTarget.position, 1.0f);
		}

		if ( Camera.main ) {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere( Camera.main.transform.position, 1.0f);
		}
	}
}
