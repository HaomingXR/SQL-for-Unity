using HaomingSQL;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    void Awake()
    {
        DBUtil.LoadDatabase("Demo.db");
    }

    IEnumerator Start()
    {
        resultText.text = "Loading...";
        while (!DBUtil.dbLoaded)
            yield return null;

        if (!DBUtil.Verify())
        {
            DBUtil.CreateTable("Showcase", new ColumnStruct("ID", "INT"), new ColumnStruct("Data", "VARCHAR(256)"));

            DBUtil.InsertData("Showcase", new ColumnStruct("ID", "1"), new ColumnStruct("Data", DBUtil.ToStr("Alpha")));
            DBUtil.InsertData("Showcase", new ColumnStruct("ID", "2"), new ColumnStruct("Data", DBUtil.ToStr("Beta")));
            DBUtil.InsertData("Showcase", new ColumnStruct("ID", "3"), new ColumnStruct("Data", DBUtil.ToStr("Gamma")));
        }
        else
            DBUtil.ModifyData("Showcase", new ColumnStruct("Data", DBUtil.ToStr("Gamma")), new ColumnStruct("ID", "3"));

        resultText.text = string.Join(", ", DBUtil.Query("Showcase", "ID", "Data"));

        yield return new WaitForSeconds(2.0f);

        DBUtil.ModifyData("Showcase", new ColumnStruct("Data", DBUtil.ToStr("Theta")), new ColumnStruct("ID", "3"));
        resultText.text = string.Join(", ", DBUtil.Query("Showcase", "ID", "Data"));

        yield return new WaitForSeconds(2.0f);

        resultText.text = string.Join(", ", DBUtil.Query("Showcase", "Data"));

        yield return new WaitForSeconds(2.0f);

        resultText.text = string.Join(", ", DBUtil.QueryAllWithFilter("Showcase", "ID", "1"));
    }

    void OnApplicationQuit()
    {
        DBUtil.Terminate();
    }
}