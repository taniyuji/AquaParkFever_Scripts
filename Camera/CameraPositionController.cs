using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEditor;
using PathCreation;
using System.Linq;

//エディットモードでもアクティブなスライダーによってカメラを切り替えられるスクリプト
[ExecuteInEditMode]
public class CameraPositionController : MonoBehaviour
{
    [SerializeField]
    private CameraPositionMover cameraPositionMover;

    [SerializeField]
    private List<PathCreator> sliders;

    private PathCreator beforeSlider;

    // Start is called before the first frame update


/*
    void Update()
    {
        if (EditorApplication.isPlaying) return;

        var targetSlider = sliders.Where(i => i.gameObject.activeSelf).FirstOrDefault();

        if (targetSlider == null) return;

        if (targetSlider == beforeSlider) return;

        var Index = sliders.IndexOf(targetSlider);

        transform.position = cameraPositionMover.GetPosition(Index);

        beforeSlider = targetSlider;
    }
*/
}
