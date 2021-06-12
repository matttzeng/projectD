using System;
using ProjectD;
using ProjectDInternal;
using UnityEngine;

using Random = UnityEngine.Random;

namespace ProjectD
{
    /// <summary>
    /// This defines a character in the game. The name Character is used in a loose sense, it just means something that
    /// can be attacked and have some stats including health. It could also be an inanimate object like a breakable box.
    /// </summary>
    public class CharacterData : HighlightableObject
    {

        [Header("Attributes")]





        


        public string CharacterName;
        private Transform m_target;
        public StatSystem Stats;
        /// <summary>
        /// The starting weapon equipped when the Character is created. Set through the Unity Editor.
        /// </summary>
        public Weapon StartingWeapon;
        public Skill ChooseSkill;
        public Skill[] Skill;
        public InventorySystem Inventory = new InventorySystem();
        public EquipmentSystem Equipment = new EquipmentSystem();

        public AudioClip[] HitClip;
    
        /// <summary>
        /// Callback for when that CharacterData receive damage. E.g. used by the player character to trigger the right
        /// animation
        /// </summary>
        public Action OnDamage { get; set; }

        /// <summary>
        /// Will return true if the attack cooldown have reached 0. False otherwise.
        /// </summary>
        public bool CanAttack
        {
            get { return m_AttackCoolDown <= 0.0f; }
        }

        float m_AttackCoolDown;
        float m_SkillAttackCoolDown;

        public void Init()
        {
            Stats.Init(this);            

            /* Stats change by every wave*/
            /*StatSystem.StatModifier modifier = new StatSystem.StatModifier();
            modifier.ModifierMode = StatSystem.StatModifier.Mode.Absolute;
            modifier.Stats.health += (WaveSpawner.waveNumber-1) * 10;
            
            this.Stats.AddModifier(modifier);*/

            Inventory.Init(this);
            Equipment.Init(this);

            if (StartingWeapon != null)
            {
                StartingWeapon.UsedBy(this);
                Equipment.InitWeapon(StartingWeapon, this);
            }

            /*if (ChooseSkill != null)
            {
                ChooseSkill.UsedBy(this);
                Debug.Log("裝技能");
                Equipment.InitSkill(ChooseSkill, this);
            }*/

            /*if(Skill.Length > 0)
            {
                for(int i = 0; i < Skill.Length; i++)
                {
                    Skill[i].UsedBy(this);
                    Debug.Log("裝技能");
                    Equipment.InitSkill(Skill[i], this);
                }
            }*/
        }

        void Awake()
        {
            Animator anim = GetComponentInChildren<Animator>();
            if(anim != null)
                SceneLinkedSMB<CharacterData>.Initialise(anim, this);

           

        }

        // Update is called once per frame
        void Update()
        {
            Stats.Tick();

            if (m_AttackCoolDown > 0.0f)
                m_AttackCoolDown -= Time.deltaTime;

            if (m_SkillAttackCoolDown > 0.0f)
                m_SkillAttackCoolDown -= Time.deltaTime;


            Animator anim = GetComponentInChildren<Animator>();
            if (anim != null)
                //這邊還要加個條件 避免怪物的ANIMATOR沒有ATTACKSPEED 一值跳黃
                anim.SetFloat("AttackSpeed",1+this.Stats.stats.attackSpeed*0.01f);

            
      
               

        }







        /// <summary>
        /// Will check if that CharacterData can reach the given target with its currently equipped weapon. Will rarely
        /// be called, as the function CanAttackTarget will call this AND also check if the cooldown is finished.
        /// </summary>
        /// <param name="target">The CharacterData you want to reach</param>
        /// <returns>True if you can reach the target, False otherwise</returns>
        public bool CanAttackReach(CharacterData target)
        {

            return Equipment._Weapon.CanHit(this, target);
        }

        public bool CanSkillAttackReach(CharacterData target)
        {

            return ChooseSkill.CanHit(this, target);
        }
        /// <summary>
        /// Will check if the target is attackable. This in effect check :
        /// - If the target is in range of the weapon
        /// - If this character attack cooldown is finished
        /// - If the target isn't already dead
        /// </summary>
        /// <param name="target">The CharacterData you want to reach</param>
        /// <returns>True if the target can be attacked, false if any of the condition isn't met</returns>
        public bool CanAttackTarget(CharacterData target)
        {
            if (target.Stats.CurrentHealth == 0)
                return false;
        
            if (!CanAttackReach(target))
                return false;

            if (m_AttackCoolDown > 0.0f)
                return false;

            return true;
        }

        public bool CanSkillAttackTarget(CharacterData target)
        {
            if (target.Stats.CurrentHealth == 0)
                return false;

            if (!CanSkillAttackReach(target))
                return false;

            if (m_SkillAttackCoolDown > 0.0f)
                return false;

            return true;
        }
        /// <summary>
        /// Call when the character die (health reach 0).
        /// </summary>
        public void Death()
        {
            Stats.Death();
        }

        /// <summary>
        /// Attack the given target. NOTE : this WON'T check if the target CAN be attacked, you should make sure before
        /// with the CanAttackTarget function.
        /// </summary>
        /// <param name="target">The CharacterData you want to attack</param>
        public void Attack(CharacterData target)
        {
            Equipment._Weapon.Attack(this, target);
        }

        public void SkillAttack(CharacterData target)
        {
            ChooseSkill.Attack(this, target);
        }
        /// <summary>
        /// This need to be called as soon as an attack is triggered, it will start the cooldown. This is separate from
        /// the actual Attack function as AttackTriggered will be called at the beginning of the animation while the
        /// Attack function (doing the actual attack and damage) will be called by an animation event to match the animation
        /// </summary>
        public void AttackTriggered()
        {
            //Agility reduce by 0.5% the cooldown to attack (e.g. if agility = 50, 25% faster to attack)
            // m_AttackCoolDown = Equipment.Weapon.Stats.Speed - (Stats.stats.agility * 0.5f * 0.001f * Equipment.Weapon.Stats.Speed);
            m_AttackCoolDown = Equipment._Weapon.Stats.Speed / (Stats.stats.attackSpeed *0.01f +1f);

           

            
        }

        public void SkillAttackTriggered()
        {
            m_SkillAttackCoolDown = ChooseSkill.Stats.Speed/ (Stats.stats.skillSpeed*0.01f+1f);// - (Stats.stats.agility * 0.5f * 0.001f * Skill.Stats.Speed;
        }
        /// <summary>
        /// Damage the Character by the AttackData given as parameter. See the documentation for that class for how to
        /// add damage to that attackData. (this will be done automatically by weapons, but you may need to fill it
        /// manually when writing special elemental effect)
        /// </summary>
        /// <param name="attackData"></param>
        public void Damage(Weapon.AttackData attackData)
        {
            if (HitClip.Length != 0)
            {
                SFXManager.PlaySound(SFXManager.Use.Player, new SFXManager.PlayData()
                {
                    Clip = HitClip[Random.Range(0, HitClip.Length)],
                    PitchMax =  1.1f,
                    PitchMin =  0.8f,
                    Position = transform.position
                });
            }
        
            Stats.Damage(attackData);
            
            OnDamage?.Invoke();
        }

        public void SkillDamage(Skill.AttackData attackData)
        {
            if (HitClip.Length != 0)
            {
                SFXManager.PlaySound(SFXManager.Use.Player, new SFXManager.PlayData()
                {
                    Clip = HitClip[Random.Range(0, HitClip.Length)],
                    PitchMax = 1.1f,
                    PitchMin = 0.8f,
                    Position = transform.position
                });
            }

            Stats.SkillDamage(attackData);

            OnDamage?.Invoke();
        }
        /*
        void updateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("
        ");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy <= shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }

                if (nearestEnemy != null && shortestDistance <= shootRange)
                {
                    target = nearestEnemy.transform;
                }
                else
                {
                    target = null;
                }


            }



        }
        */
        /*
        void shoot()
        {

            GameObject bulletGo = (GameObject)Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet bullet = bulletGo.GetComponent<bullet>();

            if (bullet != null)
                bullet.Seek(target);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootRange);
        }
        */

    }



}