using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DicomObjects;

namespace BiopticPowerPathDicomServer
{
    internal static class Utils_SQL
    {
        internal static void AddResultItem(DicomDataSet DataSet, DicomDataSet request, int group, int element, object v)
        {
            // Only send items which have been requested
            if (request[group, element].Exists)
            {
                if (v == null)
                    v = "";
                DataSet.Add(group, element, v);
            }
        }

        internal static void AddCondition(ref string query, DicomAttribute condition, string dbname)
        {
            object[] values;
            if ((condition.Exists) && (condition.Value != null))
            {
                if (condition.IsMultiple)
                {
                    query = query + " AND ( 1=0 ";
                    values = condition.Value as object[];
                    query = values.Aggregate(query, (current, value) => current + " OR " + dbname + " = '" + value.ToString() + "'");
                    query = query + " )";
                }
                else
                {
                    AddStringCondition(ref query, condition.Value.ToString(), dbname);
                }
            }
        }

        internal static string StarToPercent(string s)
        {
            s = Regex.Replace(s, "\\*", "%");
            return CleanString(s);
        }

        internal static string CleanString(String s)
        {
            return Regex.Replace(s, "'", "''");
        }

        internal static void AddStringCondition(ref string query, string condition, string dbname)
        {
            if (!string.IsNullOrEmpty(condition) && condition != "*")
            {
                if (condition.IndexOf("*", StringComparison.Ordinal) != -1)
                {
                    query = query + " AND " + dbname + " like '" + StarToPercent(condition) + "'";
                }
                else
                {
                    query = query + " AND " + dbname + " = '" + CleanString(condition) + "'";
                }
            }
        }

        internal static void AddSingleDateCondition(ref string query, DateTime condition, string operator1, string dbname)
        {
            //New method to support SQLite - use SQL Server ISO8601 date times to represent UTC
            //Due to changeable support for this and conversion to UTC in SQL versions, lets do any Timeone conversion at data insertion
            //and assume we have UTC times when creating queries.

            //As SQL server does NOT really support ISO8601, cannot use "o" as the timezone marker breaks the date string
            //SQLite supports this format also.
            string utcDate = "'" + condition.ToUniversalTime().ToString("yyyy-MM-ddTHH\\:mm\\:ss.fff") + "'";
            query = query + " AND " + dbname + operator1 + utcDate;

        }

        internal static void AddDateCondition(ref string query, DicomAttribute condition, string dbname)
        {
            if (condition.Exists && !string.IsNullOrEmpty(condition.Value.ToString()) && condition.Value.ToString() != "*")
            {
                if (condition.Value.ToString().IndexOf("-", StringComparison.Ordinal) == -1) // if Single Date
                {
                    DateTime t = condition.DateTimeTo;
                    t = t.Add(new TimeSpan(23, 59, 59));
                    AddSingleDateCondition(ref query, t, "<=", dbname);
                }
                else
                    AddSingleDateCondition(ref query, condition.DateTimeTo, "<=", dbname);

                AddSingleDateCondition(ref query, condition.DateTimeFrom, ">=", dbname);
            }
        }

        internal static void AddDateTimeCondition(ref string query, DicomAttribute DateCondition, DicomAttribute TimeCondition, string dbname)
        {
            if ( !string.IsNullOrEmpty(DateCondition.Value.ToString()) && DateCondition.Value.ToString() != "*" && !string.IsNullOrEmpty(TimeCondition.Value.ToString()) && TimeCondition.Value.ToString() != "*")
            {
                if (DateCondition.Value.ToString().IndexOf("-", StringComparison.Ordinal) == -1) // if Single Date
                {
                    DateTime t = DateCondition.DateTimeTo;
                    DateTime startTime = TimeCondition.DateTimeFrom;
                    DateTime endTime = TimeCondition.DateTimeTo;
                    DateTime startDateTime = t.Add(new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second));
                    DateTime endDateTime = t.Add(new TimeSpan(endTime.Hour, endTime.Minute, endTime.Second));

                    AddSingleDateCondition(ref query, startDateTime, ">=", dbname);
                    AddSingleDateCondition(ref query, endDateTime, "<=", dbname);
                }
                else if (DateCondition.Value.ToString().IndexOf("-", StringComparison.Ordinal) == 0) // -YYYYMMDD format
                {
                    DateTime t = DateCondition.DateTimeTo;
                    DateTime endTime = TimeCondition.DateTimeTo;
                    DateTime endDateTime = t.Add(new TimeSpan(endTime.Hour, endTime.Minute, endTime.Second));
                    AddSingleDateCondition(ref query, endDateTime, "<=", dbname);
                }
                else if (DateCondition.Value.ToString().IndexOf("-", StringComparison.Ordinal) == DateCondition.Value.ToString().Length - 1) // YYYYMMDD- format
                {
                    DateTime t = DateCondition.DateTimeFrom;
                    DateTime startTime = TimeCondition.DateTimeFrom;
                    DateTime startDateTime = t.Add(new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second));
                    AddSingleDateCondition(ref query, startDateTime, ">=", dbname);
                }
                else // YYYYMMDD-YYYYMMDD format
                {
                    DateTime startDate = DateCondition.DateTimeFrom;
                    DateTime endDate = DateCondition.DateTimeTo;
                    DateTime startTime = TimeCondition.DateTimeFrom;
                    DateTime endTime = TimeCondition.DateTimeTo;
                    DateTime startDateTime = startDate.Add(new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second));
                    DateTime endDateTime = endDate.Add(new TimeSpan(endTime.Hour, endTime.Minute, endTime.Second));

                    AddSingleDateCondition(ref query, startDateTime, ">=", dbname);
                    AddSingleDateCondition(ref query, endDateTime, "<=", dbname);
                }
            }
        }

        //Splits patient name into 2 separte strings surname and forename and send then to the addstringcondition subroutine.
        internal static void AddNameCondition(ref string query, DicomAttribute condition, string dbsurname, string dbforename)
        {
            string ptname;
            string surname;
            string forename;
            string[] nameelements;
            string delstr = "^";

            char[] del = delstr.ToCharArray();
            if (condition.Exists)
            {
                ptname = condition.Value + "^^^^";

                if (!string.IsNullOrEmpty(ptname) && ptname != "*")
                {
                    nameelements = ptname.Split(del);
                    surname = (string)nameelements.GetValue(0);
                    forename = (string)nameelements.GetValue(1);
                    AddStringCondition(ref query, surname, dbsurname);
                    AddStringCondition(ref query, forename, dbforename);
                }
            }
        }
    }
}
    
