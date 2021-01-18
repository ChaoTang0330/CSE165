using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManage : MonoBehaviour
{
    public MainGame mainGame;

    public GameObject[] animalPrefab = new GameObject[4];
    public float spawnInterval = 6.0f, spawnRandRange = 3.0f;

    //private Vector3 leftPos = new Vector3(-6.0f, 0.0f, 10.0f);
    //private Vector3 rightPos = new Vector3(6.0f, 0.0f, 10.0f);
    private Vector3[] spawnPos = new Vector3[2];
    private Quaternion forwardQua = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, -1.0f));

    private float nextSpawnTimber;

    void Awake()
    {
        spawnPos[0].x = -6.0f;
        spawnPos[0].y = 0.0f;
        spawnPos[0].z = 10.0f;

        spawnPos[1].x = 6.0f;
        spawnPos[1].y = 0.0f;
        spawnPos[1].z = 10.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTimber = 3.0f;
        //Instantiate(animalPrefab[0], leftPos, forwardQua);
        //Instantiate(animalPrefab[1], rightPos, forwardQua);
        spawnInterval = mainGame.getSpawnInterval();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainGame.getStatus() == MainGame.CHANGED)
        {
            spawnInterval = mainGame.getSpawnInterval();
            return;
        }
        else if (mainGame.getStatus() != MainGame.CONTINUE)
        {
            return;
        }

        nextSpawnTimber -= Time.deltaTime;
        if(nextSpawnTimber <= 0.0f)
        {
            randSpawn();
            nextSpawnTimber = spawnInterval + Random.Range(0, spawnRandRange);
        }
    }

    void randSpawn()
    {
        int characterNum = Random.Range(0, 4);
        int posNum = Random.Range(0, 2);
        //Debug.Log("Position: " + posNum);
        Instantiate(animalPrefab[characterNum], spawnPos[posNum], forwardQua);
    }
}
