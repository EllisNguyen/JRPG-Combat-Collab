using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask wallMask;

    private Camera mainCamera;

    [SerializeField] List<Material> curMaterials;
    [SerializeField] float fadeTime = 0.2f;
    [Range(0.05f, 0.15f)][SerializeField] float cutoutSize = 0.1f;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        if (hitObjects.Length == 0)
        {
            foreach (Material mat in curMaterials)
            {
                mat.DOFloat(0f, "_CutoutSize", fadeTime);
                mat.DOFloat(0f, "_FalloffSize", fadeTime);

                StartCoroutine(WaitToClear(0.01f));
            }
            return;
        }

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            if (curMaterials.Count == hitObjects.Length) return;

            foreach (Material mat in materials)
            {
                mat.DOFloat(cutoutSize, "_CutoutSize", fadeTime);
                mat.DOFloat(0.05f, "_FalloffSize", fadeTime);

                StartCoroutine(WaitToClear(0.01f));

                curMaterials.Add(mat);
            }
        }
    }

    IEnumerator WaitToClear(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);

        curMaterials.Clear();
    }
}