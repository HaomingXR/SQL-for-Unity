using SQL.APIs;
using System.Collections;
using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text resultText;

    void Awake()
    {
        DBUtils.LoadDatabase("Demo.db");
    }

    IEnumerator Start()
    {
        resultText.text = "Loading...";

        while (!DBUtils.dbLoaded)
            yield return null;

        if (!DBUtils.Verify())
        {
            DBUtils.CreateTable("Showcase", new ColumnStruct("ID", "INT"), new ColumnStruct("Data", "VARCHAR(256)"));

            DBUtils.InsertData("Showcase", new ColumnStruct("ID", "1"), new ColumnStruct("Data", DBUtils.ToStr("Alpha")));
            DBUtils.InsertData("Showcase", new ColumnStruct("ID", "2"), new ColumnStruct("Data", DBUtils.ToStr("Beta")));
            DBUtils.InsertData("Showcase", new ColumnStruct("ID", "3"), new ColumnStruct("Data", DBUtils.ToStr("Gamma")));
        }
        else
            DBUtils.ModifyData("Showcase", new ColumnStruct("Data", DBUtils.ToStr("Gamma")), new ColumnStruct("ID", "3"));

        resultText.text = string.Join(", ", DBUtils.Query("Showcase", "ID", "Data"));

        yield return new WaitForSeconds(2.0f);

        DBUtils.ModifyData("Showcase", new ColumnStruct("Data", DBUtils.ToStr("Theta")), new ColumnStruct("ID", "3"));
        resultText.text = string.Join(", ", DBUtils.Query("Showcase", "ID", "Data"));

        yield return new WaitForSeconds(2.0f);

        resultText.text = string.Join(", ", DBUtils.Query("Showcase", "Data"));

        yield return new WaitForSeconds(2.0f);

        resultText.text = string.Join(", ", DBUtils.QueryAllWithFilter("Showcase", "ID", "1"));
    }

    void OnApplicationQuit()
    {
        DBUtils.Terminate();
    }
}