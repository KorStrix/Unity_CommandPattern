using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(ColumnPool_Replay))]
public class ColumnPool_AI : MonoBehaviour, ICommandExecuter
{
    public class ColumnPoolCommand_Reset : CommandBase
    {
        public ColumnPool_Replay pColumnPool_Replay;

        public override void DoExcute(ref SInputValue sInputValue, ref bool bIsExcuted_DefaultIsTrue)
        {
            pColumnPool_Replay.ResetColumn();
        }
    }

    public class ColumnPoolCommand_SetColumn : CommandBase
    {
        public ColumnPool_Replay pColumnPool_Replay;

        public override void DoExcute(ref SInputValue sInputValue, ref bool bIsExcuted_DefaultIsTrue)
        {
            pColumnPool_Replay.SetColumn(sInputValue.fAxisValue_Minus1_1);
        }
    }

    // ========================================================================== //

    public CObserverSubject<ICommandExecuteArg> p_Event_OnExecuteCommand { get; private set; } = new CObserverSubject<ICommandExecuteArg>();
    public bool p_bEnableExecuter { get; set; }

    public float spawnRate = 3f;                                    //How quickly columns spawn.
    public float columnMin = -1f;                                   //Minimum y value of the column position.
    public float columnMax = 3.5f;									//Maximum y value of the column position.

    ColumnPool_Replay _pColumnReplay;
    ColumnPoolCommand_Reset _pCommand_Reset = new ColumnPoolCommand_Reset();
    ColumnPoolCommand_SetColumn _pCommand_SetColumn = new ColumnPoolCommand_SetColumn();

    private float timeSinceLastSpawned;

    // ========================================================================== //

    void Awake()
    {
        _pColumnReplay = GetComponent<ColumnPool_Replay>();
        _pCommand_Reset.pColumnPool_Replay = _pColumnReplay;
        _pCommand_SetColumn.pColumnPool_Replay = _pColumnReplay;
    }

    void OnEnable()
    {
        p_bEnableExecuter = true;
        timeSinceLastSpawned = 0f;
        CManagerCommand.instance.DoAdd_CommandExecuter(this);
    }

    void OnDisable()
    {
        CManagerCommand.instance?.DoRemove_CommandExecuter(this);
    }

    public void ICommandExecuter_Update(ref List<CommandExcuted> listCommandExecute_Default_Is_Empty)
    {
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0f;

            float spawnYPosition = Random.Range(columnMin, columnMax);
            SInputValue sInput = new SInputValue(false, false, spawnYPosition, Vector2.zero, false);
            bool bExecute = true;
            _pCommand_SetColumn.DoExcute(ref sInput, ref bExecute);

            if(bExecute)
            {
                CommandExcuted pCommandExecute = new CommandExcuted(this, _pCommand_SetColumn, sInput, Time.time);
                listCommandExecute_Default_Is_Empty.Add(pCommandExecute);
                p_Event_OnExecuteCommand.DoNotify(new ICommandExecuteArg(pCommandExecute));
            }
        }
    }
}