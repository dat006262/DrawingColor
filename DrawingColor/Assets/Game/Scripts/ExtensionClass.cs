using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ExtensionClass
{
    public static Vector2 ReadVector2FormCORFile(string path)
    {
      
            if (File.Exists(path))
            {
                string   content = File.ReadAllText(path);
                
                string[] numbers = content.Split(' ');
                if (numbers.Length == 2)
                {
                     return new Vector2(float.Parse(numbers[0]), float.Parse(numbers[1]));
                }
                else
                {
                    return Vector2.zero;
                }
                Debug.Log(content);
            }
            else
            {
                Debug.LogError("File does not exist at path: " + path);
                return Vector2.zero;
            }
    }
}
