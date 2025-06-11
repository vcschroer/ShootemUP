using UnityEngine;
using UnityEngine.UI;

public class scrollBackground : MonoBehaviour
{
    public RawImage fundo;
    public float x;
    public float y;

    
    private void Start()
    {

    }

    void Update()
    {
        fundo.uvRect = new Rect(fundo.uvRect.position + new Vector2(x, y) * Time.deltaTime, fundo.uvRect.size);

    }

}
