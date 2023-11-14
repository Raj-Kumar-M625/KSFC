using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TableSchema
    {
        public int Position { get; set; }
        public string ColumnName { get; set; }
        public int IsNullable { get; set; }
        public string DataType { get; set; }
        public int CharMaxLen { get; set; }
        public string ColumnDefault { get; set; }

        public bool IsStringDataType =>
                "nvarchar".Equals(DataType, StringComparison.OrdinalIgnoreCase);

        public bool IsDecimalDataType =>
            "decimal".Equals(DataType, StringComparison.OrdinalIgnoreCase) ||
            "float".Equals(DataType, StringComparison.OrdinalIgnoreCase);

        public bool IsNumberDataType =>
            "bigint".Equals(DataType, StringComparison.OrdinalIgnoreCase) ||
            "int".Equals(DataType, StringComparison.OrdinalIgnoreCase);

        public bool IsDateDataType =>
            "Date".Equals(DataType, StringComparison.OrdinalIgnoreCase) ||
            "datetime".Equals(DataType, StringComparison.OrdinalIgnoreCase) ||
            "dateTime2".Equals(DataType, StringComparison.OrdinalIgnoreCase);

        public bool IsBooleanDataType =>
            "bit".Equals(DataType, StringComparison.OrdinalIgnoreCase);


        public string DataTypeDisplayName
        {
            get
            {
                string s = DataType;
                if (IsStringDataType)
                {
                    s = $"Text ({CharMaxLen})";
                }
                else if (IsDecimalDataType)
                {
                    s = "9999.99";
                }
                else if (IsNumberDataType)
                {
                    s = "9999";
                }

                return s;
            }
        }
    }
}
