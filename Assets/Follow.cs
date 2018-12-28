using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    
    [SerializeField] private Transform _objectToFollow;
    
    void Update () {

        transform.position = _objectToFollow.transform.position;
        transform.rotation = _objectToFollow.transform.rotation;
    }
}
