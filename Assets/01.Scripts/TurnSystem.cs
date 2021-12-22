using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TurnState {
    START,
    PLAYERTURN,
    ENEMYTURN,
    BETWEEN,
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

    private Unit playerUnit;
    private Unit enemyUnit;


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

        GameObject enemyGO = Instantiate(enemyPrefab, startTile.unitsPosTrms[1]);
        enemyUnit= enemyGO.GetComponent<Unit>();
        enemyUnit.currentTile = startTile;

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
        Debug.Log(unit.dice.currentNum+1);
        Moving(unit, playerUnit.currentTile, playerUnit.dice.currentNum + 2);
    }

    public void Moving(Unit unit, TileNode node,int index, int dir = 0)
    {
        if ( node.nextTileNodes.Length > 0)
        {
            if(index > 0)
            {
                index--;
                unit.index = index;
                if (state != TurnState.DIRCHOISE)
                {
                    state = TurnState.DIRCHOISE;
                    unit.OnMove += Moving;
                    unit.MoveTile();
                }
                else
                {
                    dir = node.nextTileNodes.Length > 1 ? dir: 0;

                    print(dir);
                    Debug.Log(node.gameObject.name);
                    Debug.Log(node.nextTileNodes[dir].gameObject.name);
                    node = node.nextTileNodes[dir];
                    unit.nodeWay.Enqueue(node);
                    Moving(unit, node, index, dir);
                }
            }
            else
            {
                unit.MoveTile();
            }
        }
        else
        {
            unit.MoveTile();
        }
    }

    public void EndOfTurn()
    {
        switch (state)
        {
            case TurnState.START:
                break;
            case TurnState.PLAYERTURN:
                state = TurnState.ENEMYTURN;
                break;
            case TurnState.ENEMYTURN:
                break;
            case TurnState.BETWEEN:
                break;
            case TurnState.WON:
                break;
            case TurnState.LOST:
                break;
            default:
                break;
        }
    }
}
