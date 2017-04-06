using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIServerItemContainer : MonoBehaviour {

    private const int c_ItemNum = 2;
    public UIServerItem[] itemArr = new UIServerItem[c_ItemNum];

    public void SetServerItem(int index, int serverId, string serverName, string state)
    {
        if (index < itemArr.Length && itemArr[index] != null)
        {
            itemArr[index].SetServerInfo(serverId, serverName, state);
        }
    }

    public void HideServerItem()
    {
        if (itemArr.Length > 1 && itemArr[1] != null)
            NGUITools.SetActive(itemArr[1].gameObject, false);
    }
}
