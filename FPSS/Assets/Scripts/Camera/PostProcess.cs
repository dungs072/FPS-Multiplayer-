using UnityEngine;

public class PostProcess : MonoBehaviour
{
    [SerializeField] private Shader shader;
    private Material material;
    
    private void Start() {
        material = new Material(shader);
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src,dest,material);
    }
}
