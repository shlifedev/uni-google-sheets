using System.Collections.Generic;
using System.Linq;
using UnityEditor;


public class DefineSymbolManager
{
    /// <summary>
    /// 디파인 심볼 추가하기
    /// </summary>
    /// <param name="symbols"></param>
    public static void AddDefineSymbols(params string[] symbols)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        if (allDefines.Count == 0)
        {
            if (!string.IsNullOrEmpty(definesString))
                allDefines.Add(definesString);
        }
        allDefines.AddRange(symbols.Except(allDefines));

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
        EditorUserBuildSettings.selectedBuildTargetGroup,
        string.Join(";", allDefines.ToArray()));
    }
    /// <summary>
    /// 사용확인
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    public static bool IsUsed(string symbol)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        var result = allDefines.Any(x => x == symbol);
        return result;
    }
    /// <summary>
    /// 지우기
    /// </summary>
    /// <param name="symbols"></param>
    public static void RemoveDefineSymbol(params string[] symbols)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        List<string> newDefines = new List<string>();
        if (allDefines.Count == 0)
        {
            if (!string.IsNullOrEmpty(definesString))
                allDefines.Add(definesString);
        }
        foreach (var define in allDefines)
        {

            var exist = symbols.ToList().Any(x => x == define);
            if (!exist)
                newDefines.Add(define);
        }


        PlayerSettings.SetScriptingDefineSymbolsForGroup(
        EditorUserBuildSettings.selectedBuildTargetGroup,
        string.Join(";", newDefines.ToArray()));
    }
}