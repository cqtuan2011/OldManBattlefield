using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float norSensitivity;
    [SerializeField] private float aimSensitivity;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    private Transform mainCam;
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
        } else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(norSensitivity);
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.position, mainCam.forward, out hit, Mathf.Infinity))
        {

        }
    }

}
