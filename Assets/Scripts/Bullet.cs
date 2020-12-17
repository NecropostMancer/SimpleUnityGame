using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{

    public override void shoot(Vector3 speed,float str)
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~(1 << 9)))
        {

            //if (showBulletHitPoint)
            //{
            //    Instantiate(showBulletHitPoint, hitInfo.point, new Quaternion());
            //}
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
