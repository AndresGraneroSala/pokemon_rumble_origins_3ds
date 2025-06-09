using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_3DS
using UnityEngine.N3DS;
#endif

public class MenuNavigation : MonoBehaviour
{
    public float inputDelay = 0.2f;
    private float inputTimer = 0f;
    private bool hasMoved = false;

    private Stack<GameObject> firstButtonStack = new Stack<GameObject>();

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelectFirstButton();
        }

        inputTimer += Time.unscaledDeltaTime;

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        float dpadHorizontal = Input.GetAxisRaw("DPadHorizontal");
        float dpadVertical = Input.GetAxisRaw("DPadVertical");

        if (Mathf.Abs(dpadHorizontal) > 0.5f) moveHorizontal = dpadHorizontal;
        if (Mathf.Abs(dpadVertical) > 0.5f) moveVertical = dpadVertical;

#if UNITY_3DS
        moveHorizontal = UnityEngine.N3DS.GamePad.CirclePad.x;
        moveVertical = UnityEngine.N3DS.GamePad.CirclePad.y;
#endif

        if (inputTimer >= inputDelay)
        {
            hasMoved = false;

            if (moveVertical > 0.5f)
            {
                MoveSelection(Vector3.up);
                hasMoved = true;
            }
            else if (moveVertical < -0.5f)
            {
                MoveSelection(Vector3.down);
                hasMoved = true;
            }
            else if (moveHorizontal > 0.5f)
            {
                MoveSelection(Vector3.right);
                hasMoved = true;
            }
            else if (moveHorizontal < -0.5f)
            {
                MoveSelection(Vector3.left);
                hasMoved = true;
            }

            if (hasMoved)
                inputTimer = 0f;
        }

        if (Input.GetButtonDown("Submit") || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A))
        {
            PressCurrentButton();
        }
    }

    void MoveSelection(Vector3 direction)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return;

        Selectable currentSelectable = current.GetComponent<Selectable>();
        if (currentSelectable == null) return;

        Selectable next = null;

        if (direction == Vector3.up)
            next = currentSelectable.FindSelectableOnUp();
        else if (direction == Vector3.down)
            next = currentSelectable.FindSelectableOnDown();
        else if (direction == Vector3.left)
            next = currentSelectable.FindSelectableOnLeft();
        else if (direction == Vector3.right)
            next = currentSelectable.FindSelectableOnRight();

        if (next != null)
        {
            var nextGO = next.gameObject;

            if (nextGO.activeInHierarchy && next.IsInteractable())
            {
                EventSystem.current.SetSelectedGameObject(null); // Por seguridad
                EventSystem.current.SetSelectedGameObject(nextGO);
                next.OnSelect(null);
            }
        }
    }


    void PressCurrentButton()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return;

        Button btn = current.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.Invoke();
        }
    }

    void SelectFirstButton()
    {
        if (firstButtonStack.Count > 0)
        {
            GameObject top = firstButtonStack.Peek();
            if (top != null && top.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(top);
                return;
            }
        }

        GameObject fallback = GameObject.FindGameObjectWithTag("FirstButton");
        if (fallback != null)
        {
            EventSystem.current.SetSelectedGameObject(fallback);
        }
    }

    public void RegisterFirstButton(GameObject button)
    {
        if (!firstButtonStack.Contains(button))
        {
            firstButtonStack.Push(button);
        }

        if (button != null && button.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);

            // Forzar visualmente el estado "Selected"
            var selectable = button.GetComponent<Selectable>();
            if (selectable != null)
            {
                selectable.OnSelect(null);
            }

            print("entra " + button.name);
        }
    }


    public void UnregisterFirstButton(GameObject button)
    {
        if (firstButtonStack.Contains(button))
        {
            var tempStack = new Stack<GameObject>();
            while (firstButtonStack.Count > 0)
            {
                var item = firstButtonStack.Pop();
                if (item != button)
                    tempStack.Push(item);
            }
            while (tempStack.Count > 0)
                firstButtonStack.Push(tempStack.Pop());
            print("sale "+button.name);
        }
    }
}
