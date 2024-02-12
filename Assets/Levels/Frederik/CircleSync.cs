using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeeThroughMaterial
{
    public Material material;
    public int counter;

    public SeeThroughMaterial(Material material, float CircleSize)
    {
        this.material = material;
        material.SetFloat(CircleSync.SizeID, CircleSize);
        counter = 1;
    }
}

public class CircleSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");
    
    public float SphereCastRadius = 1;
    public float CircleSize = 2;
    public Camera Camera;
    public LayerMask Mask;

    Dictionary<Collider, SeeThroughMaterial> _MaterialsByName;

    RaycastHit[] hits;

    private void Start()
    {
        _MaterialsByName = new Dictionary<Collider, SeeThroughMaterial>(10);
        hits = new RaycastHit[10];
        Camera = Camera.main;
    }

    void Update()
    {
        DecreaseCounter();
        var dir = Camera.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);

        var numHits = Physics.SphereCastNonAlloc(ray, SphereCastRadius, hits, dir.magnitude - (SphereCastRadius * 2), Mask);
        
        if(numHits > 0)
        {
            for (int i = 0; i < numHits; i++)
            {
                var hit = hits[i];
                if (hit.collider == null) continue;
                if (!_MaterialsByName.ContainsKey(hit.collider))
                {
                    Material mat = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    _MaterialsByName.Add(hit.collider, new SeeThroughMaterial(mat, CircleSize));
                }
                else
                {
                    _MaterialsByName[hit.collider].counter += 1;
                }
            }
        }
        ClearMaterialList();
        UpdateMatPos();
    }

    private void DecreaseCounter()
    {
        foreach (var col in _MaterialsByName.Keys)
        {
            _MaterialsByName[col].counter -= 1;
        }
    }

    private void ClearMaterialList()
    {
        var keys = new List<Collider>(_MaterialsByName.Keys);
        foreach (var col in keys)
        {
            if (_MaterialsByName[col].counter == 0)
            {
                _MaterialsByName[col].material.SetFloat(SizeID, 0);
                _MaterialsByName.Remove(col);
            }
        }
    }

    private void UpdateMatPos()
    {
        var view = Camera.WorldToViewportPoint(transform.position);

        foreach (var (col, stMat) in _MaterialsByName)
        {
            stMat.material.SetVector(PosID, view);
        }
    }
}

