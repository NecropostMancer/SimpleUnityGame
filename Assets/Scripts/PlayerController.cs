using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家的操作。
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update


    Vector3 worldPos;
    [SerializeField]
    Transform pivot;
    [SerializeField]
    Transform pivot2;

    [SerializeField]
    float VSpeed = 200f;
    [SerializeField]
    float HSpeed = 300f;
    [SerializeField]
    float speedMult = 2.0f;
    [SerializeField]
    bool _lockCursor = true;
    [SerializeField]
    float aimmingFOV = 30f;
    [SerializeField]
    float normalFOV = 60f;
    [SerializeField]
    Camera FirstCamera;


    bool lockCursor
    {
        set
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            _lockCursor = value;
        }
        get
        {
            return _lockCursor;
        }
    }

    [SerializeField]
    private WeaponSlot weaponSlot;

    

    void Start()
    {
        worldPos = pivot.position;
        lastDiag = new Vector2();
        if(lockCursor)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    Vector2 lastDiag;
    int curIndex = 0;
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse Y")* Time.deltaTime;
        float vertical = Input.GetAxis("Mouse X") * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.K))
        {
            WeaponSwitch(curIndex);
            curIndex++;
            
        }
        //transform

        Vector2 delta = new Vector2(horizontal * HSpeed * -1 * speedMult, vertical * VSpeed * speedMult);

        float angle = (pivot2.eulerAngles.x + 90f)%360;

        if(angle + delta.x < 1f)
        {
            delta.x = 0;
        }
        else if(angle + delta.x > 100f)
        {
            delta.x = 0;
        }



        //delta.x = angle + delta.x < 280f? 0 : delta.x;

        //Debug.Log(angle);
        
        pivot.Rotate(new Vector3(0f, delta.y, 0f));
        pivot2.Rotate(new Vector3(delta.x, 0f , 0f));
        
    }

    void WeaponSwitch(int index)
    {
        weaponSlot.Switch(index);
    }
}
