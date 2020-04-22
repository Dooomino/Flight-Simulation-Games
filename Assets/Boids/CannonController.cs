using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject player;

    private GameObject stand;
    private GameObject outerBarrelLeft;
    private GameObject innerBarrelLeft;

    private GameObject muzzleFlashLeft;

    private GameObject outerBarrelRight;
    private GameObject innerBarrelRight;
    
    private GameObject muzzleFlashRight;
    private GameObject fireLocation;
    private Animator animatorController;
    public LayerMask playerLayer;
    private LineRenderer lineRenderer;
    private bool foundPlayer = false;

    public GameObject muzzleFlash;
    public GameObject playerExplosion;
    // Start is called before the first frame update
    void Start()
    {
        stand = this.transform.GetChild(0).gameObject;
        outerBarrelLeft = stand.transform.GetChild(0).gameObject;
        innerBarrelLeft = outerBarrelLeft.transform.GetChild(0).gameObject;
        muzzleFlashLeft = outerBarrelLeft.transform.GetChild(1).gameObject;


        outerBarrelRight = stand.transform.GetChild(1).gameObject;
        innerBarrelRight = outerBarrelRight.transform.GetChild(0).gameObject;
        muzzleFlashRight = outerBarrelRight.transform.GetChild(1).gameObject;

        animatorController = this.gameObject.GetComponent<Animator>();
        
    }

    public void fire(){
        Instantiate(muzzleFlash, muzzleFlashLeft.transform);
        Instantiate(muzzleFlash, muzzleFlashRight.transform);
        Instantiate(playerExplosion, player.transform);
    }
    void FixedUpdate(){
        foundPlayer = false;

        
        RaycastHit hitInfo;
        if(Physics.SphereCast(stand.transform.position, 5.0f, outerBarrelLeft.transform.up, out hitInfo, 45, playerLayer)){
            Debug.DrawLine(stand.transform.position,  hitInfo.transform.position,  Color.green);
            foundPlayer = true;
            
            
        }else{
            Debug.DrawLine(stand.transform.position,  outerBarrelLeft.transform.up* 45, Color.red);
        }
        animatorController.SetBool("foundEnemy", foundPlayer);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
