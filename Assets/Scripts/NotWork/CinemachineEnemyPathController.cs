using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineEnemyPathController : CinemachinePathController
{
    [SerializeField] List<LayerMask> excludedMasks;
    public override bool checkIfPathIsClear(Transform target, float distance, Quaternion orientation)
    {
        var colliders = Physics.OverlapBox(target.position + boxCollider.center , boxCollider.size/2f , orientation);
        if(colliders.Length != 0) { 
        
        foreach(var collider in colliders)
            {
                if(excludedMasks.Contains(collider.gameObject.layer))
                {
                    Debug.Log(collider.gameObject.name);
                    Debug.LogError("not free");
                    return false;
                }
            }
        
        
        }
        return true;
    }


}
