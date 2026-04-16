using System.Threading;
using UnityEngine;


//Makes that the objects fade in and do not just show. Takes all the child objects and let them fade in
//Works if all the children spawn at the same time!
public class VisualFadeSpawn : MonoBehaviour
{
    [SerializeField]
    public float fadeTime = 1;

    private float elapsedTime = 0;
    private float currentalpha = 0;
    private SpriteRenderer sr;
    SpriteRenderer[] children;



    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        children = GetComponentsInChildren<SpriteRenderer>();
        elapsedTime = 0;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        
    }

    private void OnDisable()
    {
        elapsedTime = 0;
        currentalpha = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime <= fadeTime) 
        {
            foreach (SpriteRenderer sr in children)
            {
                currentalpha = elapsedTime / fadeTime;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, currentalpha);
            }
        }
        
    }

}
