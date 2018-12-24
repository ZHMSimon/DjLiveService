//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using OASystem.Model.DtoModel.WebDto.module_action;
//using UtilTools.DbFactory.DbHelper;

//namespace UtilTools.DbFactory
//{
//    public class SqlHelperFactory
//    {
//        public enum DbType
//        {
//            SqlServer,
//            MySql,
//        }
//        private static IdbHelperInterface mysqlHelper = new MySqlDbHelper();
//        private static IdbHelperInterface sqlHelper = new SqlServerDbHelper();
//        public static IdbHelperInterface GetDbHelper(DbType dbType)
//        {
//            IdbHelperInterface helper = null;
//            switch (dbType)
//            {
//                case DbType.SqlServer:
//                    {
//                        helper = sqlHelper;
//                        break;
//                    }
//                case DbType.MySql:
//                    {
//                        helper = mysqlHelper;
//                        break;
//                    }
//                default:
//                    {
//                        break;
//                    }
//            }
//            return helper;
//        }
//    }
//    /// <summary>
//    /// int型 默认值不为0 才可使用以下方法
//    /// </summary>
//    public class DbUtil
//    {
//        public static T Conver2Model<T>(DataRow dataRow)
//        {
//            var properties = typeof(T).GetProperties();
//            T domain = System.Activator.CreateInstance<T>();
//            foreach (var property in properties)
//            {
//                try
//                {
//                    var value = dataRow[property.Name];
//                    if (value != DBNull.Value)
//                    {
//                        property.SetValue(domain, value);
//                    }
//                    else
//                    {
//                        property.SetValue(domain, null);
//                    }
//                }
//                catch (ArgumentException e)
//                {
//                    continue;
//                }

//            }
//            return domain;
//        }

//        public static List<T> Convert2DetailModel<T>(DataTable dtTable)
//        {
//            List<T> models = new List<T>();
//            var type = typeof(T);

//            foreach (DataRow row in dtTable.Rows)
//            {
//                var model = Activator.CreateInstance<T>();
//                foreach (PropertyInfo property in type.GetProperties())
//                {
//                    try
//                    {
//                        if (property.PropertyType.IsValueType || property.PropertyType == typeof(String))
//                        {
//                            var obj = row[property.Name];
//                            property.SetValue(model, obj, null);
//                        }
//                        else
//                        {
//                            var obj = JsonConvert.DeserializeObject(row[property.Name].ToString(), property.PropertyType);
//                            property.SetValue(model, obj, null);
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        LogHelper.Info("AccountConvert Warning, Warning Message:" + e.Message);

//                    }
//                }
//                models.Add(model);
//            }
//            return models;
//        }

//        public static string BuildWhereClauseByModel<T>(T model, out string[] nameArray, out object[] paramArray, string prefix = "arg")
//        {
//            StringBuilder sb = new StringBuilder("where 1=1 ");
//            if (model == null)
//            {
//                nameArray = new string[0];
//                paramArray = new object[0];
//                return sb.ToString();
//            }
//            var namelist = new List<string>();
//            var paramlist = new List<object>();
//            int index = 0;
//            var type = typeof(T);
//            foreach (PropertyInfo property in type.GetProperties())
//            {
//                dynamic obj = property.GetValue(model, null);
//                if (obj == null) continue;
//                if (property.PropertyType.IsValueType)
//                {
//                    if (obj is bool || (long)obj != 0)
//                    {
//                        var name = $@"{prefix}{index.ToString()}";
//                        namelist.Add(name);
//                        paramlist.Add(obj);
//                        index++;
//                        sb.Append($@"and {property.Name}=@{name}");
//                    }
//                }
//                else if (property.PropertyType == typeof(String))
//                {
//                    if (!string.IsNullOrWhiteSpace(obj.ToString()))
//                    {
//                        var name = $@"{prefix}{index.ToString()}";
//                        namelist.Add(name);
//                        paramlist.Add(obj);
//                        index++;
//                        sb.Append($@"and {property.Name}=@{name}");
//                    }
//                }
//                //if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
//                //{

//                //}
//            }
//            nameArray = namelist.ToArray();
//            paramArray = paramlist.ToArray();
//            return sb.ToString();
//        }

//        public static bool BuildInsertClause<T>(T modal, out string keyClause, out string valueClause, out string[] nameArray, out object[] paramArray, string prefix = "arg")
//        {
//            StringBuilder valuesSb = new StringBuilder();
//            StringBuilder keysSb = new StringBuilder();
//            if (modal == null)
//            {
//                keyClause = "";
//                valueClause = "";
//                nameArray = new string[0];
//                paramArray = new object[0];
//                return false;
//            }
//            var namelist = new List<string>();
//            var paramlist = new List<object>();
//            int index = 0;
//            var type = typeof(T);
//            foreach (PropertyInfo property in type.GetProperties())
//            {
//                var obj = property.GetValue(modal, null);
//                var name = $@"{prefix}{index.ToString()}";
//                namelist.Add(name);
//                paramlist.Add(obj);
//                valuesSb.Append($@",@{name}");
//                keysSb.Append($@",{property.Name}");
//                index++;
//            }
//            nameArray = namelist.ToArray();
//            paramArray = paramlist.ToArray();
//            keysSb = keysSb.Remove(0, 1);
//            keyClause = keysSb.ToString();
//            valuesSb = valuesSb.Remove(0, 1);
//            valueClause = valuesSb.ToString();
//            return true;
//        }

//        public static string BuildInClause<T>(List<T> list, out string[] nameArray, out object[] paramArray,string prefix = "arg")
//        {
//            var paramList = new List<object>();
//            var nameList = new List<string>();
//            StringBuilder sb = new StringBuilder();
//            if (typeof(T).IsValueType)
//            {
//                foreach (T item in list)
//                {
//                    sb.Append($@",{item}");
//                }
//            }
//            else
//            {
//                int index = 0;
//                foreach (T item in list)
//                {
//                    var name = $@"{prefix}{index.ToString()}";
//                    sb.Append($@",@{name}");
//                    nameList.Add(name);
//                    paramList.Add(item);
//                    index ++;
//                }
//            }
//            paramArray = paramList.ToArray();
//            nameArray = nameList.ToArray();
//            sb.Replace(',','(',0, 1);
//            sb.Append(')');
//            return sb.ToString();
//        }
//    }
   
//}
