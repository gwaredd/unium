using UnityEngine;
using UnityEngine.EventSystems;

public class TestDragImage : MonoBehaviour
    , IPointerClickHandler
    , IDragHandler
    , IBeginDragHandler
    , IEndDragHandler
{
    private Vector3 mStartPosition;

    private void Start()
    {
        mStartPosition = transform.position;
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( "Image Clicked" );
    }

    public void OnDrag( PointerEventData eventData )
    {
        Debug.Log( "Image Drag" );
        transform.position = eventData.position;
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        Debug.Log( "Image Begin Drag" );
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        Debug.Log( "Image End Drag" );
        transform.position = mStartPosition;
    }
}
