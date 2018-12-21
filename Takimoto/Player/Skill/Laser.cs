﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Skill/Laser")]
public class Laser : PlayerSkillBase
{
    [Header("攻撃力")]
    [SerializeField]
    private int attackPower = 2;
    /// <summary>
    /// 攻撃力
    /// </summary>
    public int AttackPower
    {
        get
        {
            return attackPower;
        }
    }
    [Header("消去時間")]
    [SerializeField]
    private float destroyTime = 2.4f;


    public override void ActivateSkill(Transform playerTrans, Vector3 skillCreationPos)
    {
        //Debug.Log("レーザー生成");
        Vector3 pos = 
            playerTrans.position + 
            playerTrans.right * skillCreationPos.x +
            playerTrans.up * skillCreationPos.y +
            playerTrans.forward * skillCreationPos.z;

        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(center);
        RaycastHit hit;

        Quaternion qua = new Quaternion();

        if (Physics.Raycast(ray, out hit, 1000.0f, LayerMask.GetMask(new string[] {"Field"})))
        {
            Vector3 vec = (hit.point - pos).normalized;
            qua = Quaternion.LookRotation(vec);
        }
        else
        {
            Debug.LogWarning("Rayで照準位置が取得できていない");
        }

        AttackCollision p = Instantiate(skillPrefab, pos, qua);
        Destroy(p.gameObject, destroyTime);
    }
}
