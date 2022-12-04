using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using Photon.Pun;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float norSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private GameObject aimTarget;

    [SerializeField] private GameObject pfBulletProjectile;
    [SerializeField] private Transform firePosition;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator anim;
    private PlayerData playerData;
    private PhotonView PV;

    private bool isAim;
    private bool isReload;
    private bool isFinishAim;
    [HideInInspector] public bool isDead = false;

    private int defaultAmmo = 40;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!PV.IsMine) return;
        playerData = Resources.Load("ScriptableObject/PlayerData") as PlayerData;
        playerData.ammo = defaultAmmo;
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        Vector3 mouseWorldPosition = Vector3.zero;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
 

        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            mouseWorldPosition = raycastHit.point;
            //aimTarget.transform.position = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (starterAssetsInputs.aim && !isReload)
        {
            //canShoot = true;
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
            //canShoot = false;
        }

        if (canShoot())
        {
            PV.RPC("Shoot", RpcTarget.All);
            //Shoot();
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isAim", isAim);
        Reload((Input.GetKeyDown(KeyCode.R)));
    }

    private void Reload(bool isReloadPressed)
    {
        if (isReloadPressed)
        {
            isReload = true;
            playerData.ammo = defaultAmmo;
            anim.Play("A_TP_CH_AR_01_Reload");
        }
    }

    [PunRPC]
    private void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.GetComponent<ITakeDamage>()?.TakeDamage(15);
        }

        //Vector3 aimDir = (mouseWorldPosition - firePosition.position).normalized;
        GameObject bullet = Instantiate(pfBulletProjectile, firePosition);
        bullet.transform.SetParent(null);
        bullet.GetComponent<BulletProjectileRaycast>().Setup(hit.point);
        starterAssetsInputs.shoot = false;
        playerData.ammo -= 1;
        if (playerData.ammo <= 1)
        {
            Reload(true);
        }
    }

    private bool canShoot()
    {
        if (starterAssetsInputs.shoot && isFinishAim && starterAssetsInputs.aim)
            return true;
        return false;
    }

    public void ResetReload() => isReload = false;

    public void ResetFinishAim() => StartCoroutine(FinishAim());
    public IEnumerator FinishAim()
    {
        yield return new WaitForSeconds(1f);
        isFinishAim = true;
    }
}
