using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public enum TurnState {
    START,
    PLAYERTURN,
    ENEMYTURN,
    DIRCHOISE,
    WON,
    LOST }
public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField] private TileNode startTile;
    public TurnState state;

    public Unit playerUnit { get; set; }
    public Unit enemyUnit { get; set; }


    void Start()
    {
        Instance = this;
        state = TurnState.START;
        StartCoroutine( SetupTurn());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DiceInteract();
        }
    }
    private IEnumerator SetupTurn()
    {
        GameObject playerGO = Instantiate(playerPrefab, startTile.unitsPosTrms[0]);
        playerUnit = playerGO.GetComponent<Unit>();
        playerUnit.currentTile = startTile;
        playerUnit.state = UnitState.PLAYER;

        GameObject enemyGO = Instantiate(enemyPrefab, startTile.unitsPosTrms[1]);
        enemyUnit= enemyGO.GetComponent<Unit>();
        enemyUnit.currentTile = startTile;
        enemyUnit.state = UnitState.ENEMY;

        yield return new WaitForSeconds(2f);

        state = TurnState.PLAYERTURN;
        PlayerTurn();
    }
    private void PlayerTurn()
    {

    }

    public void DiceInteract()
    {
        if (state != TurnState.PLAYERTURN ) return;

        if (!playerUnit.dice.isRolling)
        {
            playerUnit.dice.Roll();
        }
        else
        {
            playerUnit.dice.Select();
        }
    }

    public void DiceCallback(Unit unit)
    {
        Debug.Log("ÁÖ»çÀ§ ´« : " + (unit.dice.currentNum+1));
        Moving(unit, unit.currentTile, unit.dice.currentNum + 1);
    }

    public void Moving(Unit unit, TileNode node,int index, int dir = 0)
    {
        int leng = node.nextTileNodes.Length;
        print(leng);
        print(node.name);
        if (leng > 0  && leng <= 1 && index > 0)
        {
            index--;

            //dir = node.nextTileNodes.Length > 1 ? dir : 0;
            dir = 0;
            node = node.nextTileNodes[dir];
            unit.nodeWay.Enqueue(node);
            print(unit.nodeWay.Count);
            Moving(unit, node, index, dir);
        }
        else
        {
            print("¹«ºù");
            print(unit.nodeWay.Count);
           MoveTile(unit, index);
        }
    }
    public void MoveTile(Unit unit,int index)
    {
        print(unit.nodeWay.Count);
        if (unit.nodeWay.Count <= 0)
        {
            print("À¯´Ö ³ëµå ÀúÀå¼Ò ºñ¾úÀ½");
            if(index > 0 && unit.currentTile.nextTileNodes.Length > 1)
            {
                StartCoroutine(ChoiseDir(unit, index));
            }
            else
            {
                EndOfTurn(unit);
            }
        }
        else
        {
            unit.transform.DOMove(unit.nodeWay.Peek().TileCenterTrm.position, unit.moveDuration).OnComplete(() => {
                unit.currentTile = unit.nodeWay.Dequeue(); MoveTile(unit,index);
            });
        }

    }

    private IEnumerator ChoiseDir(Unit unit,int index)
    {
        bool isCor = true;
        print("A = 0, S = 1");
        //print(index);
        while (isCor)
        {
            yield return new WaitForEndOfFrame();
            if(state == TurnState.PLAYERTURN)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    print("A = 0");
                    isCor = false;
                    unit.dir = 0;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    print("S = 1");
                    isCor = false;
                    unit.dir = 1;
                }
            }
            else if (state == TurnState.ENEMYTURN)
            {
                yield return new WaitForSeconds(1f);
                isCor = false;
                unit.dir = UnityEngine.Random.Range(0, 1);
            }

        }
        index--;
        unit.nodeWay.Enqueue(unit.currentTile.nextTileNodes[unit.dir]);
        Moving(unit, unit.currentTile.nextTileNodes[unit.dir], index, unit.dir);
    }

    public void EndOfTurn(Unit unit)
    {
        switch (unit.state)
        {
            case UnitState.PLAYER:
                state = TurnState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                break;
            case UnitState.ENEMY:
                state = TurnState.PLAYERTURN;
                break;
            default:
                break;
        }
    }
    private IEnumerator EnemyTurn()
    {
        enemyUnit.dice.Roll();
        yield return new WaitForSeconds(1.5f);
        enemyUnit.dice.Select();
    }
}
