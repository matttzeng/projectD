﻿using System.Collections;
using System.Collections.Generic;
using ProjectD;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

namespace ProjectDInternal 
{
    /// <summary>
    /// Main class that handle the Game UI (health, open/close inventory)
    /// </summary>
    public class UISystem : MonoBehaviour
    {
        public static UISystem Instance { get; private set; }
        public Text WaveNumText;
        private int currentWave;
        public Text topLevelText;

        [Header("Player")]
        public CharacterControl PlayerCharacter;
        public Slider PlayerHealthSlider;
        public Text MaxHealth;
        public Text CurrentHealth;
        public Text currentLevel;
        public GameObject expPB;
        public EffectIconUI[] TimedModifierIcones;
        public Text StatsText;
        public Text StatsPointText;
        public Text SkillPointText;

        [Header("Enemy")]
        public Slider EnemyHealthSlider;
        //public Text EnemyName;
        public EffectIconUI[] EnemyEffectIcones;
    
        [Header("Inventory")]
        public InventoryUI InventoryWindow;
        public Button OpenInventoryButton;
        public AudioClip OpenInventoryClip;
        public AudioClip CloseInventoryClip;

        Sprite m_ClosedInventorySprite;
        Sprite m_OpenInventorySprite;
      

       

        void Awake()
        {
            Instance = this;
        
            InventoryWindow.Init();
        }

        void Start()
        {
            InventoryWindow.Load(PlayerCharacter.Data);
            //匯入腳色技能圖示, 暫定3個技能
            InventoryWindow.LoadSkill(PlayerCharacter.Data);
            m_ClosedInventorySprite = ((Image)OpenInventoryButton.targetGraphic).sprite;
            m_OpenInventorySprite = OpenInventoryButton.spriteState.pressedSprite;
                    
            for (int i = 0; i < TimedModifierIcones.Length; ++i)
            {
                TimedModifierIcones[i].gameObject.SetActive(false);
            }
        
            for (int i = 0; i < EnemyEffectIcones.Length; ++i)
            {
                EnemyEffectIcones[i].gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePlayerUI();
        }

       public void UpdatePlayerUI()
        {
           
            CharacterData data = PlayerCharacter.Data;

            currentWave = WaveSpawner.waveNumber;

            if (currentWave == 0)
            {
                WaveNumText.text = "START!";
            }
            else
                WaveNumText.text = "Wave " + currentWave.ToString();


            PlayerHealthSlider.value = PlayerCharacter.Data.Stats.CurrentHealth / (float) PlayerCharacter.Data.Stats.stats.health;
            MaxHealth.text = PlayerCharacter.Data.Stats.stats.health.ToString();
            CurrentHealth.text = PlayerCharacter.Data.Stats.CurrentHealth.ToString() + " / " + PlayerCharacter.Data.Stats.stats.health.ToString();
            currentLevel.text = "Lv:"+PlayerCharacter.level.currentLevel.ToString();
            topLevelText.text = "TOP WAVE : " +currentWave.ToString() + "\nLV : "+PlayerCharacter.level.currentLevel.ToString();
          
            if (PlayerCharacter.CurrentTarget != null)
            {
                UpdateEnemyUI(PlayerCharacter.CurrentTarget);
            }
            else
            {
                EnemyHealthSlider.gameObject.SetActive(false);
            }

            int maxTimedEffect = data.Stats.TimedModifierStack.Count;
            for (int i = 0; i < maxTimedEffect; ++i)
            {
                var effect = data.Stats.TimedModifierStack[i];

                TimedModifierIcones[i].BackgroundImage.sprite = effect.EffectSprite;
                TimedModifierIcones[i].gameObject.SetActive(true);
                TimedModifierIcones[i].TimeSlider.value = effect.Timer / effect.Duration;
            }

            for (int i = maxTimedEffect; i < TimedModifierIcones.Length; ++i)
            {
                TimedModifierIcones[i].gameObject.SetActive(false);
            }

            UpdateStatsText();
            //var stats = data.Stats.stats;
            //StatsText.text = $"剩餘點數 : {stats.statsPoint}\nAtk : {stats.attack} \nDef : {stats.defense} \nAtkSpeed : {stats.attackSpeed} \nMoveSpeed :{stats.moveSpeed} ";

            var playEXP = PlayerCharacter.level;         
           
            float ExpDiff = 0.25f * (playEXP.currentLevel + (300.0f * Mathf.Pow(2.0f, playEXP.currentLevel / 7.0f)));
            float topValue = playEXP.experience - playEXP.GetXPForLevel(playEXP.currentLevel);
            
            expPB.GetComponent<ProgressBar>().currentPercent = (topValue / ExpDiff)*100;

          

        
        }

        //更新UI素質欄
        public void UpdateStatsText()
        {
            CharacterData data = PlayerCharacter.Data;
            var stats = data.Stats.stats;
            SkillPointText.text = $"天賦點數 : {stats.skillPoint}\nSkillAtk : {stats.skill} \nSkillSpeed : {stats.skillSpeed} \nSKillRange: {stats.skillRange} \nMoveSpeed :{stats.moveSpeed} ";
            StatsPointText.text = $"等級點數 : {stats.statsPoint}\nAtk : {stats.attack} \nDef : {stats.defense} \nHP : {stats.health}  ";
            //StatsText.text = $"Atk : {stats.attack} \nDef : {stats.defense} \nAtkSpeed : {stats.attackSpeed} \nMoveSpeed :{stats.moveSpeed} ";
        }

        //素質加點
        public void AddStatsDEF()
        {
            CharacterData data = PlayerCharacter.Data;

            if (data.Stats.stats.statsPoint > 0)
            {
                data.Stats.baseStats.defense += 2;
                data.Stats.UpdateFinalStats();

                data.Stats.stats.statsPoint -= 1;
            }
        }

        public void AddStatsAtk()
        {
            CharacterData data = PlayerCharacter.Data;

            if (data.Stats.stats.statsPoint > 0)
            {
                data.Stats.baseStats.attack += 5;
                data.Stats.UpdateFinalStats();

                data.Stats.stats.statsPoint -= 1;
            }
        }

        public void AddStatsHP()
        {
            CharacterData data = PlayerCharacter.Data;

            if (data.Stats.stats.statsPoint > 0)
            {
                data.Stats.baseStats.health += 10;
                data.Stats.UpdateFinalStats();

                data.Stats.stats.statsPoint -= 1;
            }
        }
        //素質重置
        public void StatsReset()
        {
            CharacterData data = PlayerCharacter.Data;

            data.Stats.stats.statsPoint = data.Stats.stats.initPoint;
            data.Stats.Reset();
            data.Stats.UpdateFinalStats();
        }

        void UpdateEnemyUI(CharacterData enemy)
        {
            EnemyHealthSlider.gameObject.SetActive(true);
            EnemyHealthSlider.value = enemy.Stats.CurrentHealth / (float) enemy.Stats.stats.health;
            //EnemyName.text = enemy.CharacterName;

            int top = enemy.Stats.ElementalEffects.Count;
        
            for (int i = 0; i < top; ++i)
            {
                var effect = enemy.Stats.ElementalEffects[i];
            
                EnemyEffectIcones[i].gameObject.SetActive(true);
                EnemyEffectIcones[i].TimeSlider.value = effect.CurrentTime / effect.Duration;
            }

            for (int i = top; i < EnemyEffectIcones.Length; ++i)
            {
                EnemyEffectIcones[i].gameObject.SetActive(false);
            }
        }

        public void ToggleInventory()
        {

            if (InventoryWindow.gameObject.activeSelf)
            {
                ((Image)OpenInventoryButton.targetGraphic).sprite = m_ClosedInventorySprite;
                InventoryWindow.gameObject.SetActive(false);
                SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData(){ Clip = CloseInventoryClip});
            }
            else
            {
                ((Image)OpenInventoryButton.targetGraphic).sprite = m_OpenInventorySprite;
                InventoryWindow.gameObject.SetActive(true);
               // Debug.Log("進入Load");
                InventoryWindow.Load(PlayerCharacter.Data);
                SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData(){ Clip = OpenInventoryClip});
            }
      
       
    }
    }
}