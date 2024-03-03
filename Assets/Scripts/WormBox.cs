using AurumGames.Animation;
using AurumGames.Animation.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WormBox : MonoBehaviour
{
    public int amount;
    public TextMeshPro amountText;
    
    private void Start()
    {
        amountText.text = amount.ToString();

        var target = transform;
        Vector3 position = target.localPosition;
        Vector3 localScale = target.localScale;
        new AnimationPlayer(this, new ITrack[]
        {
            new LocalPositionTrack(target, new []
            {
                new KeyFrame<Vector3>(0, position + new Vector3(0, 10), Easing.QuadOut),
                new KeyFrame<Vector3>(500, position, Easing.QuadOut),
                new KeyFrame<Vector3>(580, position + new Vector3(0, 0.3f), Easing.QuadOut),
                new KeyFrame<Vector3>(650, position, Easing.QuadOut),
            }),
            new ScaleTrack(target, new []
            {
                new KeyFrame<Vector3>(400, new Vector3(localScale.x * 0.4f, localScale.y * 1.6f, 1), Easing.QuadOut),
                new KeyFrame<Vector3>(500, new Vector3(localScale.x * 1.6f, localScale.y * 0.4f, 1), Easing.QuadOut),
                new KeyFrame<Vector3>(540, localScale, Easing.QuadOut),
            })
        }).Play();
    }
    
    public void Interact()
    {
        Player.Instance.CollectWorms(amount);
        Destroy(gameObject);
    }
}
