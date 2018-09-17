using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Dumpster.Core;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Experimental.Animations;
using UnityEngine.Experimental.Playables;


public class MockCharacterController : MonoBehaviour {

	[SerializeField] private Animator2 _animator;

	[Range(0,1)] [SerializeField] private float _aimWeight;
	[Range(0,1)] [SerializeField] private float _aimProgress;
	

	[Range(0,1)] [SerializeField] private float _jumpWeight;
	[Range(0,1)] [SerializeField] private float _jumpProgress;

	[Range(-1,1)] [SerializeField] private float _moveX;
	[Range(-1,1)] [SerializeField] private float _moveY;

	private void Update () {

		_animator.SetGrowthPoint( "Move", _moveX, _moveY );
		_animator.SetProgress( "Move", Mathf.Repeat( Time.time, 1.0f) );
	}
}