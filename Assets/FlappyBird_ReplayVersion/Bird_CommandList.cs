#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2019-06-18 오후 4:11:09
 *	개요 : 
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(menuName =nameof(Bird_CommandList))]
public class Bird_CommandList : CommandListBase
{
    protected override Type _pGetInheritedClassType => typeof(Bird_CommandList);

    public class Command_Jump : CommandBase
    {
        Bird _pBird;

        public override void OnInitCommand(out bool bIsInit)
        {
            _pBird = FindObjectOfType<Bird>();

            bIsInit = _pBird;
        }

        public override void DoExcute(ref SInputValue sInputValue, ref bool bIsExcuted_DefaultIsTrue)
        {
            _pBird?.Jump();
        }
    }
}