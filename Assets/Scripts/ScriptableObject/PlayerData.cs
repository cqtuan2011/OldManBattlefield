using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UIScriptable", menuName = "SO/SO_HUD")]
public class PlayerData : ScriptableObject
{
    public int ammo;
    public int health;
}
