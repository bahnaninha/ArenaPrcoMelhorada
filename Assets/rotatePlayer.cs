using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatePlayer : MonoBehaviour
{
    public Transform turret;
    public float velocity = 5.0f;

    public bool freezyY = true;

    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        
    }
    
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit))
            return;

        Vector3 pos = turret.position - hit.point;

        if (freezyY)
            pos.y = 0;

        Quaternion rot = Quaternion.LookRotation(pos);

        turret.rotation = Quaternion.Lerp(turret.rotation, rot, velocity * Time.deltaTime);
    }
}