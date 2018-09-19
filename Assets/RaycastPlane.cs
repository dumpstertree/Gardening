using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPlane : MonoBehaviour {

	private Vector2 _size;
	private Vector3 _offset;

	[SerializeField] private Dumpster.Core.Bounds _bounds;
	[SerializeField] private Vector2 _rayAmount;
	[SerializeField] private LayerMask _mask;

	public bool IsColliding () {

		foreach ( Ray r in _projections ) {
			
			RaycastHit hit;
			if ( Physics.Raycast( r, out hit, _mask ) ) {
				Debug.Log( hit.transform.name, hit.transform );
				return true;
			}
		}

		return false;
	}
	private List<Ray> _projections {
		get {
			
			var rays = new List<Ray>();
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
					
					rays.Add( r );
				}
			}

			return rays;
		}
	}
	private void Update () {


	}
	private void OnDrawGizmos () {

		foreach ( Ray r in _projections ) {
			Debug.DrawRay( r.origin, r.direction );
		}
	}
}
