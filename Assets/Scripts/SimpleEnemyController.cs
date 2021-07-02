using ProjectD;
using UnityEngine;
using UnityEngine.AI;


namespace ProjectDInternal
{
    public class SimpleEnemyController : MonoBehaviour,
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
        public float bulletForce;

        public AudioClip[] SpottedAudioClip;

        Vector3 m_StartingAnchor;
        Animator m_Animator;
        NavMeshAgent m_Agent;
        CharacterData m_CharacterData;

        CharacterAudio m_CharacterAudio;

        int m_SpeedAnimHash;
        int m_AttackAnimHash;
        int m_DeathAnimHash;
        int m_HitAnimHash;
        bool m_Pursuing;
        float m_PursuitTimer = 0.0f;

        State m_State;

        LootSpawner m_LootSpawner;
        CharacterControl g;

        public int bulletAmount = 1;




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

            //g = GameObject.Find("character").GetComponent<CharacterControl>();

        }

        // Update is called once per frame
        void Update()
        {
            //See the Update function of CharacterControl.cs for a comment on how we could replace
            //this (polling
            //) to a callback method.
            if (m_CharacterData.Stats.CurrentHealth == 0)
            {
                m_Animator.SetTrigger(m_DeathAnimHash);

                m_CharacterAudio.Death(transform.position);
                m_CharacterData.Death();

                if (m_LootSpawner != null)
                    m_LootSpawner.SpawnLoot();




                if (m_CharacterData.name == "Boss01Enemy")
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

                            if (m_CharacterData.CanAttackTarget(playerData))
                            {
                                m_CharacterData.AttackTriggered();
                                m_Animator.SetTrigger(m_AttackAnimHash);
                                m_State = State.ATTACKING;
                                m_Agent.ResetPath();
                                m_Agent.velocity = Vector3.zero;
                                m_Agent.isStopped = true;
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

            m_Animator.SetFloat(m_SpeedAnimHash, m_Agent.velocity.magnitude / Speed);
        }


        public void AttackFrame()
        {
            CharacterData playerData = CharacterControl.Instance.Data;
            //m_Animator.SetTrigger(m_AttackAnimHash);
            //if we can't reach the player anymore when it's time to damage, then that attack miss.
            if (!m_CharacterData.CanAttackReach(playerData))
                return;
            if (m_CharacterData.Stats.stats.attackRange <= 3)
            {
                //近戰判定擊中扣血
                m_CharacterData.Attack(playerData);
                Debug.Log("近戰");
            }
            else if (m_CharacterData.Stats.stats.attackRange > 3)
            {
                //遠程判定擊中扣血
                shootPlayer();




                //if (玩家被擊中)
                //{
                ////扣血
                //    m_CharacterData.Attack(playerData);
                //}


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


        void shootPlayer()
        {
            this.GetComponentInChildren<Transform>().LookAt(CharacterControl.Instance.Data.transform.position);
            transform.LookAt(CharacterControl.Instance.Data.transform.position);

            Debug.Log(this.GetComponentInChildren<Transform>().name);
            //Debug.Log("打玩家");
            Vector3 shootPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f, gameObject.transform.position.z);
            for (int i = 0; i < bulletAmount && i < 5; i++)
            {
                GameObject rb = Instantiate(GetComponent<CharacterData>().StartingWeapon.WorldObjectPrefab, shootPoint, Quaternion.identity);
                rb.GetComponent<Rigidbody>().useGravity = false;
                //Rigidbody rb = Instantiate(GetComponent<CharacterData>().StartingWeapon.WorldObjectPrefab, shootPoint, Quaternion.identity).GetComponent<Rigidbody>();
                //var l = rb.AddComponent<Rigidbody>();
                if (m_CharacterData != null)
                {
                    rb.GetComponent<bullet>().Shooter = m_CharacterData;
                    Vector3 newDir = Quaternion.Euler(0, i * 10, 0) * transform.forward;

                    if (i % 2 == 0 && i > 0)
                        newDir = Quaternion.Euler(0, -(i - 1) * 10, 0) * transform.forward;

                    rb.GetComponent<Rigidbody>().AddForce(newDir * bulletForce, ForceMode.Impulse);
                    //rb.GetComponent<Rigidbody>().AddForce(transform.up * 2f, ForceMode.Impulse);
                }
                var destroyTime = 3f;
                Destroy(rb, destroyTime);
            }

        }


    }
}