using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Unit : MonoBehaviour
{
    public GameObject diceObj;
    public Dice dice { get; set; }
    [SerializeField] private Transform DiceTrm;

    public Queue<TileNode> nodeWay { get; set; }
    public TileNode currentTile;

    private float moveDuration = 2;

    public delegate void CallBack(Unit unit, TileNode node, int index, int dir = 0);

    public int index;
    public int dir = 0;

    public CallBack OnMove { get; set; }

    private void Start()
    {
        GameObject diceGO = Instantiate(diceObj, DiceTrm);

        dice = diceGO.GetComponent<Dice>();
        dice.unit = this;

        nodeWay = new Queue<TileNode>();

        OnMove += ClearOnMove;
    }
    public void MoveTile()
    {
        if (nodeWay.Count <= 0)
        {
            Debug.Log("µµÂø");
            if(TurnSystem.Instance.state == TurnState.DIRCHOISE)
            {
                if(index > 0 &&currentTile.nextTileNodes.Length > 0)
                {
                    StartCoroutine(ChoiseDir());
                }
            }
        }
        else
        {
            print("AAAAAA");
            transform.DOMove(nodeWay.Peek().TileCenterTrm.position, moveDuration).OnComplete(() => {
                currentTile = nodeWay.Dequeue(); MoveTile(); 
            });
        }

    }

    private IEnumerator ChoiseDir()
    {
        bool isCor = true;
        print("A = 0, S = 1");
        print(index);
        while (isCor)
        {
            yield return new WaitForEndOfFrame();
            if(Input.GetKey(KeyCode.A))
            {
                print("A = 0");
                isCor = false;
                dir = 0;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                print("S = 1");
                isCor = false;
                dir = 1;
            }
        }
        OnMove?.Invoke(this, currentTile, index, dir);
    }
    private void ClearOnMove(Unit unit, TileNode node, int index, int dir = 0)
    {
        OnMove = null;
        OnMove += ClearOnMove;
    }

}
