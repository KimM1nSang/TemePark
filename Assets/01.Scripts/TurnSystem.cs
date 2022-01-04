using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public enum TurnState {
    BEFORESTART,
    WATCH,
    START,
    PLAYERTURN,
    ENEMYTURN,
    DIRCHOISE,
    BETWEEN,
    SELECTED,
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

    public bool isCor { get; set; } = true;
    public int unitDir { get; set; }
    void Awake()
    {
        Instance = this;
        state = TurnState.BEFORESTART;
        UIManager.Instance.StartButton.onClick.AddListener(() => { UIManager.Instance.RemoveButton(); StartCoroutine(SetupTurn()); });
        UIManager.Instance.WatchButton.onClick.AddListener(() => { UIManager.Instance.RemoveButton(); state = TurnState.WATCH; });
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

        state = TurnState.START;

        float delay = 2f;

        UIManager.Instance.TurnText("PlayerTurn", delay);
        yield return new WaitForSeconds(delay);

        state = TurnState.PLAYERTURN;
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
        //print(leng);
        if (leng > 0  && leng <= 1 && index > 0)
        {
            index--;

            //dir = node.nextTileNodes.Length > 1 ? dir : 0;
            dir = 0;
            node = node.nextTileNodes[dir];
            unit.nodeWay.Enqueue(node);
            //print(unit.nodeWay.Count);
            Moving(unit, node, index, dir);
        }
        else
        {
            print("¹«ºù");
            //print(unit.nodeWay.Count);
           MoveTile(unit, index);
        }
    }
    public void MoveTile(Unit unit,int index)
    {
        //print(unit.nodeWay.Count);
        if (unit.nodeWay.Count <= 0)
        {
            print("À¯´Ö ³ëµå ÀúÀå¼Ò ºñ¾úÀ½");
            if(index > 0 && unit.currentTile.nextTileNodes.Length > 1)
            {
                StartCoroutine(ChoiseDir(unit, index));
            }
            else
            {
                StartCoroutine(EndOfTurn(unit));
            }
        }
        else
        {
            print("length" + unit.nodeWay.Peek().currentUnits.Count);
            TileNode nodePeek = unit.nodeWay.Peek();
            int count = nodePeek.currentUnits.Count;
            if (count>= 1)
            {
                nodePeek.currentUnits[0].transform.DOMove(unit.nodeWay.Peek().unitsPosTrms[0].position, nodePeek.currentUnits[0].moveDuration*5).SetSpeedBased();
                unit.transform.DOMove(unit.nodeWay.Peek().unitsPosTrms[count].position, unit.moveDuration).SetSpeedBased().OnComplete(() => {
                    unit.currentTile.currentUnits.Remove(unit);
                    unit.currentTile = unit.nodeWay.Dequeue();
                    unit.currentTile.currentUnits.Add(unit);
                    MoveTile(unit, index);
                });
            }
            else
            {
                unit.transform.DOMove(unit.nodeWay.Peek().TileCenterTrm.position, unit.moveDuration).SetSpeedBased().OnComplete(() => {
                    unit.currentTile.currentUnits.Remove(unit);
                    unit.currentTile = unit.nodeWay.Dequeue();
                    unit.currentTile.currentUnits.Add(unit);
                    MoveTile(unit, index);
                });
            }

           
        }

    }

    private IEnumerator ChoiseDir(Unit unit,int index)
    {
        print("A = 0, S = 1");
        //print(index);
        while (isCor)
        {
            yield return new WaitForEndOfFrame();
            if(unit.state == UnitState.PLAYER)
            {
                
            }
            if (unit.state == UnitState.ENEMY)
            {
                yield return new WaitForSeconds(1f);
                isCor = false;
                unitDir = UnityEngine.Random.Range(0, 1);
            }

        }

        print(unitDir);
        isCor = true;
        index--;
        unit.nodeWay.Enqueue(unit.currentTile.nextTileNodes[unitDir]);
        Moving(unit, unit.currentTile.nextTileNodes[unitDir], index, unitDir);
    }

    public IEnumerator EndOfTurn(Unit unit)
    {
        float delay = 2f;
        switch (unit.state)
        {
            case UnitState.PLAYER:
                state = TurnState.BETWEEN;
                UIManager.Instance.TurnText("EnemyTurn", delay);
                yield return new WaitForSeconds(delay);
                state = TurnState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
                break;
            case UnitState.ENEMY:
                state = TurnState.BETWEEN;
                UIManager.Instance.TurnText("PlayerTurn", delay);
                yield return new WaitForSeconds(delay);
                state = TurnState.PLAYERTURN;
                break;
            default:
                break;
        }
    }
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1.5f);
        enemyUnit.dice.Roll();
        yield return new WaitForSeconds(1.5f);
        enemyUnit.dice.Select();
    }
}
