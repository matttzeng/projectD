using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using ProjectD;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace ProjectDInternal
{    public class CharacterControl : MonoBehaviour,
        AnimationControllerDispatcher.IAttackFrameReceiver,
        AnimationControllerDispatcher.IFootstepFrameReceiver,
        AnimationControllerDispatcher.ISkillFrameReceiver
    {

        
        public static CharacterControl Instance { get; protected set; }

        public int Speed = 10;
        public Level level;
     
        public CharacterData Data => m_CharacterData;
        public CharacterData CurrentTarget => m_CurrentTargetCharacterData;

        public Transform WeaponLocator;

        [Header("Audio")]
        public AudioClip[] SpurSoundClips;

        Vector3 m_LastRaycastResult;
        Animator m_Animator;
        NavMeshAgent m_Agent;
        CharacterData m_CharacterData;

        HighlightableObject m_Highlighted;

        RaycastHit[] m_RaycastHitCache = new RaycastHit[16];

        int m_SpeedParamID;
        int m_AttackParamID;
        int m_HitParamID;
        int m_FaintParamID;
        int m_RespawnParamID;
        int m_SkillAttackParamID;

        bool m_IsKO = false;
        float m_KOTimer = 0.0f;

        int m_InteractableLayer;
        int m_LevelLayer;
        Collider m_TargetCollider;
        InteractableObject m_TargetInteractable = null;
        Camera m_MainCamera;

        NavMeshPath m_CalculatedPath;

        CharacterAudio m_CharacterAudio;

        int m_TargetLayer;
        CharacterData m_CurrentTargetCharacterData = null;
        //this is a flag that tell the controller it need to clear the target once the attack finished.
        //usefull for when clicking elwswhere mid attack animation, allow to finish the attack and then exit.
        bool m_ClearPostAttack = false;

        SpawnPoint m_CurrentSpawn = null;

        enum State
        {
            DEFAULT,
            HIT,
            ATTACKING
        }

        State m_CurrentState;

        void Awake()
        {
            Instance = this;
            m_MainCamera = Camera.main;
        }

        // Start is called before the first frame update
        void Start()
        {
            level = new Level(1, OnLevelUp);

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            m_CalculatedPath = new NavMeshPath();

            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponentInChildren<Animator>();

            m_Agent.speed = Speed;
            m_Agent.angularSpeed = 360.0f;

            m_LastRaycastResult = transform.position;

            m_SpeedParamID = Animator.StringToHash("Speed");
            m_AttackParamID = Animator.StringToHash("Attack");
            m_HitParamID = Animator.StringToHash("Hit");
            m_FaintParamID = Animator.StringToHash("Faint");
            m_RespawnParamID = Animator.StringToHash("Respawn");
            m_SkillAttackParamID = Animator.StringToHash("Skill");

            m_CharacterData = GetComponent<CharacterData>();

            m_CharacterData.Equipment.OnEquiped += item =>
            {
                if (item.Slot == (EquipmentItem.EquipmentSlot)666)
                {
                    var obj = Instantiate(item.WorldObjectPrefab, WeaponLocator, false);
                    Helpers.RecursiveLayerChange(obj.transform, LayerMask.NameToLayer("PlayerEquipment"));
                }
            };

            m_CharacterData.Equipment.OnUnequip += item =>
            {
                if (item.Slot == (EquipmentItem.EquipmentSlot)666)
                {
                    foreach (Transform t in WeaponLocator)
                        Destroy(t.gameObject);
                }
            };

            m_CharacterData.Init();

            m_InteractableLayer = 1 << LayerMask.NameToLayer("Interactable");
            m_LevelLayer = 1 << LayerMask.NameToLayer("Level");
            m_TargetLayer = 1 << LayerMask.NameToLayer("Target");

            m_CurrentState = State.DEFAULT;

            m_CharacterAudio = GetComponent<CharacterAudio>();

            m_CharacterData.OnDamage += () =>
            {
                m_Animator.SetTrigger(m_HitParamID);
                m_CharacterAudio.Hit(transform.position);
            };
        }

        //被球打扣血
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "enemyBullet")
            {
                Debug.Log("被"+collision.gameObject.name+"打中呢");
               

                //子彈 對玩家造成傷害
                //下面這個應該才是對的方向  但是無效
                //collision.gameObject.GetComponent<CharacterData>().Attack(m_CharacterData);
                //這個是硬找一個值去扣玩家的血  脫離原本characterData的架構了
                int Dam = collision.gameObject.GetComponent<bullet>().damage;               
                StatSystem.StatModifier modifier = new StatSystem.StatModifier();
                m_CharacterData.Stats.ChangeHealth(-1*Dam);
                Debug.Log("被打了" + Dam + "血");

                //兩秒後刪除子彈
                Destroy(collision.gameObject,2f);

             
                
            }
        }
       
        public void OnLevelUp()
        {
            

             StatSystem.StatModifier modifier = new StatSystem.StatModifier();
            //m_CharacterData.Stats.ChangeHealth(m_CharacterData.Stats.CurrentHealth + 10);
            m_CharacterData.Stats.ChangeHealth(10);
           // Debug.Log("目前血量+10");

            m_CharacterData.Stats.AddModifier(modifier);

            modifier.Stats.health += 10;
            //Debug.Log("最大血量+10 = " + m_CharacterData.Stats.stats.health);

            //Debug.Log("升到了等級 "+level.currentLevel);
        }

        // Update is called once per frame
        void Update()
        {
           
            
            
            CharacterData data = this.Data;
            var stats = data.Stats.stats;
            Speed =   stats.moveSpeed;


          

           

            

            Vector3 pos = transform.position;

            if (m_IsKO)
            {
                m_KOTimer += Time.deltaTime;
                if (m_KOTimer > 3.0f)
                {
                    GoToRespawn();
                }

                return;
            }

            //The update need to run, so we can check the health here.
            //Another method would be to add a callback in the CharacterData that get called
            //when health reach 0, and this class register to the callback in Start
            //(see CharacterData.OnDamage for an example)
            if (m_CharacterData.Stats.CurrentHealth == 0)
            {
                m_Animator.SetTrigger(m_FaintParamID);

                m_Agent.isStopped = true;
                m_Agent.ResetPath();
                m_IsKO = true;
                m_KOTimer = 0.0f;

                Data.Death();

                m_CharacterAudio.Death(pos);

                return;
            }

            //Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);
            Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(transform.position);

            if (m_TargetInteractable != null)
            {
                CheckInteractableRange();
            }

            if (m_CurrentTargetCharacterData != null)
            {
                if (m_CurrentTargetCharacterData.Stats.CurrentHealth == 0)
                    m_CurrentTargetCharacterData = null;
                else
                    CheckAttack();
            }

            float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            if (!Mathf.Approximately(mouseWheel, 0.0f))
            {
                Vector3 view = m_MainCamera.ScreenToViewportPoint(Input.mousePosition);
                if (view.x > 0f && view.x < 1f && view.y > 0f && view.y < 1f)
                    CameraController.Instance.Zoom(-mouseWheel * Time.deltaTime * 20.0f);
            }

            if (Input.GetMouseButtonDown(0))
           // { //if we click the mouse button, we clear any previously et targets
                //m_Animator.SetTrigger(m_AttackParamID);
                if (m_CurrentState != State.ATTACKING)
                {
                    m_CurrentTargetCharacterData = null;
                    m_TargetInteractable = null;
                }
                else
                {
                    m_ClearPostAttack = true;
                }
           // }

            MoveCheck();

            //void updateTarget()
            //{

            /*
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

                    if (nearestEnemy != null && shortestDistance <= 15)
                    {
                        CharacterData data = m_Highlighted as CharacterData;
                        m_CurrentTargetCharacterData = data;
                    }
                    else
                    {
                        m_CurrentTargetCharacterData = null;
                    }


                }
            */
            //Debug.Log("State:"+ m_CurrentState);
                //if (!EventSystem.current.IsPointerOverGameObject() && m_CurrentState != State.ATTACKING)
                if (m_CurrentState != State.ATTACKING)
                {
                

                //Raycast to find object currently under the mouse cursor
                ObjectsRaycasts(screenRay);

                //if (Input.GetMouseButton(0))
                //{
                //if (m_TargetInteractable == null)
                        //{
                            //InteractableObject obj = m_Highlighted as InteractableObject;
                            //if (obj)
                            //{
                                //InteractWith(obj);
                            //}
                            //else
                            //{
                                CharacterData[] enemies = FindObjectsOfType<CharacterData>();
                                float shortestDistance = Mathf.Infinity;
                                CharacterData nearestEnemy = null;

                                foreach (CharacterData enemy in enemies)
                                {
                                   
                                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                                    if (distanceToEnemy <= shortestDistance && distanceToEnemy >= 0.1)
                                    {
                                        shortestDistance = distanceToEnemy;
                                        nearestEnemy = enemy;
                                    }
                                }    
                                    if (nearestEnemy != null && shortestDistance <= 5f)
                                    {
                                        m_CurrentTargetCharacterData = nearestEnemy as CharacterData;
                                    }
                                    else
                                    {
                                        m_CurrentTargetCharacterData = null;
                                        InteractableObject obj = m_Highlighted as InteractableObject;
                                        if (obj)
                                        {
                                            InteractWith(obj);
                                        }

                                    }
                                    /*
                                    CharacterData data = target as CharacterData;
                                    if (data != null)
                                    {
                                        m_CurrentTargetCharacterData = data;
                                    }
                                    else
                                    {
                                        //MoveCheck(screenRay);
                                    }
                                    */
                             

                                

                            //}
                        //}
                    //}
                }
            /*
            if (!EventSystem.current.IsPointerOverGameObject() && m_CurrentState != State.ATTACKING)
            {
                //Raycast to find object currently under the mouse cursor
                ObjectsRaycasts(screenRay);

                if (Input.GetMouseButton(0))
                {
                    if (m_TargetInteractable == null && m_CurrentTargetCharacterData == null)
                    {
                        InteractableObject obj = m_Highlighted as InteractableObject;
                        if (obj)
                        {
                            InteractWith(obj);
                        }
                        else
                        {
                            CharacterData data = m_Highlighted as CharacterData;
                            if (data != null)
                            {
                                m_CurrentTargetCharacterData = data;
                            }
                            else
                            {
                                //MoveCheck(screenRay);
                            }
                        }
                    }
                }
            }
            */

            m_Animator.SetFloat(m_SpeedParamID, m_Agent.velocity.magnitude / m_Agent.speed);

            //Keyboard shortcuts
            if (Input.GetKeyUp(KeyCode.I))
                UISystem.Instance.ToggleInventory();
        }

        void GoToRespawn()
        {
            m_Animator.ResetTrigger(m_HitParamID);

            m_Agent.Warp(m_CurrentSpawn.transform.position);
            m_Agent.isStopped = true;
            m_Agent.ResetPath();
            m_IsKO = false;

            m_CurrentTargetCharacterData = null;
            m_TargetInteractable = null;

            m_CurrentState = State.DEFAULT;

            m_Animator.SetTrigger(m_RespawnParamID);

            m_CharacterData.Stats.ChangeHealth(m_CharacterData.Stats.stats.health);
        }

        void ObjectsRaycasts(Ray screenRay)
        {
            bool somethingFound = false;

            //first check for Target not interactable Object
            int count = Physics.SphereCastNonAlloc(screenRay, 5.0f, m_RaycastHitCache, 1000.0f, m_TargetLayer);
            
            if (count > 0)
            {
                for (int i = 0; i < count; ++i)
                {
                    CharacterData data = m_RaycastHitCache[0].collider.GetComponentInParent<CharacterData>();
                    //InteractableObject obj = m_RaycastHitCache[0].collider.GetComponentInParent<InteractableObject>();
                    if (data != null)
                    {
                        SwitchHighlightedObject(data);
                        //Debug.Log("切換Highlight敵人");
                        somethingFound = true;
                        break;
                    }
                }
            }
            else
            {
                count = Physics.SphereCastNonAlloc(screenRay, 10.0f, m_RaycastHitCache, 1000.0f, m_InteractableLayer);
                //Debug.Log("找到裝備" + count);
                if (count > 0)
                {
                    //CharacterData data = m_RaycastHitCache[0].collider.GetComponentInParent<CharacterData>();
                    InteractableObject obj = m_RaycastHitCache[0].collider.GetComponentInParent<InteractableObject>();
                    if (obj != null && obj.IsInteractable)
                    {
                        SwitchHighlightedObject(obj);
                        //Debug.Log("切換Highlight裝備");
                        somethingFound = true;
                    }
                }
            }

            if (!somethingFound && m_Highlighted != null)
            {
                SwitchHighlightedObject(null);
            }
        }

        void SwitchHighlightedObject(HighlightableObject obj)
        {
            if (m_Highlighted != null) m_Highlighted.Dehighlight();

            m_Highlighted = obj;
            if (m_Highlighted != null) m_Highlighted.Highlight();
        }
        
        void MoveCheck()
        {
            
            float xDirection = Input.GetAxis("Horizontal");
            float zDirection = Input.GetAxis("Vertical");

            Vector3 moverDirection = new Vector3(xDirection, 0.0f, zDirection);

            

           

            transform.position += moverDirection * Speed/20;

            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                transform.rotation = Quaternion.LookRotation(moverDirection, Vector3.up);
                m_Animator.SetBool("Run", true);
            }
            else
            {
                m_Animator.SetBool("Run", false);
            }
            /*
            if (m_CalculatedPath.status == NavMeshPathStatus.PathComplete)
            {
                m_Agent.SetPath(m_CalculatedPath);
                m_CalculatedPath.ClearCorners();
            }
            
            if (Physics.RaycastNonAlloc(screenRay, m_RaycastHitCache, 1000.0f, m_LevelLayer) > 0)
            {
                Vector3 point = m_RaycastHitCache[0].point;
                //avoid recomputing path for close enough click
                if (Vector3.SqrMagnitude(point - m_LastRaycastResult) > 1.0f)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(point, out hit, 0.5f, NavMesh.AllAreas))
                    {//sample just around where we hit, avoid setting destination outside of navmesh (ie. on building)
                        m_LastRaycastResult = point;
                        //m_Agent.SetDestination(hit.position);

                        m_Agent.CalculatePath(hit.position, m_CalculatedPath);
                    }
                }
            }
            */
        }
        

        void CheckInteractableRange()
        {
            if (m_CurrentState == State.ATTACKING)
                return;

            Vector3 distance = m_TargetCollider.ClosestPointOnBounds(transform.position) - transform.position;


            if (distance.sqrMagnitude < 1.5f * 1.5f)
            {
                StopAgent();
                m_TargetInteractable.InteractWith(m_CharacterData);
                m_TargetInteractable = null;
            }
        }

        void StopAgent()
        {
            m_Agent.ResetPath();
            m_Agent.velocity = Vector3.zero;
        }

        void CheckAttack()
        {
            if (m_CurrentState == State.ATTACKING)
                return;

            if (m_CharacterData.CanSkillAttackTarget(m_CurrentTargetCharacterData))
            {
                StopAgent();

                //if the mouse button isn't pressed, we do NOT attack
                //if (Input.GetMouseButton(0))
                //{
                    Vector3 forward = (m_CurrentTargetCharacterData.transform.position - transform.position);
                    forward.y = 0;
                    forward.Normalize();


                    transform.forward = forward;
                    if (m_CharacterData.CanSkillAttackTarget(m_CurrentTargetCharacterData))
                    {
                        m_CurrentState = State.ATTACKING;

                        m_CharacterData.SkillAttackTriggered();
                        m_Animator.SetTrigger(m_SkillAttackParamID);

                    }

            }

            if (m_CharacterData.CanAttackReach(m_CurrentTargetCharacterData))
            {
                StopAgent();

                //if the mouse button isn't pressed, we do NOT attack
                //if (Input.GetMouseButton(0))
                //{
                    Vector3 forward = (m_CurrentTargetCharacterData.transform.position - transform.position);
                    forward.y = 0;
                    forward.Normalize();


                    transform.forward = forward;
                    if (m_CharacterData.CanAttackTarget(m_CurrentTargetCharacterData))
                    {
                        m_CurrentState = State.ATTACKING;

                        m_CharacterData.AttackTriggered();
                        m_Animator.SetTrigger(m_AttackParamID);

                    }
                //}
            }
            /*else
            {
                m_Agent.SetDestination(m_CurrentTargetCharacterData.transform.position);
            }*/
        }

        public void AttackFrame()
        {
            if (m_CurrentTargetCharacterData == null)
            {
                m_ClearPostAttack = false;
                return;
            }

            //if we can't reach the target anymore when it's time to damage, then that attack miss.
            if (m_CharacterData.CanAttackReach(m_CurrentTargetCharacterData))
            {
                m_CharacterData.Attack(m_CurrentTargetCharacterData);

                var attackPos = m_CurrentTargetCharacterData.transform.position + transform.up * 0.5f;
                VFXManager.PlayVFX(VFXType.Hit, attackPos);
                SFXManager.PlaySound(m_CharacterAudio.UseType, new SFXManager.PlayData() { Clip = m_CharacterData.Equipment.Weapon.GetHitSound(), PitchMin = 0.8f, PitchMax = 1.2f, Position = attackPos });
            }

            if (m_ClearPostAttack)
            {
                m_ClearPostAttack = false;
                m_CurrentTargetCharacterData = null;
                m_TargetInteractable = null;
            }

            m_CurrentState = State.DEFAULT;
        }

        public void SkillFrame()
        {
            if (m_CurrentTargetCharacterData == null)
            {
                m_ClearPostAttack = false;
                return;
            }

            //if we can't reach the target anymore when it's time to damage, then that attack miss.
            if (m_CharacterData.CanSkillAttackReach(m_CurrentTargetCharacterData))
            {
                m_CharacterData.SkillAttack(m_CurrentTargetCharacterData);

                var attackPos = m_CurrentTargetCharacterData.transform.position + transform.up * 0.5f;
                Vector3 pos = transform.position;
                VFXManager.PlayVFX(VFXType.Nova, pos);
                VFXManager.PlayVFX(VFXType.Hit, attackPos);
                SFXManager.PlaySound(m_CharacterAudio.UseType, new SFXManager.PlayData() { Clip = m_CharacterData.Equipment.Weapon.GetHitSound(), PitchMin = 0.8f, PitchMax = 1.2f, Position = attackPos });
            }

            if (m_ClearPostAttack)
            {
                m_ClearPostAttack = false;
                m_CurrentTargetCharacterData = null;
                m_TargetInteractable = null;
            }

            m_CurrentState = State.DEFAULT;
        }

        public void SetNewRespawn(SpawnPoint point)
        {
            if (m_CurrentSpawn != null)
                m_CurrentSpawn.Deactivated();

            m_CurrentSpawn = point;
            m_CurrentSpawn.Activated();
        }

        public void InteractWith(InteractableObject obj)
        {
            if (obj.IsInteractable)
            {
                m_TargetCollider = obj.GetComponentInChildren<Collider>();
                m_TargetInteractable = obj;
                //m_Agent.SetDestination(obj.transform.position);
            }
        }
        
        public void FootstepFrame()
        {
            Vector3 pos = transform.position;

            //m_CharacterAudio.Step(pos);

            //SFXManager.PlaySound(SFXManager.Use.Player, new SFXManager.PlayData()
            //{
            //    Clip = SpurSoundClips[Random.Range(0, SpurSoundClips.Length)],
            //    Position = pos,
            //    PitchMin = 0.8f,
            //    PitchMax = 1.2f,
            //    Volume = 0.3f
            //});

            VFXManager.PlayVFX(VFXType.StepPuff, pos);
        }
    }
}