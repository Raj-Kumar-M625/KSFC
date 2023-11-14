using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EpicCrmWebApi
{
    public class FilterCondition
    {
        public string ColumnName { get; set; }
        public string ColumnDataType { get; set; }
        public string Operator { get; set; }
        public string ConditionText { get; set; }

        public string ConditionRenderText()
        {
            if (String.IsNullOrEmpty(ConditionText))
            {
                return "";
            }

            if (ColumnDataType == "N")
            {
                return $"{ConditionText}";
            }

            if (ColumnDataType == "D")
            {
                return $"'{ConditionText}'";
            }

            if (Operator == OperatorType.EQ.ToString())
            {
                return $"'%{ConditionText}%'";
            }
            else
            {
                return $"'{ConditionText}'";
            }
        }

        public string OpertorRenderText()
        {
            if (Operator == OperatorType.EQ.ToString())
            {
                if (ColumnDataType == "S")
                {
                    return "like";
                }
                else
                {
                    return "=";
                }
            }

            if(Operator == OperatorType.LE.ToString())
            {
                return "<=";
            }

            if(Operator == OperatorType.LT.ToString())
            {
                return "<";
            }

            if(Operator == OperatorType.GE.ToString())
            {
                return ">=";
            }
            if(Operator == OperatorType.GT.ToString())
            {
                return ">";
            }
            if(Operator == OperatorType.NE.ToString())
            {
                return "!=";
            }

            return "";
        }
    }

    public class TableDataQueryModel : IValidatableObject
    {
        public string TableName { get; set; }
        public IEnumerable<string> SelectColumns { get; set; }
        public IEnumerable<FilterCondition> FilterSet { get; set; }
        public bool ShowQuery { get; set; }
        public bool GetCountOnly { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

        public string GetTranslatedSelectQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Select ");

            StringBuilder sb2 = new StringBuilder();
            foreach(var c in SelectColumns)
            {
                sb2.Append($"[{c}],");
            }

            sb.AppendLine(sb2.ToString().Trim(new char[] { ',' }));

            sb.AppendLine($" FROM {TableName}");

            AddPredicates(sb);

            return sb.ToString();
        }

        public string GetTranslatedCountQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"SELECT COUNT(*) FROM {TableName}");

            AddPredicates(sb);

            return sb.ToString();
        }

        private void AddPredicates(StringBuilder sb)
        {
            int i = 0;

            Regex regex = new Regex(@"^['%0-9a-zA-Z_\- ]{1,20}$");

            if ((FilterSet?.Count() ?? 0) > 0)
            {
                sb.AppendLine(" WHERE ");
                foreach (var fs in FilterSet)
                {
                    string t = fs.ConditionRenderText();
                    if (String.IsNullOrEmpty(t) || regex.IsMatch(t) == false)
                    {
                        continue;
                    }

                    i++;

                    if (i > 1)
                    {
                        sb.Append(" AND ");
                    }

                    sb.AppendLine($"[{fs.ColumnName}] {fs.OpertorRenderText()} {t}");
                }
            }
        }
    }
}