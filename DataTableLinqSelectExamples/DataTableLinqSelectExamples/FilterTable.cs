using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace DataTableLinqSelectExamples
{
    enum ConditionalFilter
    {
        Equal,
        Less,
        Greater,
        LessOrEqual,
        GreaterOrEqual,
        Contains,
        Different
    }

    internal class FilterTable
    {
        private DataTable TableScope;
        public DataTable TableSchema => TableScope.Clone();

        public void Load(DataTable dataTable)
        {
            TableScope = dataTable;
        }

        public DataTable SearchByText(string field, string text)
        {
            // >>>
            // see https://learn.microsoft.com/pt-br/dotnet/framework/data/adonet/creating-a-datatable-from-a-query-linq-to-dataset
            // <<<
            IEnumerable<DataRow> query =
                from order in TableScope.AsEnumerable()
                where order.Field<string>(field).Contains(text)
                select order;

            if (query.ToArray().Length > 0)
            {
                // Create a table from the query.
                DataTable boundTable = query.CopyToDataTable<DataRow>();
                return boundTable;
            }
            else
            {
                return TableSchema;
            }
        }

        public static DataTable Filter<T>(DataTable dataTable, string field, T value, ConditionalFilter filter)
        {
            // >>>
            // see https://learn.microsoft.com/pt-br/dotnet/framework/data/adonet/creating-a-datatable-from-a-query-linq-to-dataset
            // <<<
            IEnumerable<DataRow> query =
                from order in dataTable.AsEnumerable()
                where
                // Case Contains (only string)
                    filter == ConditionalFilter.Contains ? 
                        order.Field<string>(field).ToUpper().Contains(value.ToString().ToUpper()) :

                // Case Equal
                    filter==ConditionalFilter.Equal ? 
                        order.Field<T>(field).Equals(value) :

                // Case less than (only DateTime and numbers)
                    filter == ConditionalFilter.Less ?
                        (typeof(T)==typeof(DateTime) ? 
                            order.Field<DateTime>(field) < DateTime.Parse(value.ToString()) : 
                            order.Field<decimal>(field) < decimal.Parse(value.ToString())) :

                // Case greater than (only DateTime and numbers)
                    filter == ConditionalFilter.Greater ?
                        (typeof(T) == typeof(DateTime) ? 
                            order.Field<DateTime>(field) > DateTime.Parse(value.ToString()) : 
                            order.Field<decimal>(field) > decimal.Parse(value.ToString())) :

                // Case less or equal than (only DateTime and numbers)
                    filter == ConditionalFilter.LessOrEqual ?
                        (typeof(T) == typeof(DateTime) ? 
                            order.Field<DateTime>(field) <= DateTime.Parse(value.ToString()) : 
                            order.Field<decimal>(field) <= decimal.Parse(value.ToString())) :

                // Case less or equal than (only DateTime and numbers)
                    filter == ConditionalFilter.GreaterOrEqual ?
                        (typeof(T) == typeof(DateTime) ? 
                            order.Field<DateTime>(field) >= DateTime.Parse(value.ToString()) : 
                            order.Field<decimal>(field) >= decimal.Parse(value.ToString())) :

                // Case different than
                    !order.Field<T>(field).Equals(value)

                select order;

            if (query.ToArray().Length > 0)
            {
                // Create a table from the query.
                DataTable boundTable = query.CopyToDataTable();
                return boundTable;
            }
            else
            {
                return dataTable.Clone();
            }
        }
    }
}
