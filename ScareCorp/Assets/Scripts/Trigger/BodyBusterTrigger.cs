using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBusterTrigger : TriggerBase {

	protected Animator anim;
    public GameObject zombie;  // Probably temp
    public bool hasZombie = false;
    private Transform spawnPos;
    private void OnEnable()
    {
        spawnPos = gameObject.transform.Find("SpawnPoint");
        //zombie = this.transform.Find("Zombie").gameObject;
    }


    protected override void Init()
    {
        base.Init();
        anim = transform.GetChild(1).GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "visitor")
        {
            anim.SetTrigger("Scare");
        }
    }


    public override void ObjectEvent()
	{
		if (!beenUsed)
		{
            hasZombie = true;
            anim = GetComponent<Animator> ();
			
            GameObject zomb;
            zomb = Instantiate(zombie) as GameObject;
            zomb.transform.position = new Vector3(spawnPos.position.x, spawnPos.transform.position.y , spawnPos.position.z);
            zomb.transform.rotation = spawnPos.transform.rotation;
			triggered = true;
			beenUsed = true;
		}
	}
		
}
