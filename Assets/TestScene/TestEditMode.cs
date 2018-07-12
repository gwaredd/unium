using gw.gql;
using gw.proto.utils;
using gw.unium;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestEditMode : MonoBehaviour
{
    public static string ExecuteGQL(string queryString)
    {
        var query = new Query(queryString, Unium.Root).Select();
        return JsonReflector.Reflect(query.Execute());
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TestEditMode))]
public class TestEditModeInspector : Editor
{
    private string myQuery = "/scene/";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        myQuery = EditorGUILayout.TextField("Custom GQL", myQuery);
        if (GUILayout.Button("Execute"))
        {
            LogGQLQuery(myQuery);
        }

        if (GUILayout.Button("Samples"))
        {
            LogSampleGQLQueries();
        }
    }

    private void LogGQLQuery(string queryString)
    {
        Debug.LogFormat("Query <color=orange>{0}</color>\n{1}", queryString, TestEditMode.ExecuteGQL(queryString));
    }

    private void LogSampleGQLQueries()
    {
        string[] sampleQueries = {"/scene/*.name", "/scene//*.Canvas.name", "/scene//[tag='Pickup']"};
        foreach (var query in sampleQueries)
        {
            LogGQLQuery(query);
        }
    }
}
#endif