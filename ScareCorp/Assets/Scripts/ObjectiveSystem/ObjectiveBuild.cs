using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObjectiveBuild : Objective {

	[SerializeField]
	Material outlineMat;
	[SerializeField]
	protected GameObject ObjectiveToBuild;
	
	protected GameObject obj;
	protected bool displayOutline;
	// Use this for initialization
	protected override void Awake()
	{
		base.Awake();
		CreateObjectToBuildOutline();
	}

	private void CreateObjectToBuildOutline()
	{
		obj = Instantiate(ObjectiveToBuild) as GameObject;
        obj.transform.parent = this.transform;
        Destroy(obj.GetComponent<Buildable>());
        obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);                   
        obj.GetComponent<MeshRenderer>().material = outlineMat;
		if (obj.transform.childCount > 0)
            {
                for (int i = 0; i < obj.transform.childCount; i++)
                 {
					 if(obj.transform.GetChild(i).childCount > 0)
					 {
						 for (int k = 0; k < obj.transform.GetChild(i).childCount; k++)
                		 {
							 if (obj.transform.GetChild(i).transform.GetChild(k).GetComponent<MeshRenderer>())
							 {
                       			 obj.transform.GetChild(i).transform.GetChild(k).GetComponent<MeshRenderer>().material = outlineMat;
					 		 }
						 }
					 }
                     if (obj.transform.GetChild(i).GetComponent<MeshRenderer>())
					 {
                        obj.transform.GetChild(i).GetComponent<MeshRenderer>().material = outlineMat;
					 }
                 }
       		 }

		if(obj.GetComponent<Rigidbody>())
        {
            obj.GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(DestroyRigidBodies(obj));
        }
        else
        {
            if(transform.childCount > 0)
            {
                if (obj.transform.GetChild(0).GetComponent<Rigidbody>() != null)
                {
					Rigidbody rb;
                    rb = obj.transform.GetChild(0).GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    StartCoroutine(DestroyRigidBodies(obj));
                }
            }
             
        }
	}

	public void CheckBuiltObject(GameObject _builtObj)
	{
		if(ObjectiveToBuild.GetComponent<Buildable>().objectID == _builtObj.GetComponent<Buildable>().objectID)
		{
			ObjectiveComplete();
			Destroy(obj);
		}
	}
	protected override void ObjectiveComplete()
	{
		Debug.Log("Objective Complete");
		base.ObjectiveComplete();

	} 

	


	IEnumerator DestroyRigidBodies(GameObject obj)
    {
        yield return new WaitForSeconds(3.0f);   
        Destroy(obj.GetComponent<Rigidbody>());
		 obj.transform.parent = null;
    }
}
