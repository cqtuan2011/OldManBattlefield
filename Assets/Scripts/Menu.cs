using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;

    private MenuLauncher menuLauncher;

    private void Awake()
    {
        menuLauncher = FindObjectOfType<MenuLauncher>();
    }

    public void OnClickButton()
    {
        menuLauncher.OpenMenu(menuName);
    }
}
