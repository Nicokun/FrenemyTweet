﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

[CreateAssetMenu(menuName = "ScriptableObject/EnemySkill/SurroundingAttack")]
public class SurroundingAttack : EnemySkillBase
{

    private GameObject instantAreaObject;
    /// <summary>
    /// 攻撃するプレイヤーを格納
    /// </summary>
    private List<GameObject> attackPlayers = new List<GameObject>();


    public override void ActivateSkill(Transform thisTransform)
    {
        instantAreaObject = null;
        Vector3 instantPos = new Vector3(thisTransform.position.x, thisTransform.position.y + useAreaObj.transform.position.y, thisTransform.position.z);
        instantAreaObject = Instantiate(useAreaObj, instantPos, thisTransform.rotation);

        // 詠唱
        Observable.TimerFrame(getSkillChantFrame).Subscribe(_ =>
            AttackPlayerSearch(thisTransform.position)
        ).AddTo(thisTransform.gameObject);
    }

    /// <summary>
    /// 攻撃するプレイヤーを探す
    /// </summary>
    /// <param name="thisPosition"></param>
    private void AttackPlayerSearch(Vector3 thisPosition)
    {
        attackPlayers.Clear();
        // 攻撃するプレイヤーを取得
        attackPlayers = instantAreaObject.GetComponent<AttackArea>().GetAcquisitionPlayerList;
        Attack(attackPlayers);
        Destroy(instantAreaObject);
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    /// <param name="attackPlayers"></param>
    private void Attack(List<GameObject> attackPlayers)
    {
        for (int index = 0; attackPlayers.Count > index; index++)
        {
            ExecuteEvents.Execute<IDamage>(
                target: attackPlayers[index].gameObject,
                eventData: null,
                functor: (iDamage, eventData) => iDamage.TakeDamage(getAtackPower)
            );
            Debug.Log("周辺攻撃：【" + attackPlayers[index].gameObject.name + "】へ【" + getAtackPower + "】ダメージ");
        }
    }
}
