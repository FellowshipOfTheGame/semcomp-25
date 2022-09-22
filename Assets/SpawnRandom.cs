using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs;

    //SpawnChance => Define qual é a chance de um dado prefab aparecer. 
    [SerializeField] List<int> spawnChance;
    GameObject objSpawned;
    public void Spawn()
    {
        int index =  setIndex();
        objSpawned=Instantiate(prefabs[index], transform);
        //objSpawned=Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform);
    
    }

    void Awake()
    {
        Spawn();
    }

    int setIndex(){

        int totalSum = 0;

        //Soma total, que é usada para tirar um número aleatório.
        for(int i = 0; i < spawnChance.Count; i++){
            totalSum += spawnChance[i];
        }

        int thresHold = Random.Range(0,totalSum);


        int sum = 0;

        int index = 0;
        //se o Threshold for menor ou igual a soma atual, selecionamos esse valor e damos break
        for(int i = 0; i < spawnChance.Count; i++){
            sum += spawnChance[i];

            if(sum >= thresHold){
                index = i;
                return index;
            }
        }

        return index;
    }

}
