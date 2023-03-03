using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Build : MonoBehaviour
{
    public Vector2 downLeftTileIndex;
    public ItemDefinition ItemDefinition;
    public ItemSelection ItemSelection;
    protected Sprite splash;
    protected Image information;
    protected Text nameArea;
    protected string buildingName;
    protected Button spawn;

    // get the information panel elements
    protected virtual void Start()
    {
        ItemSelection = FindObjectOfType<ItemSelection>();
        //information = UIManager.instance.informationImage;
        //nameArea = UIManager.instance.informationText;
        //spawn = UIManager.instance.spawn;
    }
    // set the information menu
    protected virtual void OnMouseDown()
    {

        //information.gameObject.SetActive(true);
        //information.sprite = splash;
        //nameArea.text = buildingName;
        ItemSelection.HandleItemHover(ItemDefinition);
    }
    // color when cursor is over the building
    protected void OnMouseEnter()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        ItemSelection.HandleItemHover(ItemDefinition);
        
    }
    // reset color when cursor exits
    protected void OnMouseExit()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }
    // flash warning
    protected IEnumerator Warning(SpriteRenderer spriteRenderer)
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
}
