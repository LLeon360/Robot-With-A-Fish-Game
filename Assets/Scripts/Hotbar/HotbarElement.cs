using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//wrapper class for HotbarElements to keep track of their cooldowns, cannot do this within HotbarElementObjects since scripable objects are static and should not keep track of runtime data
public class HotbarElement {
    public HotbarElementObject hotbarElementObject;
    public float nextUsableTime;
    public HotbarElement(HotbarElementObject hotbarElementObject)
    {
        this.hotbarElementObject = hotbarElementObject;
        this.nextUsableTime = 0f;
    }

    public void SetNextUsableTime()
    {
        nextUsableTime = Time.time + hotbarElementObject.cooldown;
    }

    public bool isUsable()
    {
        return Time.time > nextUsableTime;
    }
}
