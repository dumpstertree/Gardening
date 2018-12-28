using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {
    
    public class Flash : Dumpster.Characteristics.NotificationResponder {

        [SerializeField] private float _fadeInTime  = 0.1f;
        [SerializeField] private float _fadeOutTime = 0.1f;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private MeshRenderer[] _meshRends;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRends;

        private const string STRENGTH_PROPERTY = "_GlowStrength";
        private const string COLOR_PROPERTY = "_GlowColor";

        private Dumpster.Tweening.Tween _tween;
        private float _lastValue;
        
        protected override void Respond () {

            _tween?.Kill();
            _tween = Dumpster.Tweening.Tween.Float( v => { SetValue( v ); }, _lastValue, 1f, _fadeInTime )
                .OnComplete( () => { _tween = Dumpster.Tweening.Tween.Float( v => { SetValue( v ); }, _lastValue, 0f, _fadeInTime ); } );
        }
        private void SetValue ( float v )  {
            
            _lastValue = v;
            
            foreach( MeshRenderer r in _meshRends ) {

                if( r != null ) {
                    
                    foreach( Material m in r.materials ) {
                       
                        if( m.HasProperty( STRENGTH_PROPERTY ) ) {
                            m.SetFloat( STRENGTH_PROPERTY, v );
                        }
                        
                        if( m.HasProperty( COLOR_PROPERTY ) ) {
                            m.SetColor( COLOR_PROPERTY, _flashColor );
                        }
                    }
                }
            }

            foreach( SkinnedMeshRenderer r in _skinnedMeshRends ) {
               foreach( Material m in r.materials ) {
                   if( m.HasProperty( STRENGTH_PROPERTY ) ) {
                       m.SetFloat( STRENGTH_PROPERTY, v );
                   }
                   if( m.HasProperty( COLOR_PROPERTY ) ) {
                       m.SetColor( COLOR_PROPERTY, _flashColor );
                   }
               }
            }
        }
    }
}