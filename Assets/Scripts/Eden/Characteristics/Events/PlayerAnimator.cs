using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster;

public class PlayerAnimator : MonoBehaviour {

    [Header( "Reference" )]
    [SerializeField] private Dumpster.Controllers.ThirdPersonCharacterController _controller;
    [SerializeField] private CharacterController _characterController;
	[SerializeField] private Animator2 _animator;

    [Header( "Strides" )]
    [SerializeField] private float _stridesPerMeterWalk = 0.5f;
    [SerializeField] private float _stridesPerMeterRun = 0.2f;

    private AnimationDampener _jumpDampener;	
    private AnimationDampener _fallDampener;
    private AnimationDampener _landDampener;
    private AnimationDampener _aimDampener;
    private KindaBlendTree _sprintingBlendTree;
    private KindaBlendTree _runningBlendTree;
    private KindaBlendTree _dashingBlendTree;
    private KindaBlendTree _walkingBlendTree;
    private Vector3 _lastFramePos;
    private Vector3 _distanceCovered;    
    private float _stride;


    private Vector3 _movementDelta {
        get { return _lastFramePos - transform.position; }
    }
    private float _stridesPerMeter {
		get { return _stridesPerMeterRun;
            return Mathf.Lerp( _stridesPerMeterWalk, _stridesPerMeterRun, Vector3.Distance( Vector3.zero, _characterController.velocity)/_controller.TerminalVelocity ); }
    }


	private void Start () { 

        _aimDampener  = new AnimationDampener( _animator, "Aim" );
        _jumpDampener = new AnimationDampener( _animator, "Jump" );
        _fallDampener = new AnimationDampener( _animator, "Fall" );
        _landDampener = new AnimationDampener( _animator, "Land" );

        _runningBlendTree = new KindaBlendTree( 
            animator : _animator,
            posX : "Run_Right",
            negX : "Run_Left",
            posY : "Run_Forward",
            negY : "Run_Back"
        );

        _walkingBlendTree = new KindaBlendTree( 
            animator : _animator,
            posX : "Walk_Right",
            negX : "Walk_Left",
            posY : "Walk_Forward",
            negY : "Walk_Back"
        );

        _dashingBlendTree = new KindaBlendTree( 
            animator : _animator,
            posX : "Dash_Right",
            negX : "Dash_Left",
            posY : "Dash_Forward",
            negY : "Dash_Back"
        );

        _animator.SetWeight( "Idle", 1f );
    }
    private void LateUpdate () {
        
        UpdateDistanceCovered ();
        UpdateStride ();

		Animate ();

        _lastFramePos = transform.position;
	}
    
    private void Animate () {

        // Strafing
        if ( !_wasStrafing && _controller.IsStrafing ) {
            OnBeginStrafing ();
        } else if ( _wasStrafing && !_controller.IsStrafing ) {
            OnEndStrafing ();
        } else if ( _controller.IsStrafing) { 
            OnStrafing (); 
        }

        _wasStrafing = _controller.IsStrafing;
        

        // On Ground
        if ( !_wasOnGround && _controller.IsOnGround ) {
            OnBeginOnGround ();
        } else if ( _wasOnGround && !_controller.IsOnGround ) {
            OnEndOnGround ();
        } else if ( _controller.IsOnGround) { 
            OnGround (); 
        }

        _wasOnGround =  _controller.IsOnGround;

        
        // Jumping
        if ( !_wasJumping && _controller.IsJumpingUp ) {
            OnBeginJump ();
        } else if ( _wasJumping && !_controller.IsJumpingUp ) {
            OnEndJump ();
        } else if ( _controller.IsJumpingUp ) { 
            OnJump (); 
        }

        _wasJumping =  _controller.IsJumpingUp;


        // Falling
        if ( !_wasFalling && _controller.IsFallingDown ) {
            OnBeginFall ();
        } else if ( _wasFalling && !_controller.IsFallingDown ) {
            OnEndFall ();
        } else if ( _controller.IsFallingDown ) { 
            OnFall (); 
        }

        _wasFalling =  _controller.IsFallingDown;

        // Idling
        if ( !_wasIdling && _controller.IsIdling ) {
            OnBeginIdle ();
        } else if ( _wasIdling && ! _controller.IsIdling ) {
            OnEndIdle ();
        } else if (  _controller.IsIdling ) { 
            OnIdle (); 
        }

        _wasIdling = _controller.IsIdling;

        
        // Walk
        if ( !_wasWalking && _controller.IsWalking ) {
            OnBeginWalk ();
        } else if ( _wasWalking && !_controller.IsWalking ) {
            OnEndWalk ();
        } else if ( _controller.IsWalking ) { 
            OnWalk (); 
        }

        _wasWalking = _controller.IsWalking;


        // Run
        if ( !_wasRunning && _controller.IsRunning ) {
            OnBeginRun ();
        } else if ( _wasRunning && !_controller.IsRunning ) {
            OnEndRun ();
        } else if ( _controller.IsRunning ) { 
            OnRun (); 
        }

        _wasRunning =  _controller.IsRunning;
    

        // Dash
        if ( !_wasDashing && _controller.IsDashing ) {
            OnBeginDash ();
        } else if ( _wasDashing && !_controller.IsDashing ) {
            OnEndDash ();
        } else if ( _controller.IsDashing ) { 
            OnDash (); 
        }

        _wasDashing = _controller.IsDashing;
    }
	private void UpdateDistanceCovered () {
		
		_distanceCovered += (transform.position - _lastFramePos);
	}
	private void UpdateStride () {
		
        _stride += Vector3.Distance( Vector3.zero, (transform.position - _lastFramePos) ) * _stridesPerMeter;
	}

   
    // Strafing
    private bool _wasStrafing;

    private void OnBeginStrafing () {

        _aimDampener.SetProgress( 1f );
        _aimDampener.SetWeight( 1f, 0.2f );
    }
    private void OnStrafing () {
    }
    private void OnEndStrafing () {

        _aimDampener.SetWeight( 0f, 0.2f );
    }


    // Falling
    private bool _wasFalling;

    private void OnBeginFall () {
        
        _fallDampener.SetProgress( 0f );
        _fallDampener.SetWeight( 1f, 0.2f );
    }
    private void OnFall () {

        var prog = Mathf.Clamp01( _controller.InputVelocity.y / -_controller.JumpPower );
        
        _fallDampener.SetProgress( prog );
    }
    private void OnEndFall () {
    }


    // Jumping
    private bool _wasJumping;

    private void OnBeginJump () {

        _jumpDampener.SetProgress( 0f );
        _jumpDampener.SetWeight( 1f );
    }
    private void OnJump () {

        var prog = Mathf.Clamp01( 1f - _controller.InputVelocity.y / _controller.JumpPower );

        _jumpDampener.SetProgress( prog );
    }
    private void OnEndJump () {
    }


    // Grounded
    private bool _wasOnGround;

    private void OnBeginOnGround () {
        _fallDampener.SetWeight( 0f, 0.2f );
        _jumpDampener.SetWeight( 0f, 0.2f );
        
        _landDampener.SetWeight( 1f );
        _landDampener.SetProgress( 0, 0.3f );
        _landDampener.SetWeight( 0, 0.3f );
    }
    private void OnGround () {
    }
    private void OnEndOnGround () {
    }

    
    // Idle
    private bool _wasIdling;

    private void OnBeginIdle () {
    }
    private void OnIdle () {

        var prog = Mathf.Repeat( Time.time, 1.0f );

        _animator.SetProgress( 
            "Idle", 
            prog
        );
    }
    private void OnEndIdle () {
    }

    
    // Walking
    private bool _wasWalking;

    private void OnBeginWalk () {

        _walkingBlendTree.SetWeight( 1f, 0.2f );
    }
    private void OnWalk () {

        var prog = Mathf.Repeat( _stride, 1.0f );

        _walkingBlendTree.SetProgress( prog );

        _walkingBlendTree.SetBlendPoint( 
            _controller.LocalVelocity.x / _controller.WalkSpeed,
            _controller.LocalVelocity.z / _controller.WalkSpeed
        );
    }
    private void OnEndWalk () {

        _walkingBlendTree.SetWeight( 0f, 0.2f );
    }
    
    
    // Running
    private bool _wasRunning;

    private void OnBeginRun () {

        _runningBlendTree.SetWeight( 1f, 0.2f );
    }
    private void OnRun () {

        var prog = Mathf.Repeat( _stride, 1.0f );
        
        _runningBlendTree.SetProgress( prog );

        _runningBlendTree.SetBlendPoint( 
            _controller.LocalVelocity.x / _controller.RunSpeed,
            _controller.LocalVelocity.z / _controller.RunSpeed
        );
    }
    private void OnEndRun () {

        _runningBlendTree.SetWeight( 0f, 0.2f );
    }

    
    // Sprinting
    private bool _wasSprinting;

    private void OnBeginSprint () {

        _sprintingBlendTree.SetWeight( 1f, 0.2f );
    }
    private void OnSprint () {

        var prog = Mathf.Repeat( _stride, 1.0f );
        
        _sprintingBlendTree.SetProgress( prog );

        _sprintingBlendTree.SetBlendPoint( 
            _controller.LocalVelocity.x / _controller.RunSpeed,
            _controller.LocalVelocity.z / _controller.RunSpeed
        );
    }
    private void OnEndSprint () {

        _sprintingBlendTree.SetWeight( 0f, 0.2f );
    }


    // Dashing

    [Header( "Dashing" )]
    [SerializeField] private Color _dashGlowColor;
    [SerializeField] private SkinnedMeshRenderer[] _dashRenderers;
    [SerializeField] private TrailRenderer[] _dashTrailRenderers;
    [SerializeField] private float _dashFadeInLength = 0.2f;
    [SerializeField] private float _dashFadeOutLength = 0.2f;

    private bool _wasDashing;
    private float _dashStartTime;

    private void OnBeginDash () {

        _dashingBlendTree.SetWeight( 1f, 0.1f );
        _dashStartTime = Time.time;
        
        foreach( SkinnedMeshRenderer r in _dashRenderers ) {
            if ( r.material.HasProperty( "_GlowStrength" ) ) {
                Dumpster.Tweening.Tween.Float( v => { r.material.SetFloat( "_GlowStrength", v ); }, r.material.GetFloat( "_GlowStrength" ), 1f, _dashFadeInLength );
            }
             if ( r.material.HasProperty( "_GlowColor" ) ) {
                r.material.SetColor( "_GlowColor", _dashGlowColor );
            }
        }
        foreach( TrailRenderer r in _dashTrailRenderers ) {
            Dumpster.Tweening.Tween.Float( v => { r.widthMultiplier = v; }, r.widthMultiplier, 1f, _dashFadeInLength );
        }
      
    }
    private void OnDash () {

        _dashingBlendTree.SetProgress( 0.1f );
        _dashingBlendTree.SetBlendPoint( 0f, 1f );

        // _dashingBlendTree.SetProgress( Mathf.Clamp01(Time.time - _dashStartTime)/1.0f );
        // _dashingBlendTree.SetBlendPoint(
        //     _controller.LocalVelocity.x / _controller.DashSpeed,
        //     _controller.LocalVelocity.z / _controller.DashSpeed 
        // );
    }
    private void OnEndDash () {

        _dashingBlendTree.SetWeight( 0f, 0.3f );

        foreach( SkinnedMeshRenderer r in _dashRenderers ) {
             if ( r.material.HasProperty( "_GlowStrength" ) ) {
                Dumpster.Tweening.Tween.Float( v => { r.material.SetFloat( "_GlowStrength", v ); }, r.material.GetFloat( "_GlowStrength" ), 0f, _dashFadeOutLength );
             }
        }
        foreach( TrailRenderer r in _dashTrailRenderers ) {
            Dumpster.Tweening.Tween.Float( v => { r.widthMultiplier = v; }, r.widthMultiplier, 0f, _dashFadeOutLength );
        }
    }
}
