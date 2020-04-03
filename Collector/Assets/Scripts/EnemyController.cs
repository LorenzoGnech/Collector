using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};

public enum EnemyType{
    Melee, 
    Ranged
};

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currState = EnemyState.Idle;

    public EnemyType enemyType;

    public float range;

    public float speed;

    public float bulletSpeed;

    public float attackRange;

    public float coolDown = 1f;

    private bool chooseDirection = false;

    private bool dead = false;

    private bool coolDownAttack = false;

    private Vector3 randomDir;

    public GameObject bulletPrefab;

    public bool notInRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState){
            case(EnemyState.Idle):
                Idle();
                break;
            case(EnemyState.Wander):
                Wander();
                break;
            case(EnemyState.Follow):
                Follow();
                break;
            case(EnemyState.Die):
                Death();
                break;
            case(EnemyState.Attack):
                Attack();
                break;
        }
        
        if(!notInRoom){
            if(IsPlayerInRange(range) && currState != EnemyState.Die){
                currState = EnemyState.Follow;
            } else if(!IsPlayerInRange(range) && currState != EnemyState.Die){
                currState = EnemyState.Wander;
            }

            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange){
                currState = EnemyState.Attack;
            }
        } else{
            currState = EnemyState.Idle;
        }
    }



    private bool IsPlayerInRange(float range){
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection(){
        chooseDirection = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection = false;
    }

    void Wander(){
        if(!chooseDirection){
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        if(IsPlayerInRange(range)){
            currState = EnemyState.Follow;
        }
    }

    void Follow(){
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void Death(){
        Destroy(gameObject);
    }

    void Attack(){
        if(!coolDownAttack){
            switch(enemyType){
                case(EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;
                case(EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown()); 
                    break;
            }
        }
    }

    void Idle(){
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    private IEnumerator CoolDown(){
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }
}

