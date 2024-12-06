using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class CAD_KnowledgeBaseUtils
{
    public static List<string> GetBooleanMembers()
    {
        Type type = typeof(CAD_KnowledgeBase);
        List<string> memberNames = new List<string>();

        // Get all boolean properties
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(bool));
        memberNames.AddRange(properties.Select(p => p.Name));

        return memberNames;
    }
}
