using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;

namespace Eden.Characteristics {
    
    public class Dissolve : Dumpster.Characteristics.NotificationResponder {
        
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;
        [SerializeField] private float _time = 0.25f;

        private const string PROPERTY = "_Dissolve";
        private const string FINISHED = "Dissolve.Finished";

        public override List<string> GetNotifications () {
            
            return new List<string>() { 
                FINISHED 
            };
        }
        protected override void Respond () {

            foreach ( MeshRenderer r in _meshRenderers ) {
                
                foreach ( Material m in r.materials ) {
                
                    DissolveMAT( m );
                }
            }

             foreach ( SkinnedMeshRenderer r in _skinnedMeshRenderers ) {
                
                foreach ( Material m in r.materials ) {
                
                    DissolveMAT( m );
                }
            }

           Game.GetModule<Async>().WaitForSeconds( _time, () => _actor.PostNotification( FINISHED ) );
        }

        private void DissolveMAT ( Material m ) {
         
            if (  m.HasProperty( PROPERTY ) ) {
                
                Dumpster.Tweening.Tween.Float( 
                    v => { m.SetFloat( PROPERTY, v ); },  
                    m.GetFloat( PROPERTY ), 
                    1f, 
                    _time 
                );
            }
        }
    }
}
