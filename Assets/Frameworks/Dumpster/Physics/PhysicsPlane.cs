using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Physics {
	
	public class PhysicsPlane : MonoBehaviour {


		// ***************** Public *********************
		
		public Vector3 Forward {
			get{ return -transform.forward; }
		}
		public bool IsColiding {
			get{ return _isColiding; }
		}
		public float ClosestCollision {
			get{ return _closestCollision; }
		}
		public float InsetIntoCollision {
			get{ return _insetIntoCollision; }
		}


		// ***************** Private *********************

		[SerializeField] Core.Bounds _bounds;
		[SerializeField] private Vector2 _rayAmount;
		[SerializeField] private LayerMask _mask;

		private const float ROUNDING = 0.01f;

		[SerializeField] private bool _isColiding;
		[SerializeField] private float _closestCollision;
		[SerializeField] private float _insetIntoCollision;

		private Projection _hit;

		private List<Projection> _projections {
			get {
				
				var rays = new List<Projection>();
				var numXRays = Mathf.RoundToInt( _rayAmount.x );
				var numYRays = Mathf.RoundToInt( _rayAmount.y );

				for ( int x = 0; x< numXRays; x++ ){

					for ( int y = 0; y< numYRays; y++ ){
						
						var xSpacing = _bounds.Size.x / (numXRays);
						var ySpacing = _bounds.Size.y / (numYRays);

						var xPos = transform.right   * (  xSpacing/2f + -_bounds.Width/2  + (x * xSpacing) );
						var yPos = transform.up      * (  ySpacing/2f + -_bounds.Height/2 + (y * ySpacing) );
						var zPos = transform.forward * ( -_bounds.Depth/2 ); 
						var pos = _bounds.Center + (xPos + yPos + zPos );
						var r = new Ray ();

						r.direction = transform.forward;
						r.origin = pos;
						
						rays.Add( new Projection( r, _mask, _bounds.Depth ) );
					}
				}
				return rays;
			}
		}
		public void ForceUpdate () {
			Update ();
		}
		
		private void Update () {

			_isColiding = false;
			_closestCollision = Mathf.Infinity;
			_insetIntoCollision = 0f;

			foreach ( Projection p in _projections ) {
				
				if ( p.IsColiding ) {
					_isColiding = true;
				}

				if ( _closestCollision > p.ProjectedLength ) {
					_closestCollision = p.ProjectedLength;
					_insetIntoCollision = _bounds.Depth - _closestCollision;
					_hit = p;
				}
			}
		}
		public void OnDrawGizmos () {

			if ( !UnityEngine.Application.isPlaying ) {
				Update ();
			}

			// rays 
			Gizmos.color = Color.gray;
			foreach ( Projection p in _projections ) {
				
				Gizmos.color = p.IsColiding ? Color.red : Color.gray;
				Gizmos.DrawRay( p.Ray.origin, p.Ray.direction * p.ProjectedLength );
				Gizmos.DrawWireSphere( p.ProjectedPoint, 0.02f );
			}
		
			// draw contact points
			Matrix4x4 rotationMatrix = Matrix4x4.TRS( _bounds.Center, transform.rotation, transform.lossyScale );
			Gizmos.matrix = rotationMatrix;
			
			Gizmos.color = Color.gray;
			Gizmos.DrawWireCube( Vector3.zero, _bounds.Size );
		}
	}
}