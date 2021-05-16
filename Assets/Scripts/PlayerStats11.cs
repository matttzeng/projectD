using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats11 : MonoBehaviour
{
    public static int Money;
    public static int Exp;
    public static int PlayHP=100;
    public int startMoney = 400;
    public static int Score =0;
    public static int FinalScore;


    // Start is called before the first frame update
    void Start()
    {
        Money = startMoney;
        Score = 0;
        PlayHP = 10;
    }

  public  void Update()
    {
       
    }


}
