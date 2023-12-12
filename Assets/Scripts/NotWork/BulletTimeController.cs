using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TimeScaleController))]
public class BulletTimeController : MonoBehaviour
{
    public class TargetTrackingSetup
    {
        public CinemachinePathController avaliableTrack;
        public CameraCartController avalibleDolly;
    }

    public class BulletTrackingSetup : TargetTrackingSetup
    {
        public float minDistance;
        public float maxDistance;
    }
    [SerializeField]
    private GameObject canves;
    [SerializeField]
    private CinemachineBrain cameraBrain;
    [SerializeField]
    private BulletTrackingSetup[] bulletsTrackingSetup;
    [SerializeField]
    private TargetTrackingSetup[] targetsTrackingSetup;
    [SerializeField]
    private Player player;
    [SerializeField]
    private float distanceToChangeCamera;
    [SerializeField]
    private float finishCameraDuration;
    private TimeScaleController timeScaleController;
    private CinemachineSmoothPath trackInstance;
    private CameraCartController dollyInstance;
    private BulletProjectile bulletProjectile;
    private Vector3 targetPosition;
    private List<TargetTrackingSetup> clearTracks = new List<TargetTrackingSetup>();
    private bool isLastCameraActive;


    private void Awake()
    {
        timeScaleController = GetComponent<TimeScaleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!bulletProjectile) return;
        if (CheckIfBulletIsNearTarget()) ChangeCamera();
    }

    internal void StartSequence(BulletProjectile bulletProjectile , Vector3 targetPosiotion)
    {
        ResetVariables();
        float distanceToTarget = Vector3.Distance(bulletProjectile.transform.position, targetPosiotion);
        var setupsInRange = bulletsTrackingSetup.Where(s => distanceToTarget>s.minDistance && distanceToTarget < s.maxDistance).ToArray();
        var selectTrakingSetup = SelectTrakingSetup(bulletProjectile.transform , setupsInRange , bulletProjectile.transform.rotation);
        
            if(selectTrakingSetup == null)
            return;

            this.bulletProjectile = bulletProjectile;
            this.targetPosition = targetPosiotion;


        CreateBulletPath(bulletProjectile.transform, selectTrakingSetup.avaliableTrack);
        CreateDolly(selectTrakingSetup);
        cameraBrain.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        canves.gameObject.SetActive(false);
        float speed = CalculateDollySpeed();
        dollyInstance.InitDolly(trackInstance, bulletProjectile.transform, speed);

        
    }

    private float CalculateDollySpeed()
    {
        if(trackInstance == null || bulletProjectile == null)
            return 0f;


            float distanceToTarget = Vector3.Distance(bulletProjectile.transform.position, targetPosition);
        float speed = 2f;
            float pathDistance = trackInstance.PathLength;
            return pathDistance * speed / distanceToTarget;
        
    }

    private void CreateDolly(TargetTrackingSetup setup)
    {
        var selectedDolly = setup.avalibleDolly;
        dollyInstance = Instantiate(selectedDolly);
    }

    private TargetTrackingSetup SelectTrakingSetup(Transform trans, TargetTrackingSetup[] setups, Quaternion rotation)
    {
        clearTracks.Clear();
        for (int i = 0; i < setups.Length; i++)
        {
            if (CheckIfPathIsClear(setups[i].avaliableTrack , trans , rotation))
                clearTracks.Add(setups[i]);
        }
        if(clearTracks.Count == 0)
        {
            return null;
           
        }
        return clearTracks[Random.Range(0, clearTracks.Count)];
    }

    private bool CheckIfPathIsClear(CinemachinePathController path, Transform trans, Quaternion rotation)
    {
        return path.checkIfPathIsClear(trans , Vector3.Distance(trans.position , targetPosition), rotation);
    }

    private void CreateBulletPath(Transform bulletTransform , CinemachinePathController selectedPath)
    {
        trackInstance = Instantiate(selectedPath.path, bulletTransform);
        trackInstance.transform.localPosition = selectedPath.transform.position;
        trackInstance.transform.localRotation = selectedPath.transform.rotation;
    }

    private bool CheckIfBulletIsNearTarget()
    {
        return Vector3.Distance(bulletProjectile.transform.position, targetPosition) < distanceToChangeCamera;
    }

    private void ChangeCamera()
    {
        if (isLastCameraActive) return;
        isLastCameraActive = true;
        DestroyCinemachineSetup();
        Transform hitTransform = gameObject.transform;
        if (hitTransform)
        {
            Quaternion rotation = Quaternion.Euler(Vector3.up * bulletProjectile.transform.rotation.eulerAngles.y);
            var selectedTrackingSetup = SelectTrakingSetup(hitTransform , targetsTrackingSetup, rotation);
            if (selectedTrackingSetup != null)
            {
                CreateEnemyPath(hitTransform, bulletProjectile.transform, selectedTrackingSetup.avaliableTrack);
                CreateDolly(selectedTrackingSetup);
                dollyInstance.InitDolly(trackInstance, hitTransform.transform);
                timeScaleController.SlowDownTime();
            }
        }
        StartCoroutine(FinishSequence());
        
    }

    private IEnumerator FinishSequence()
    {
        yield return new WaitForSecondsRealtime(finishCameraDuration);
        cameraBrain.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        canves.gameObject.SetActive(true);
        timeScaleController.SpeedUpTime();
        DestroyCinemachineSetup() ;
        Destroy(bulletProjectile.gameObject);
        ResetVariables();
    }

    private void CreateEnemyPath(Transform enemytransform , Transform bulletTransform , CinemachinePathController selectedPath)
    {
        Quaternion rotation = Quaternion.Euler(Vector3.up * bulletTransform.root.eulerAngles.y);
        trackInstance = Instantiate(selectedPath.path , enemytransform.position, rotation);
    }


    private void DestroyCinemachineSetup()
    {
        Destroy(trackInstance.gameObject);
        Destroy(dollyInstance.gameObject);
    }


    private void ResetVariables()
    {
        isLastCameraActive = false;
        trackInstance = null;
        dollyInstance = null;
        bulletProjectile = null;
        clearTracks.Clear();
        targetPosition = Vector3.zero;
    }

}
