using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");
    
    public float SphereCastRadius = 1;
    public float CircleSize = 2;
    public Camera Camera;
    public LayerMask Mask;

    List<Material> _Materials;

    private void Start()
    {
        _Materials = new List<Material>();
    }

    void Update()
    {
        var dir = Camera.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);

        ClearMaterialList();
        var hits = Physics.SphereCastAll(ray, SphereCastRadius, dir.magnitude - (SphereCastRadius * 2), Mask);
        
        if(hits.Length > 0)
        {
            foreach(var hit in hits)
            {
                Material mat = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                _Materials.Add(mat);
                mat.SetFloat(SizeID, CircleSize);
            }
        }
        UpdateMatPos();
    }

    private void ClearMaterialList()
    {
        foreach(var mat in _Materials)
        {
            mat.SetFloat(SizeID, 0);
        }
        _Materials.Clear();
    }

    private void UpdateMatPos()
    {
        foreach(var mat in _Materials)
        {
            var view = Camera.WorldToViewportPoint(transform.position);
            mat.SetVector(PosID, view);
        }
    }
}

