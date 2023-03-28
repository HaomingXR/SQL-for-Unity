using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    void Awake()
    {
        DBUtil.LoadDatabase("Data.db");
    }

    IEnumerator Start()
    {
        resultText.text = "Loading...";
        while (!DBUtil.dbLoaded)
            yield return null;

        if (!DBUtil.Verify())
        {
            DBUtil.CreateTable("Number", Tuple.Create("ID", "INT"), Tuple.Create("Data", "INT"));

            DBUtil.InsertData("Number", Tuple.Create("ID", "1"), Tuple.Create("Data", "123"));
            DBUtil.InsertData("Number", Tuple.Create("ID", "2"), Tuple.Create("Data", "456"));
            DBUtil.InsertData("Number", Tuple.Create("ID", "3"), Tuple.Create("Data", "789"));
            DBUtil.InsertData("Number", Tuple.Create("ID", "4"), Tuple.Create("Data", "135"));
            DBUtil.InsertData("Number", Tuple.Create("ID", "5"), Tuple.Create("Data", "246"));
            DBUtil.InsertData("Number", Tuple.Create("ID", "1"), Tuple.Create("Data", "87564231"));
        }

        resultText.text = DBUtil.Query("Number", true, "ID", "Data");
        yield return new WaitForSeconds(2.5f);
        resultText.text = DBUtil.Query("Number", false, "Data");
        yield return new WaitForSeconds(2.5f);
        resultText.text = DBUtil.QueryAllWithFilter("Number", "ID", "1");
    }

    void OnApplicationQuit()
    {
        DBUtil.Terminate();
    }
}