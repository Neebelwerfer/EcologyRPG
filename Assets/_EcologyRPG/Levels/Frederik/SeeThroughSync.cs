using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeeThroughMaterial
{
    public Material material;
    public int counter;

    public SeeThroughMaterial(SeeThroughSync seeThroughSync, Material material, float CircleSize)
    {
        this.material = material;
        counter = 1;
        material.SetFloat(SeeThroughSync.SizeID, 0.5f);
        seeThroughSync.StartCoroutine(ScaleCircle(CircleSize));
    }


    public IEnumerator ScaleCircle (float targetSize)
    {
        float size = material.GetFloat(SeeThroughSync.SizeID);
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            material.SetFloat(SeeThroughSync.SizeID, Mathf.Lerp(size, targetSize, time));
            yield return null;
        }
    }
}

public class SeeThroughSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");
    
    public float SphereCastRadius = 1;
    public float CircleSize = 2;
    public Camera Camera;
    public LayerMask Mask;

    Dictionary<Collider, SeeThroughMaterial> _MaterialsByName;
    Dictionary<SeeThroughMaterial, Coroutine> _CoroutinesByMaterial;

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
                if (hit.collider.isTrigger) continue;
                if (!_MaterialsByName.ContainsKey(hit.collider) && hit.collider.gameObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
                {
                    Material mat = meshRenderer.material;
                    _MaterialsByName.Add(hit.collider, new SeeThroughMaterial(this, mat, CircleSize));
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
                var stMat = _MaterialsByName[col];
                StartCoroutine(stMat.ScaleCircle(0));
                _MaterialsByName.Remove(col);
            }
        }
    }

    private void UpdateMatPos()
    {
        var view = Camera.WorldToViewportPoint(transform.position);

        foreach (var (_, stMat) in _MaterialsByName)
        {
            stMat.material.SetVector(PosID, view);
        }
    }
}

