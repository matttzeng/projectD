using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectD;


public class bullet :MonoBehaviour
{
    
    private Transform target;
    public float speed = 30f;
    public int damage = 1;
    public float attackRange = 3f;
    public GameObject impactFX;
    private Vector3 dir;
    public CharacterData Shooter;

    public void Seek(Transform _target)
    {
       
        target = _target;
    }


    private void Start()
    {
        //damage = gameObject.GetComponent<CharacterData>().Stats.stats.attack;

       dir = target.position - transform.position;

    }
    private void Update()
    {
       
       
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

       // Vector3 dir = target.position - transform.position;

        Vector3 dirToEnemy = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dirToEnemy.magnitude > 30)
        {
            Destroy(gameObject);
            return;
        }

        if (dirToEnemy.magnitude <= distanceThisFrame + attackRange)
        {
            
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        void HitTarget()
        {
           
            
            GameObject FXIns = (GameObject)Instantiate(impactFX, transform.position, transform.rotation);
            Destroy(FXIns, 2f);

            Damage(target);
            Destroy(gameObject);
        }
        
           
        void Damage (Transform enemy)
        {
            EnemyAI e = enemy.GetComponent<EnemyAI>();

            if(e != null)
            {
                e.TakeDamage(damage);
            }
           


        }

       



      
    }

    /*
    // Update is called once per frame
    void Update()
      {
          if (target == null)
          {
              Destroy(gameObject);
              return;
          }

          Vector3 dir = target.position - transform.position;
          float distanceThisFrame = speed * Time.deltaTime;



          if(dir.magnitude <= distanceThisFrame)
          {
              HitTarget();
              return;
          }

          transform.Translate(dir.normalized * distanceThisFrame, Space.World);

          void HitTarget()
          {

              GameObject FXIns =  (GameObject)Instantiate(impactFX, transform.position, transform.rotation);
              Destroy(FXIns, 2f);

              Destroy(gameObject);
          }
      }
    
    */
}
