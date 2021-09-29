using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private LevelMaster levelMaster;
    [SerializeField]
    private int id;
    public EnemyController[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool flag = false;
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            Debug.Log("no more enemies");
            levelMaster.NextLevel(id);
        }
    }
        public void SetLevelMaster(LevelMaster levelMaster)
        {
            this.levelMaster = levelMaster;
        }
     
    
}
