using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Familiar.asset", menuName = "Collector/Familiars/FamiliarObject", order = 0)]
public class FamiliarData : ScriptableObject {
    
    public string familiarType;
    public float speed;
    public float fireDelay;
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
