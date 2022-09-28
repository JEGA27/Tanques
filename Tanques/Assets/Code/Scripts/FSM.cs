using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    private enum FSMStates
    {
        Patrol, Chase, Aim, Shoot, Evade
    }

    [SerializeField]
    private FSMStates currentState = FSMStates.Patrol;
    private int health = 100;
    private Vector3 destPos;

    public GameObject bullet;
    public Transform playerTransform;
    public GameObject bulletSpawnPoint;
    public GameObject turretBase;
    public List<GameObject> pointList;
    public float curSpeed;
    public float rotSpeed = 150.0f;
    public float turretRotSpeed = 10.0f;
    public float maxForwardSpeed = 30.0f;
    public float maxBackwardSpeed = -30.0f;
    public float shootRate = 0.5f;
    private float elapsedTime;
    public float patrolRadius = 10f;
    private float patrolReachPlayer = 3f;
    public float chaseRadius = 25f;
    public float AttackRadius = 20f;
    private int index = -1;

    // Start is called before the first frame update
    void Start()
    {
        FindNextPoint();
    }

    private void FindNextPoint()
    {
        print("Finding next point");
        index = (index + 1) % pointList.Count; //Random.Range(0, pointList.Count);
        destPos = pointList[index].transform.position;
    }
    private void FindPlayer()
    {
        print("Finding player");
        destPos = playerTransform.position;
    }

    void Update()
    {
        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrol();
                break;

            case FSMStates.Chase:
                UpdateChase();
                break;

            case FSMStates.Aim:
                UpdateAim();
                break;

            case FSMStates.Shoot:
                UpdateShoot();
                break;

            case FSMStates.Evade:
                UpdateEvade();
                break;
        }
    }

    void UpdatePatrol()
    {
        //Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= patrolRadius)
        {
            print("Reached the destination point -- calculating the next point");
            FindNextPoint();
        }
        //Check the distance with player tank, when the distance is near, transition to chase state
        else if (Vector3.Distance(transform.position, playerTransform.position) <= chaseRadius)
        {
            print("Switch to Chase state");
            currentState = FSMStates.Chase;
        }
        //Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

    }

    void UpdateChase()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= patrolReachPlayer)
        {
            print("player reached");
            currentState = FSMStates.Aim;
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) >= chaseRadius)
        {
            print("player out of sight");
            currentState = FSMStates.Patrol;
        }
        FindPlayer();
        Quaternion TurretRotation = Quaternion.LookRotation(pointList[0].transform.position - transform.position);
        turretBase.transform.rotation = TurretRotation;
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    void UpdateAim()
    {

    }

    void UpdateShoot()
    {

    }

    void UpdateEvade()
    {

    }

    private void FixedUpdate() 
    {
        
    }
}
