using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Deklarasikan instance GameManager sebagai singleton

    public List<GameObject> defenders = new List<GameObject>(); // Buat list untuk menyimpan defender yang sudah dibeli 
    private void Awake() 
    {
       instance = this; 
    }
}
