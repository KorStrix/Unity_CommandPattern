using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReplayObserver_Command))]
[RequireComponent(typeof(ReplayObserver_Transform))]
public class ReplayObserver_BirdCommand : CReplayObserverBase
{
    [GetComponent]
    ReplayObserver_Command _pReplayCommand = null;
    [GetComponent]
    ReplayObserver_Transform _pReplayTransform = null;


    protected override void OnEnableObject()
    {
        base.OnEnableObject();

        _pReplayTransform.EventOnAwake();

        bool bIsRecordData = false;
        _pReplayTransform.IReplayObserver_Record_ReplayData(Time.time, ref bIsRecordData);
    }

    public override void IReplayObserver_Clear_ReplayData()
    {
        _pReplayCommand.IReplayObserver_Clear_ReplayData();
        _pReplayTransform.IReplayObserver_Clear_ReplayData();
    }

    public override void IReplayObserver_Record_ReplayData(float fCurrentTime, ref bool bIsRecordData_DefaultValue_Is_False)
    {
        _pReplayCommand.IReplayObserver_Record_ReplayData(fCurrentTime, ref bIsRecordData_DefaultValue_Is_False);
        if (bIsRecordData_DefaultValue_Is_False)
        {
            // 커맨드를 기록할 때에만 Transform 위치를 기록합니다.
            _pReplayTransform.IReplayObserver_Record_ReplayData(fCurrentTime, ref bIsRecordData_DefaultValue_Is_False);
        }
    }

    public override void IReplayObserver_Seek_Replay(bool bIsFirstFrame, float fTime)
    {
        _pReplayCommand.IReplayObserver_Seek_Replay(bIsFirstFrame, fTime);

        if(bIsFirstFrame)
            _pReplayTransform.IReplayObserver_Seek_Replay(bIsFirstFrame, fTime);
    }
}
