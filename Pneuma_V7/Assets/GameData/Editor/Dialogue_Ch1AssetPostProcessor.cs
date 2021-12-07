using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class Dialogue_Ch1AssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/GameData/Excel/Pneuma.xlsx";
    private static readonly string assetFilePath = "Assets/GameData/Excel/Dialogue_Ch1.asset";
    private static readonly string sheetName = "Dialogue_Ch1";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            Dialogue_Ch1 data = (Dialogue_Ch1)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Dialogue_Ch1));
            if (data == null) {
                data = ScriptableObject.CreateInstance<Dialogue_Ch1> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<Dialogue_Ch1Data>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<DialogueData>().ToArray();
                data.dataList = query.Deserialize<DialogueData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
