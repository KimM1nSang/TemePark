using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum UnitState
{
    PLAYER,
    ENEMY
}
public class Unit : MonoBehaviour
{
    public UnitState state = UnitState.PLAYER;

    public GameObject diceObj;
    public Dice dice { get; set; }
    [SerializeField] private Transform DiceTrm;

    public Queue<TileNode> nodeWay;
    public TileNode currentTile;

    public float moveDuration = 2;

    public int dir = 0;

    private void Start()
    {
        GameObject diceGO = Instantiate(diceObj, DiceTrm);

        dice = diceGO.GetComponent<Dice>();
        dice.unit = this;

        nodeWay = new Queue<TileNode>();

    }
  
}
