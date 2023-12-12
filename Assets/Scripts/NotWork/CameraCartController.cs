using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CinemachineDollyCart))]
public class CameraCartController : MonoBehaviour
{
    [SerializeField] private bool followTarget;
    private CinemachineDollyCart cart;
    private CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update

    private void Awake()
    {
       cart = GetComponent<CinemachineDollyCart>();
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }


    public void InitDolly(CinemachineSmoothPath dollyTrack , Transform target , float speed = 0f )
    {
        if(speed != 0f)
        {
            cart.m_Speed = speed;
            cart.m_Path = dollyTrack;
            if (followTarget)
            {
                virtualCamera.m_LookAt = target;
            }
        }
    }
}
