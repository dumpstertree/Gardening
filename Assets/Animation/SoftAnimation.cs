using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

public struct SoftAnimationJob : IAnimationJob {

	public TransformStreamHandle joint;
	public Vector3 _velocity;
	public Vector3 _lastPosition;

	void IAnimationJob.ProcessRootMotion( AnimationStream stream ) {
	}

    void IAnimationJob.ProcessAnimation( AnimationStream stream ) {
        
        var targetPosition = joint.GetPosition( stream );
        var lerpedPos = Vector3.Lerp( _lastPosition, targetPosition, 0.1f );
       
        _lastPosition = lerpedPos;
		
		joint.SetPosition( stream, lerpedPos );
    }
}
