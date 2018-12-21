using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {
    
    public class DisableActor : Dumpster.Characteristics.NotificationResponder {
        
        protected override void Respond() {

            _actor.Enabled = false;
        }
    }
}
