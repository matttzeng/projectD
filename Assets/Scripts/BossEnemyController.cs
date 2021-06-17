using System.Collections;
using ProjectD;
using UnityEngine;
using UnityEngine.AI;


namespace ProjectDInternal {
    public class BossEnemyController : MonoBehaviour, 
        AnimationControllerDispatcher.IAttackFrameReceiver,
        AnimationControllerDispatcher.IFootstepFrameReceiver
    {
        public enum State
        {
            IDLE,
            PURSUING,
            ATTACKING
        }
    
        public float Speed;
        public float detectionRadius;
        public float skillRadius;
        public float skillInterval;
        public float skillDelay;
        public float bulletForce;
      
        public AudioClip[] SpottedAudioClip;

        Vector3 m_StartingAnchor;
        Animator m_Animator;
        NavMeshAgent m_Agent;
        public CharacterData m_CharacterData;

        CharacterAudio m_CharacterAudio;

        int m_SpeedAnimHash;
        int m_AttackAnimHash;
        int m_DeathAnimHash;
        int m_HitAnimHash;
        int m_whirlwindAnimHash;
        int m_trippleSlashAnimHash;
        int skillFlag;
        
        bool m_Pursuing;
        float m_PursuitTimer = 0.0f;
        float m_SkillTimer = 0.0f;

        State m_State;

        LootSpawner m_LootSpawner;
        CharacterControl g;




        //public GameObject projectile;
        public NavMeshAgent agent;



        // Start is called before the first frame update
        void Start()
        {
            m_Animator = GetComponentInChildren<Animator>();
            m_Agent = GetComponent<NavMeshAgent>();
        
            m_SpeedAnimHash = Animator.StringToHash("Speed");
            m_AttackAnimHash = Animator.StringToHash("Attack");
            m_DeathAnimHash = Animator.StringToHash("Death");
            m_HitAnimHash = Animator.StringToHash("Hit");
            m_whirlwindAnimHash = Animator.StringToHash("whirlwind");
            m_trippleSlashAnimHash = Animator.StringToHash("trippleSlash");

            m_CharacterData = GetComponent<CharacterData>();
            m_CharacterData.Init();

            m_CharacterAudio = GetComponentInChildren<CharacterAudio>();
        
            m_CharacterData.OnDamage += () =>
            {
                m_Animator.SetTrigger(m_HitAnimHash);
                m_CharacterAudio.Hit(transform.position);
            };
        
            m_Agent.speed = Speed;
            
            m_LootSpawner = GetComponent<LootSpawner>();
        
            m_StartingAnchor = transform.position;
            m_SkillTimer = skillInterval;

            //g = GameObject.Find("character").GetComponent<CharacterControl>();

        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(CharacterControl.Instance.Data.transform.position);
            //See the Update function of CharacterControl.cs for a comment on how we could replace
            //this (polling
            //) to a callback method.
            if (m_CharacterData.Stats.CurrentHealth == 0)
            {
                m_Animator.SetTrigger(m_DeathAnimHash);
            
                m_CharacterAudio.Death(transform.position);
                m_CharacterData.Death();
            
                if(m_LootSpawner != null)
                    m_LootSpawner.SpawnLoot();

                

                
                    if (m_CharacterData.name == "CactusBossyEnemy")
                    {
                    GameObject.Find("Character").GetComponent<CharacterControl>().level.AddExp(200);
                     }
                    else
                    {
                       // g.level.AddExp(10);

                    GameObject.Find("Character").GetComponent<CharacterControl>().level.AddExp(50);


                    }
                



                Destroy(m_Agent);
                Destroy(gameObject);
                Destroy(GetComponent<Collider>());
                Destroy(this);
                return;
            }
 
            //NOTE : in a full game, this would use a targetting system that would give the closest target
            //of the opposing team (e.g. multiplayer or player owned pets). Here for simplicity we just reference
            //directly the player.
            Vector3 playerPosition = CharacterControl.Instance.transform.position;
            CharacterData playerData = CharacterControl.Instance.Data;

            //旋風斬使用中判定, 撞到角色時會停止, 需要修正
            if (gameObject.GetComponent<Rigidbody>() && skillFlag ==3)
            {
                Vector3 l = playerPosition - transform.position;
                l.Normalize();

                //Debug.Log(l);
                m_Agent.SetDestination(playerPosition);
                m_Agent.speed = 8;
                //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(l.x, 0.0f, l.z);
                //gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 999f, 0);
                return;
            }

            switch (m_State)
            {
                case State.IDLE:
                {
                    if (Vector3.SqrMagnitude(playerPosition - transform.position) < detectionRadius * detectionRadius)
                    {
                        if (SpottedAudioClip.Length != 0)
                        {
                            SFXManager.PlaySound(SFXManager.Use.Enemies, new SFXManager.PlayData()
                            {
                                Clip = SpottedAudioClip[Random.Range(0, SpottedAudioClip.Length)],
                                Position = transform.position
                            });
                        }

                        m_PursuitTimer = 4.0f;
                        m_State = State.PURSUING;
                        
                       m_Agent.isStopped = false;
                    }
                }
                    break;
                case State.PURSUING:
                {
                    float distToPlayer = Vector3.SqrMagnitude(playerPosition - transform.position);
                    if (distToPlayer < detectionRadius * detectionRadius)
                    {
                        m_PursuitTimer = 4.0f;
                        //視野範圍內時, 技能施放冷卻
                        m_SkillTimer -= Time.deltaTime;

                        if (m_CharacterData.CanAttackTarget(playerData))
                        {
                            m_CharacterData.AttackTriggered();
                            m_Animator.SetTrigger(m_AttackAnimHash);
                            m_State = State.ATTACKING;
                            m_Agent.ResetPath();
                            m_Agent.velocity = Vector3.zero;
                           
                                //m_Agent.isStopped = true;

                        }
                        //視野範圍內時, 技能施放
                        else if (m_SkillTimer <= 0)
                        {
                            transform.LookAt(CharacterControl.Instance.Data.transform.position);
                            m_CharacterData.AttackTriggered();
                            m_Animator.SetTrigger(m_AttackAnimHash);
                            //m_State = State.ATTACKING;
                            m_Agent.ResetPath();
                            m_Agent.velocity = Vector3.zero;
                           // m_Agent.isStopped = true;
                              
                            Invoke("SkillDelay", skillDelay);
                            m_SkillTimer = skillInterval;
                        }
                    }
                    else
                    {
                        if (m_PursuitTimer > 0.0f)
                        {
                            m_PursuitTimer -= Time.deltaTime;
   
                            if (m_PursuitTimer <= 0.0f)
                            {
                                m_Agent.SetDestination(m_StartingAnchor);
                                m_State = State.IDLE;
                            }
                        }
                    }
                
                    if (m_PursuitTimer > 0)
                    {
                        m_Agent.SetDestination(playerPosition);
                    }
                }
                    break;
                case State.ATTACKING:
                {
                    transform.LookAt(CharacterControl.Instance.Data.transform.position);
                    if (!m_CharacterData.CanAttackReach(playerData))
                    {
                        m_State = State.PURSUING;
                        m_Agent.isStopped = false;
                    }
                    else
                    {
                        if (m_CharacterData.CanAttackTarget(playerData))
                        {
                            m_CharacterData.AttackTriggered();
                            m_Animator.SetTrigger(m_AttackAnimHash);
                        }
                    }
                }
                    break;
            }
        
            m_Animator.SetFloat(m_SpeedAnimHash, m_Agent.velocity.magnitude/Speed);
        }

       
        public void AttackFrame()
        {
            CharacterData playerData = CharacterControl.Instance.Data;
            //m_Animator.SetTrigger(m_AttackAnimHash);
            //if we can't reach the player anymore when it's time to damage, then that attack miss.
            //if (!m_CharacterData.CanAttackReach(playerData))
                //return;
            if (m_CharacterData.Equipment.Weapon.Stats.MaxRange <= 3)
            {
                if (!m_CharacterData.CanAttackReach(playerData))
                    return;
                //近戰判定擊中扣血
                m_CharacterData.Attack(playerData);
                Debug.Log("近戰");
            }
            //遠程判定擊中扣血
            else if (m_CharacterData.Equipment.Weapon.Stats.MaxRange > 3)
            {
                //攻擊範圍內時
                if (m_CharacterData.CanAttackReach(playerData))
                    skillFlag = 3;
                //攻擊範圍外時(視野內)
                else
                    skillFlag = Random.Range(0, 3);
                //技能選擇
                shootPlayer(skillFlag); 
            }

        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        public void FootstepFrame()
        {
            Vector3 pos = transform.position;
        
            m_CharacterAudio.Step(pos);
            VFXManager.PlayVFX(VFXType.StepPuff, pos, Quaternion.Euler(0, 0, 0)); 
        }

        void shootPlayer(int skillFlag)
        {
            //丟子彈
            if (skillFlag == 0)
            {
                transform.LookAt(CharacterControl.Instance.Data.transform.position);

                //Debug.Log("打玩家");
                Vector3 shootPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f, gameObject.transform.position.z);
                GameObject rb = Instantiate(GetComponent<CharacterData>().StartingWeapon.WorldObjectPrefab, shootPoint, Quaternion.identity);
                //Rigidbody rb = Instantiate(GetComponent<CharacterData>().StartingWeapon.WorldObjectPrefab, shootPoint, Quaternion.identity).GetComponent<Rigidbody>();
                //var l = rb.AddComponent<Rigidbody>();
                if (m_CharacterData != null)
                {
                    rb.GetComponent<bullet>().Shooter = m_CharacterData;
                    rb.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce, ForceMode.Impulse);
                    rb.GetComponent<Rigidbody>().AddForce(transform.up * 2f, ForceMode.Impulse);
                }
            }
            //隕石術
            if (skillFlag == 1)
            {
                var l = CharacterControl.Instance.Data.transform.position;
                transform.LookAt(l);

                //Debug.Log("打玩家");
                for (int i = 0; i < 5; i++)
                {
                    Vector3 shootPoint = new Vector3(l.x + Random.Range(-5f, 5f), 15.0f, l.z + Random.Range(-5f, 5f));

                    GameObject rb = Instantiate(GetComponent<CharacterData>().StartingWeapon.WorldObjectPrefab, shootPoint, Quaternion.identity);
                    //Rigidbody rb = Instantiate(GetComponent<CharacterData>().StartingWeapon.WorldObjectPrefab, shootPoint, Quaternion.identity).GetComponent<Rigidbody>();
                    //var l = rb.AddComponent<Rigidbody>();
                    if (m_CharacterData != null)
                    {
                        rb.GetComponent<bullet>().Shooter = m_CharacterData;
                        //rb.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce, ForceMode.Impulse);
                        rb.GetComponent<Rigidbody>().AddForce(transform.up * -2f, ForceMode.Impulse);
                        Destroy(rb, 2f);
                    }
                }
            }
            //衝鋒攻擊
            if(skillFlag ==2)
            {
                transform.LookAt(CharacterControl.Instance.Data.transform.position);

                if (m_CharacterData != null)
                {
                    m_Agent.isStopped = false;
                    m_Animator.SetTrigger(m_trippleSlashAnimHash);
                    if (!gameObject.GetComponent<Rigidbody>())
                        gameObject.AddComponent<Rigidbody>();
                    gameObject.GetComponent<Rigidbody>().mass = 1;
                    //gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    //gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                    //gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    Debug.Log(transform.forward * bulletForce);
                    gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce/2, ForceMode.Impulse);
                    Invoke("Stop", 1.5f);
                }
            }
            //旋風斬
            if(skillFlag ==3)
            {
                if (m_CharacterData != null)
                {
                    m_Agent.isStopped = false;
                    m_Animator.SetTrigger(m_whirlwindAnimHash);
                    if (!gameObject.GetComponent<Rigidbody>())
                        gameObject.AddComponent<Rigidbody>();
                    gameObject.GetComponent<Rigidbody>().mass = 10000;
                    //gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<BoxCollider>().size = new Vector3(7, 2, 7);
                    //gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 999f, 0);
                    Invoke("Stop", 1.8f);
                }
            }
        }
        //停止
        public void Stop()
        {
            if (gameObject.GetComponent<Rigidbody>())
            {
                gameObject.GetComponent<BoxCollider>().size = new Vector3(3, 2, 3);
                Destroy(GetComponent<Rigidbody>());
                m_Agent.speed = Speed;
                Debug.Log("停止");
            }
        }
        //衝鋒攻擊, 撞到人時停止
        public void OnCollisionEnter(Collision collision)
        {
            if (gameObject.GetComponent<Rigidbody>() && skillFlag ==2)
            {
                Destroy(GetComponent<Rigidbody>());
                Debug.Log("撞到Character");
            }
        }

        public void SkillDelay()
        {
            m_Agent.isStopped = false;
        }


    }
}