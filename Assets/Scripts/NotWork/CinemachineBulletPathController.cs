using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineBulletPathController : CinemachinePathController
{
    [SerializeField] LayerMask mask;
    public override bool checkIfPathIsClear(Transform target, float distance, Quaternion orientation)
    {
        if(Physics.BoxCast(target.TransformPoint(boxCollider.center), boxCollider.size/2f , target.forward , out RaycastHit hitInfo , orientation , distance , mask ))
        {
            Debug.LogError(hitInfo.collider.gameObject.name);
            return false;
        }
        return true;
    }
}
