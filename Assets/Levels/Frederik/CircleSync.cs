using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    public Material WallMaterial;
    public Camera Camera;
    public LayerMask Mask;

    void Update()
    {
        var dir = Camera.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);

        if(Physics.Raycast(ray, out RaycastHit hitinfo, 3000, Mask))
        {
            WallMaterial.SetFloat(SizeID, 0);
            WallMaterial = hitinfo.collider.gameObject.GetComponent<MeshRenderer>().material;
            WallMaterial.SetFloat(SizeID, 2);
        }
        else
        {
            WallMaterial.SetFloat (SizeID, 0);
        }

        var view =  Camera.WorldToViewportPoint(transform.position);
        WallMaterial.SetVector(PosID, view);
    }
}
