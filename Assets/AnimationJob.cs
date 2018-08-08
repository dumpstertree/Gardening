// using Unity.Collections;
// using UnityEngine;
// using UnityEngine.Experimental.Animations;

// public struct LerpJob : IAnimationJob {

	
// 	// ***************** Public ***************
	
// 	public int TargetFrameNumber;
// 	public int LastFrameNumber;
// 	public float Time;


// 	public void UpdateAnimation ( AnimationThing animation ) {

// 		for ( int i=0; i<AnimationThings.Length; i++ ) {
			
// 			if( AnimationThings[i].Name == animation.Name ) {
// 				AnimationThings[ i ] = animation;
// 			}
// 		}
// 	}
// 	public void RegisterAnimation ( string identifier, AnimationThing animation, Transforms transforms ) {

// 		// create animations
// 		if ( !AnimationThings.IsCreated ) {
// 			AnimationThings = new NativeArray<AnimationThing>( 1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
// 		}

// 		var newArray = new NativeArray<AnimationThing>( AnimationThings.Length + 1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
// 		newArray[ newArray.Length - 1 ] = animation;
// 		AnimationThings = newArray;

// 		// create transforms
// 		if ( !_transformHandles.IsCreated ) {
// 			_transformHandles = new NativeArray<Transforms>( 1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
// 		}

// 		var newArray2 = new NativeArray<Transforms>( AnimationThings.Length + 1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
// 		newArray2[ newArray2.Length - 1 ] = transforms;
// 		_transformHandles = newArray2;
// 	}

	
// 	// ***************** Private ***************

// 	private NativeArray<AnimationThing> AnimationThings;
// 	private NativeArray<Transforms> _transformHandles;

// 	private Transforms GetBones ( string identifier ) {

// 		foreach ( Transforms t in _transformHandles ) {
// 			if ( t.Name == identifier ) {
// 				return t;
// 			}
// 		}

// 		return new Transforms( "NULL" );
// 	}


// 	// ***************** IAnimationJob ***************

// 	void IAnimationJob.ProcessRootMotion( AnimationStream stream ) {
// 	}
// 	void IAnimationJob.ProcessAnimation( AnimationStream stream ) {

// 		var lastStream = stream.GetInputStream( LastFrameNumber );
// 		var targetStream = stream.GetInputStream( TargetFrameNumber );

// 		foreach ( AnimationThing animation in AnimationThings ) {
			

// 			if ( animation.IsDirty ) {

// 				var bones = GetBones( animation.Name );
// 				foreach ( TransformStreamHandle handle in bones.TransformHandles ) {
					
// 					var start  = handle.GetLocalRotation( lastStream );
// 					var target = handle.GetLocalRotation( targetStream );
					
// 					handle.SetLocalRotation( stream, Quaternion.Slerp( start, target, Time  ) );
// 				}
// 			}
// 		}
// 	}
// }


// public struct Transforms {
	
// 	public string Name { get; }
// 	public NativeArray<TransformStreamHandle> TransformHandles;

// 	public Transforms ( string name, Animator animator, Transform[] transforms ) {

// 		Name = name;
// 		TransformHandles = new NativeArray<TransformStreamHandle>( 0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

// 		for ( var i = 0; i < transforms.Length; i++ ) {

// 			TransformHandles[ i ] = animator.BindStreamTransform( transforms[ i ] );
// 		}
// 	}
// 	public Transforms ( string name ) {

// 		Name = name;
// 		TransformHandles = new NativeArray<TransformStreamHandle>( 0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
// 	}
// }

// public struct AnimationThing {

// 	public string Name { get; }
// 	public float Weight { get; }
// 	public float Progress { get; }
// 	public bool IsDirty { get; }

// 	public AnimationThing ( string name, float weight, float progress, bool isDirty = true ) {

// 		Name = name;
// 		Weight = weight;
// 		Progress = progress;
// 		IsDirty = isDirty;
// 	}
// }