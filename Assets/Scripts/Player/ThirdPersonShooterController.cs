using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float norSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private GameObject aimTarget;
    [SerializeField] private LayerMask aimColliderLayerMask;

    [SerializeField] private GameObject pfBulletProjectile;
    [SerializeField] private Transform firePosition;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator anim;

    private bool isAim;


    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            //aimTarget.transform.position = raycastHit.point;
            hitTransform = raycastHit.transform;

            if (raycastHit.collider.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage ITakeDamage))
            {

            }
        }
    
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.ResetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, Time.deltaTime * 20f);
            //anim.Play("A_TP_CH_AR_01_Aim_Pose");
            isAim = true;

            //anim.SetTrigger("Aim");
        } else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(norSensitivity);
            thirdPersonController.ResetRotateOnMove(true);
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, Time.deltaTime * 20f);
            isAim = false;
        }

        if (starterAssetsInputs.shoot)
        {
            //Vector3 aimDir = (mouseWorldPosition - firePosition.position).normalized;
            GameObject bullet = Instantiate(pfBulletProjectile, firePosition);
            bullet.transform.SetParent(null);
            bullet.GetComponent<BulletProjectileRaycast>().Setup(mouseWorldPosition);
            starterAssetsInputs.shoot = false;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isAim", isAim);
    }
}
