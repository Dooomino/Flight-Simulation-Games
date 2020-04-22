using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject player;

    private GameObject stand;
    private GameObject outerBarrel;
    private GameObject innerBarrel;

    private GameObject fireLocation;
    private Animator animatorController;
    public LayerMask playerLayer;

    private bool foundPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        stand = this.transform.GetChild(0).gameObject;
        outerBarrel = stand.transform.GetChild(0).gameObject;
        innerBarrel = outerBarrel.transform.GetChild(0).gameObject;
        fireLocation = outerBarrel.transform.GetChild(1).gameObject;
        animatorController = this.gameObject.GetComponent<Animator>();
    }

    void FixedUpdate(){
        Debug.DrawLine(outerBarrel.transform.position,  (fireLocation.transform.position.normalized) * 20, Color.yellow);
        RaycastHit hitInfo = new RaycastHit();
        if(!foundPlayer){
            
            //Search for the player by casting a ray
            if(Physics.SphereCast(outerBarrel.transform.position, 5.0f, fireLocation.transform.position.normalized, out hitInfo, 40.0f, playerLayer)){
                foundPlayer = true;
                animatorController.SetBool("foundEnemy", foundPlayer);
                
            }else{
                foundPlayer = false;
            }
        }else{
            if(Physics.SphereCast(outerBarrel.transform.position, 5.0f, fireLocation.transform.position.normalized, out hitInfo, 40.0f, playerLayer)){
                foundPlayer = true;
                
            }else{
                foundPlayer = false;
            }
                
            
            //if you've found the player, keep the barrel pointed at the player by rotating the base so it's looking at the player
            
        }

        if(foundPlayer){
            var lookAt = Quaternion.LookRotation(hitInfo.collider.gameObject.transform.position, Vector3.up);
            stand.transform.rotation = Quaternion.Slerp(stand.transform.rotation, lookAt, 0.5f);
        }

        animatorController.SetBool("foundEnemy", foundPlayer);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
