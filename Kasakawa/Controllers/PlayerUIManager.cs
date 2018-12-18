﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// プレイヤー関連のUIを管理するクラス
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    [Inject]
    private MainGameManager gameManager;

    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private Text skillNameText;

    [SerializeField]
    private Image skillIcon;

    void Awake()
    {

        // プレイヤーのHPが変化した場合、表示を更新する
        gameManager.ObserveEveryValueChanged(_ => gameManager.player.GetHp())
            .Subscribe(hp => { UpdateHPValue(hp); });

        // プレイヤーの選択スキルが変化した場合、表示を更新する
        gameManager.ObserveEveryValueChanged(_ => gameManager.player.GetSelectSkill())
            .Subscribe(skill => { UpdateSkillInfo(skill); });

    }

    private void Start()
    {
        InitializePlayerUI();
    }

    /// <summary>
    /// UI初期化処理
    /// </summary>
    private void InitializePlayerUI()
    {
        int playerHP = gameManager.player.GetHp();

        hpSlider.maxValue = gameManager.player.GetMaxHp();
        UpdateHPValue(playerHP);

        // 現在のスキル情報を表示する
        UpdateSkillInfo(gameManager.player.GetSelectSkill());

    }

    /// <summary>
    /// プレイヤーのHPをUIに反映する
    /// </summary>
    /// <param name="hp"></param>
    private void UpdateHPValue(int hp)
    {
        hpSlider.value = hp;
    }

    /// <summary>
    /// プレイヤーが選択しているスキルの情報をUIに反映する
    /// </summary>
    /// <param name="skill"></param>
    private void UpdateSkillInfo(PlayerSkillBase skill)
    {
        skillNameText.text = skill.SkillName;

        skillIcon.sprite = skill.SkillIcon;
    }

}