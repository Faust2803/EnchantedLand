namespace Common.Scripts.GoogleSheets
{
   using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

public static class CsvParser
{
    public static List<T> Parse<T>(string csv) where T : new()
    {
        var result = new List<T>();

        if (string.IsNullOrWhiteSpace(csv))
            return result;

        List<List<string>> rows = ReadRows(csv);

        if (rows.Count < 2)
            return result;

        var headers = rows[0];

        var members = typeof(T).GetMembers(
            BindingFlags.Public | BindingFlags.Instance);

        var mappings = new Dictionary<int, MemberInfo>();

        for (int i = 0; i < headers.Count; i++)
        {
            string header = headers[i].Trim();

            foreach (var member in members)
            {
                if (member is PropertyInfo property &&
                    property.CanWrite &&
                    property.Name.Equals(
                        header, StringComparison.OrdinalIgnoreCase))
                {
                    mappings[i] = property;
                    break;
                }

                if (member is FieldInfo field &&
                    !field.IsInitOnly &&
                    field.Name.Equals(
                        header, StringComparison.OrdinalIgnoreCase))
                {
                    mappings[i] = field;
                    break;
                }
            }
        }

        for (int rowIndex = 1; rowIndex < rows.Count; rowIndex++)
        {
            var columns = rows[rowIndex];

            if (columns.Count == 0)
                continue;

            var item = new T();

            foreach (var mapping in mappings)
            {
                int columnIndex = mapping.Key;

                if (columnIndex >= columns.Count)
                    continue;

                string value = columns[columnIndex].Trim();

                if (string.IsNullOrEmpty(value))
                    continue;

                MemberInfo member = mapping.Value;

                Type memberType = member switch
                {
                    PropertyInfo p => p.PropertyType,
                    FieldInfo f => f.FieldType,
                    _ => typeof(string)
                };

                object convertedValue = ConvertValue(value, memberType);

                switch (member)
                {
                    case PropertyInfo property:
                        property.SetValue(item, convertedValue);
                        break;

                    case FieldInfo field:
                        field.SetValue(item, convertedValue);
                        break;
                }
            }

            result.Add(item);
        }

        return result;
    }

    private static object ConvertValue(string value, Type type)
    {
        Type actualType = Nullable.GetUnderlyingType(type) ?? type;

        if (actualType == typeof(string))
            return value;

        if (actualType.IsEnum)
            return Enum.Parse(actualType, value, true);

        return Convert.ChangeType(
            value,
            actualType,
            CultureInfo.InvariantCulture);
    }

    private static List<List<string>> ReadRows(string csv)
    {
        var rows = new List<List<string>>();
        var row = new List<string>();
        var cell = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < csv.Length; i++)
        {
            char c = csv[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < csv.Length &&
                    csv[i + 1] == '"')
                {
                    cell.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                row.Add(cell.ToString());
                cell.Clear();
            }
            else if ((c == '\r' || c == '\n') && !inQuotes)
            {
                if (c == '\r' && i + 1 < csv.Length &&
                    csv[i + 1] == '\n')
                    i++;

                row.Add(cell.ToString());
                cell.Clear();

                if (row.Exists(x => !string.IsNullOrWhiteSpace(x)))
                    rows.Add(row);

                row = new List<string>();
            }
            else
            {
                cell.Append(c);
            }
        }

        if (cell.Length > 0 || row.Count > 0)
        {
            row.Add(cell.ToString());
            rows.Add(row);
        }

        return rows;
    }
}
}