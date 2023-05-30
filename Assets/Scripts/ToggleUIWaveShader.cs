using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ToggleUIWaveShader : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Material _wavyMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void SetWavyMaterial()
    {
        _image.material = _wavyMaterial;
    }

    public void UnsetWavyMaterial()
    {
        _image.material = null;
    }
}
