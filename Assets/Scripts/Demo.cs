using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    IEnumerator Start()
    {
        try
        {
            DBUtil.LoadDatabase();
        }
        catch (Exception e)
        {
            resultText.text = e.Message;
        }

        while (!DBUtil.dbLoaded)
            yield return null;

        if (!DBUtil.Verify())
        {
            try
            {
                DBUtil.CreateTable("Number", Tuple.Create("ID", DBUtil.Category.Int), Tuple.Create("Data", DBUtil.Category.Int));
            }
            catch (Exception e)
            {
                resultText.text = e.Message;
            }

            try
            {
                DBUtil.InsertData("Number", Tuple.Create("ID", "1"), Tuple.Create("Data", "123"));
                DBUtil.InsertData("Number", Tuple.Create("ID", "2"), Tuple.Create("Data", "456"));
                DBUtil.InsertData("Number", Tuple.Create("ID", "3"), Tuple.Create("Data", "789"));
                DBUtil.InsertData("Number", Tuple.Create("ID", "4"), Tuple.Create("Data", "135"));
                DBUtil.InsertData("Number", Tuple.Create("ID", "5"), Tuple.Create("Data", "246"));
            }
            catch (Exception e)
            {
                resultText.text = e.Message;
            }
        }

        resultText.text = DBUtil.Query("Number", true, "ID", "Data");
        yield return new WaitForSeconds(2.5f);
        resultText.text = DBUtil.QueryAllWithFilter("Number", "ID", "3");
    }

    void OnApplicationQuit()
    {
        DBUtil.Terminate();
    }
}