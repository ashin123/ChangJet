using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace H3.BizBus
{
    public class BizStructureUtility
    {
        public static string SchemaToJson(BizStructureSchema schema)
        {
            if (schema == null)
            {
                return null;
            }

            if (schema.Code.IndexOf(".") > -1)
            {
                return "编码[" + schema.Code + "]含有特殊字符!";
            }

            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject.Add("Code", schema.Code);
            List<Dictionary<string, object>> itemsObject = new List<Dictionary<string, object>>();
            ItemSchema[] items = schema.Items;
            if (items != null && items.Length != 0)
            {
                foreach (ItemSchema item in items)
                {
                    Dictionary<string, object> itemObject = new Dictionary<string, object>();
                    itemObject.Add("Name", item.Name);
                    itemObject.Add("DisplayName", item.DisplayName);
                    itemObject.Add("DataType", item.DataType.ToString());

                    if (item.DataType == BizDataType.BizStructure || item.DataType == BizDataType.BizStructureArray)
                    {
                        string subSchema = SchemaToJson(item.ChildSchema);
                        if (subSchema != null)
                        {
                            itemObject.Add("ChildSchema", Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(subSchema));
                        }
                    }

                    itemsObject.Add(itemObject);
                }
            }

            jsonObject.Add("Items", itemsObject);
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
        }

        public static bool JsonToSchema(string jsonString, out BizStructureSchema schema, out string errorMessage)
        {
            schema = null;
            errorMessage = null;
            try
            {
                JObject jobject = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
                if (jobject == null)
                {
                    errorMessage = "传入的数据位空";
                    return false;
                }
                return JObjectToSchema(jobject, out schema, out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }

        private static bool JObjectToSchema(JObject jobject, out BizStructureSchema schema, out string errorMessage)
        {
            errorMessage = null;
            schema = new BizStructureSchema();
            foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
            {
                if (keyValuePair.Value == null)
                {
                    continue;
                }
                if (string.Compare(keyValuePair.Key, "code", true) == 0)
                {
                    schema.Code = keyValuePair.Value.ToString();
                    continue;
                }

                if (!(keyValuePair.Value is JArray))
                {
                    errorMessage = keyValuePair.Value.ToString() + "不合法";
                    return false;
                }
                JArray itemObject = (JArray)keyValuePair.Value;
                string name = keyValuePair.Key;
                string displayName = null;
                string strDataType = null;
                BizDataType dataType = BizDataType.ShortString;
                BizStructureSchema childSchema = null;
                foreach (JObject childObject in itemObject)
                {
                    foreach (KeyValuePair<string, JToken> itemKeyValuePair in childObject)
                    {
                        if (string.Compare(itemKeyValuePair.Key, "name", true) == 0)
                        {
                            name = itemKeyValuePair.Value.ToString();
                        }
                        else if (string.Compare(itemKeyValuePair.Key, "displayname", true) == 0)
                        {
                            displayName = itemKeyValuePair.Value.ToString();
                        }
                        else if (string.Compare(itemKeyValuePair.Key, "datatype", true) == 0)
                        {
                            strDataType = itemKeyValuePair.Value.ToString();
                            try
                            {
                                dataType = (BizDataType)Enum.Parse(typeof(BizDataType), strDataType, true);
                            }
                            catch
                            {
                                errorMessage = string.Format("字段类型{0}不合法", strDataType);
                                return false;
                            }
                        }
                        else if (string.Compare(itemKeyValuePair.Key, "childschema", true) == 0)
                        {
                            if (itemKeyValuePair.Value != null && !string.IsNullOrEmpty(itemKeyValuePair.Value.ToString()))
                            {
                                if (!(itemKeyValuePair.Value is JObject))
                                {
                                    errorMessage = string.Format("字段{0}不合法", itemKeyValuePair.Value.ToString());
                                    return false;
                                }
                                JObject childJObject = (JObject)itemKeyValuePair.Value;
                                if (!JObjectToSchema(childJObject, out childSchema, out errorMessage))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = name;
                    }
                    ItemSchema itemSchema = new ItemSchema(name, displayName, dataType, childSchema);

                    schema.Add(itemSchema);
                }
            }
            return true;
        }

        public static bool JsonToStructures(string[] jsonStrings, BizStructure[] structures, out string errorMessage)
        {
            structures = null;
            errorMessage = null;
            if (jsonStrings == null)
            {
                return true;
            }
            List<BizStructure> ss = new List<BizStructure>();
            foreach (string s in jsonStrings)
            {
                BizStructure structure = null;
                if (!JsonToStructure(s, structure, out errorMessage))
                {
                    return false;
                }
                ss.Add(structure);
            }
            structures = ss.ToArray();
            return true;
        }

        public static bool JsonToStructure(string jsonString, BizStructure structure, out string errorMessage)
        {
            try
            {
                JObject jobject = JObject.Parse(jsonString);
                if (jobject == null)
                {
                    errorMessage = "传入的数据位空";
                    return false;
                }
                return JObjectToStructure(jobject, structure, out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }

        private static bool JObjectToStructure(JObject jobject, BizStructure obj, out string errorMessage)
        {
            errorMessage = null;

            // 赋值进去
            foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
            {
                ItemSchema property = obj.Schema.GetItem(keyValuePair.Key);
                if (keyValuePair.Value == null)
                {
                    continue;
                }
                else if (property == null)
                {
                    continue;
                }
                else if (property.DataType == BizDataType.BizStructure)
                {
                    BizStructure childObject = null;
                    if (keyValuePair.Value.ToString() != "")
                    {
                        if (!(keyValuePair.Value is JObject))
                        {
                            errorMessage = string.Format("属性{0}是一个结构，但是传过来的Json字符串与该类型不一致", property.Name);
                            return false;
                        }
                        JObject childJObject = (JObject)keyValuePair.Value;
                        BizStructureSchema childSchema = property.ChildSchema;
                        childObject = new BizStructure(childSchema);
                        if (!JObjectToStructure(childJObject, childObject, out errorMessage))
                        {
                            return false;
                        }
                    }
                    obj[property.Name] = childObject;
                }
                else if (property.DataType == BizDataType.BizStructureArray)
                {
                    List<BizStructure> boList = new List<BizStructure>();
                    if (keyValuePair.Value.ToString() != "")
                    {
                        if (!(keyValuePair.Value is JArray))
                        {
                            errorMessage = string.Format("属性{0}是一个业务对象数组，但是传过来的Json字符串与该类型不一致", property.Name);
                            return false;
                        }
                        BizStructureSchema childSchema = property.ChildSchema;
                        if (childSchema == null)
                        {
                            continue;
                        }

                        int count = keyValuePair.Value.Count();
                        if (count == 0)
                        {
                            continue;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            object p_v = keyValuePair.Value[i];
                            if (!(p_v is JObject))
                            {
                                errorMessage = string.Format("属性{0}是一个业务对象数组，但是传过来的Json字符串与该类型不一致", property.Name);
                                return false;
                            }
                            JObject childJObject = (JObject)p_v;
                            BizStructure childBo = new BizStructure(childSchema);
                            if (!JObjectToStructure(childJObject, childBo, out errorMessage))
                            {
                                return false;
                            }
                            boList.Add(childBo);
                        }
                    }
                    obj[keyValuePair.Key] = boList.ToArray();
                }
                else if (property.DataType == BizDataType.UnitArray)
                {
                    List<string> vs = new List<string>();
                    if (keyValuePair.Value.ToString() != "")
                    {
                        if (!(keyValuePair.Value is JArray))
                        {
                            errorMessage = string.Format("属性{0}是一个多人字段，但是传过来的Json字符串与该类型不一致", property.Name);
                            return false;
                        }
                        int count = keyValuePair.Value.Count();
                        for (int i = 0; i < count; i++)
                        {
                            string unit = keyValuePair.Value[i].ToString();
                            vs.Add(unit);
                        }
                    }
                    obj[keyValuePair.Key] = vs.ToArray();
                }
                else if (property.DataType == BizDataType.Unit)
                {
                    obj[keyValuePair.Key] = keyValuePair.Value.ToString();
                }
                else
                {
                    object v = Utility.Convert(keyValuePair.Value.ToString(), property.RealType, true, null);
                    obj[property.Name] = v;
                }
            }

            return true;
        }

        static private void ConvertUnitCodeToIds(List<KeyValuePair<BizStructure, string>> changedProperties, Dictionary<string, string> unitCodeIdTable)
        {
            foreach (KeyValuePair<BizStructure, string> pair in changedProperties)
            {
                BizStructure obj = pair.Key;
                string name = pair.Value;
                ItemSchema p = obj.Schema.GetItem(name);
                object v = obj[p.Name];
                if (v == null)
                {
                    continue;
                }
                if (p.DataType == BizDataType.Unit)
                {
                    if (!unitCodeIdTable.ContainsKey((string)v))
                    {
                        obj[p.Name] = null;
                    }
                    else
                    {
                        obj[p.Name] = unitCodeIdTable[(string)v];
                    }
                }
                else if (p.DataType == BizDataType.UnitArray)
                {
                    string[] vs = (string[])v;
                    for (int i = 0; i < vs.Length; i++)
                    {
                        string _v = vs[i];
                        if (string.IsNullOrEmpty(_v) || !unitCodeIdTable.ContainsKey(_v))
                        {
                            vs[i] = null;
                        }
                        else
                        {
                            vs[i] = unitCodeIdTable[_v];
                        }
                    }
                    obj[p.Name] = vs;
                }
            }
        }

        public static string StructureToJson(BizStructure structure)
        {
            if (structure == null)
            {
                return null;
            }
            System.Text.StringBuilder jsonString = new System.Text.StringBuilder();
            BizStructureSchema schema = structure.Schema;
            ItemSchema[] properties = schema.Items;
            jsonString.Append("{");
            foreach (ItemSchema p in properties)
            {
                object v = structure[p.Name];
                if (v == null)
                {
                    continue;
                }

                jsonString.Append("\"" + p.Name + "\":");
                if (p.DataType == BizDataType.UnitArray)
                {
                    string[] ids = (string[])v;
                    jsonString.Append("[");
                    if (ids.Length > 0)
                    {
                        foreach (string code in ids)
                        {
                            jsonString.Append("\"" + code + "\",");
                        }
                        jsonString.Remove(jsonString.Length - 1, 1);
                    }
                    jsonString.Append("],");
                }
                else if (p.DataType == BizDataType.BizStructure)
                {
                    jsonString.Append(StructureToJson((BizStructure)v) + ",");
                }
                else if (p.DataType == BizDataType.BizStructureArray)
                {
                    BizStructure[] children = (BizStructure[])v;
                    jsonString.Append("[");
                    foreach (BizStructure child in children)
                    {
                        jsonString.Append(StructureToJson(child) + ",");
                    }
                    if (jsonString[jsonString.Length - 1] == ',')
                    {
                        jsonString.Remove(jsonString.Length - 1, 1);
                    }
                    jsonString.Append("],");
                }
                else
                {
                    jsonString.Append("\"" + v.ToString() + "\",");
                }
            }
            if (jsonString[jsonString.Length - 1] == ',')
            {
                jsonString.Remove(jsonString.Length - 1, 1);
            }
            jsonString.Append("}");
            return jsonString.ToString();
        }

        public static string FilterToJson(Filter filter)
        {
            Dictionary<string, object> json = new Dictionary<string, object>();
            json.Add("FromRowNum", filter.FromRowNum);
            json.Add("RequireCount", filter.RequireCount);
            if (filter.ReturnColumns != null)
            {
                json.Add("ReturnColumns", filter.ReturnColumns);
            }
            if (filter.ReturnItems != null)
            {
                json.Add("ReturnItems", filter.ReturnItems);
            }
            json.Add("SortByCollection", filter.SortByCollection);
            json.Add("ToRowNum", filter.ToRowNum);
            json.Add("Matcher", MatcherToJson(filter.Matcher));

            return Newtonsoft.Json.JsonConvert.SerializeObject(json);
        }

        private static Dictionary<string, object> MatcherToJson(Matcher matcher)
        {
            Dictionary<string, object> json = new Dictionary<string, object>();
            if (matcher == null)
            {
                return json;
            }

            if (matcher is ItemMatcher)
            {
                ItemMatcher i = (ItemMatcher)matcher;
                json.Add("Type", "Item");
                json.Add("Name", i.ItemName);
                json.Add("Operator", i.ComparisonOperator);
                json.Add("Value", i.Value == null ? "" : i.Value.ToString());
                return json;
            }
            else
            {
                Matcher[] matchers = null;
                List<Dictionary<string, object>> matcherArray = new List<Dictionary<string, object>>();
                if (matcher is And)
                {
                    And and = (And)matcher;
                    json.Add("Type", "And");
                    matchers = and.Matchers;
                }
                else
                {
                    Or or = (Or)matcher;
                    json.Add("Type", "Or");
                    matchers = or.Matchers;
                }

                if (matchers != null)
                {
                    foreach (Matcher m in matchers)
                    {
                        matcherArray.Add(MatcherToJson(m));
                    }
                    json.Add("Matchers", matcherArray);
                }
                return json;
            }
        }

        private static Matcher JsonToMatcher(string matcherStr)
        {
            Dictionary<string, object> json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(matcherStr);
            if (json["Type"].ToString() == "Item")
            {
                ItemMatcher i = new ItemMatcher(
                    json["Name"].ToString(),
                    (ComparisonOperatorType)Enum.Parse(typeof(ComparisonOperatorType), json["Operator"].ToString()),
                    json["Value"]);
                return i;
            }
            else if (json["Type"].ToString() == "And")
            {
                And and = new And();
                if (json.ContainsKey("Matchers"))
                {
                    Newtonsoft.Json.Linq.JArray matchers = (Newtonsoft.Json.Linq.JArray)json["Matchers"];
                    for (int i = 0; i < matchers.Count; i++)
                    {
                        and.Add(JsonToMatcher(matchers[i].ToString()));
                    }
                }
                return and;
            }
            else
            {
                Or or = new Or();
                if (json.ContainsKey("Matchers"))
                {
                    Newtonsoft.Json.Linq.JArray matchers = (Newtonsoft.Json.Linq.JArray)json["Matchers"];
                    for (int i = 0; i < matchers.Count; i++)
                    {
                        or.Add(JsonToMatcher(matchers[i].ToString()));
                    }
                }
                return or;
            }

        }

        public static Filter JsonToFilter(string filterStr)
        {
            Filter filter = new Filter();
            Dictionary<string, object> jsonFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(filterStr);

            filter.FromRowNum = int.Parse(jsonFilter["FromRowNum"].ToString());
            filter.ToRowNum = int.Parse(jsonFilter["ToRowNum"].ToString());
            filter.RequireCount = bool.TrueString == jsonFilter["RequireCount"].ToString();
            if (jsonFilter.ContainsKey("ReturnColumns"))
            {
                filter.ReturnColumns = Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnColumn[]>(jsonFilter["ReturnColumns"].ToString());
            }

            if (jsonFilter.ContainsKey("ReturnItems"))
            {
                filter.ReturnItems = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(jsonFilter["ReturnItems"].ToString());
            }

            filter.SortByCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<SortBy[]>(jsonFilter["SortByCollection"].ToString());
            filter.Matcher = JsonToMatcher(jsonFilter["Matcher"].ToString());
            return filter;
        }

        public static string InvokeResultToJson(InvokeResult result)
        {
            if (result == null)
            {
                return null;
            }
            string jsonSchema = SchemaToJson(result.Schema);
            if (string.IsNullOrEmpty(jsonSchema))
            {
                jsonSchema = "\"\"";
            }
            string jsonData = StructureToJson(result.Data);
            if (string.IsNullOrEmpty(jsonData))
            {
                jsonData = "\"\"";
            }
            return "{\"ResultCode\":\"" + result.Code + "\", \"Message\":\"" + result.Message + "\", \"Schema\":" + jsonSchema + ", \"Data\":" + jsonData + "}";
        }

        public static bool JsonToInvokeResult(string json, out InvokeResult result, out string errorMessage)
        {
            errorMessage = null;
            result = null;
            try
            {
                JObject jobject = Newtonsoft.Json.Linq.JObject.Parse(json);
                if (jobject == null)
                {
                    errorMessage = "传入的数据位空";
                    return false;
                }
                int resultCode = (int)ErrorCode.GeneralFailed;
                string message = null;
                H3.BizBus.BizStructure structure = null;
                H3.BizBus.BizStructureSchema schema = null;

                foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
                {
                    if (string.Compare(keyValuePair.Key, "schema", true) == 0)
                    {
                        if (keyValuePair.Value == null || keyValuePair.Value.ToString() == "")
                        {
                        }
                        else if (!(keyValuePair.Value is JObject))
                        {
                            errorMessage = "传入的数据格式不正确";
                        }
                        else
                        {
                            if (!JObjectToSchema((JObject)keyValuePair.Value, out schema, out errorMessage))
                            {
                                return false;
                            }
                        }
                    }
                }

                foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
                {
                    if (string.Compare(keyValuePair.Key, "resultcode", true) == 0)
                    {
                        string s = keyValuePair.Value == null ? null : keyValuePair.Value.ToString();
                        int.TryParse(s, out resultCode);
                    }
                    else if (string.Compare(keyValuePair.Key, "message", true) == 0)
                    {
                        message = keyValuePair.Value == null ? null : keyValuePair.Value.ToString();
                    }
                    else if (string.Compare(keyValuePair.Key, "data", true) == 0)
                    {
                        if (keyValuePair.Value == null || keyValuePair.Value.ToString() == "")
                        {
                        }
                        else if (!(keyValuePair.Value is JObject))
                        {
                            errorMessage = "传入的数据格式不正确";
                        }
                        else if (schema == null)
                        {
                            errorMessage = "传入的数据没有包含Schema属性";
                            return false;
                        }
                        else
                        {
                            structure = new BizStructure(schema);
                            if (!JObjectToStructure((JObject)keyValuePair.Value, structure, out errorMessage))
                            {
                                return false;
                            }
                        }
                    }
                }
                result = new InvokeResult(resultCode, message, structure);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }

        public static string ListResultToJson(ListResult result)
        {
            if (result == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            if (result.Data != null && result.Data.Length != 0)
            {
                foreach (BizStructure s in result.Data)
                {
                    builder.Append(StructureToJson(s) + ", ");
                }
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return "{\"Code\":\"" + result.Code + "\", \"Message\":\"" + result.Message + "\", \"Schema\":" + SchemaToJson(result.Schema) + ", \"Count\":" + result.Count + ", \"Data\":[" + builder.ToString() + "]}";
        }

        public static bool JsonToListResult(string json, out ListResult result, out string errorMessage)
        {
            errorMessage = null;
            result = null;
            try
            {
                JObject jobject = Newtonsoft.Json.Linq.JObject.Parse(json);
                if (jobject == null)
                {
                    errorMessage = "传入的数据位空";
                    return false;
                }
                int resultCode = (int)ErrorCode.GeneralFailed;
                string message = null;
                H3.BizBus.BizStructure[] structures = null;
                H3.BizBus.BizStructureSchema schema = null;
                int count = -1;

                foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
                {
                    if (string.Compare(keyValuePair.Key, "schema", true) == 0)
                    {
                        if (keyValuePair.Value == null)
                        {
                        }
                        else if (!(keyValuePair.Value is JObject))
                        {
                            errorMessage = "传入的数据格式不正确";
                        }
                        else
                        {
                            if (!JObjectToSchema((JObject)keyValuePair.Value, out schema, out errorMessage))
                            {
                                return false;
                            }
                        }
                    }
                }
                if (schema == null)
                {
                    errorMessage = "传入的数据没有包含Schema属性";
                    return false;
                }

                foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
                {
                    if (string.Compare(keyValuePair.Key, "code", true) == 0)
                    {
                        string s = keyValuePair.Value == null ? null : keyValuePair.Value.ToString();
                        int.TryParse(s, out resultCode);
                    }
                    else if (string.Compare(keyValuePair.Key, "count", true) == 0)
                    {
                        string s = keyValuePair.Value == null ? null : keyValuePair.Value.ToString();
                        int.TryParse(s, out count);
                    }
                    else if (string.Compare(keyValuePair.Key, "message", true) == 0)
                    {
                        message = keyValuePair.Value == null ? null : keyValuePair.Value.ToString();
                    }
                    else if (string.Compare(keyValuePair.Key, "data", true) == 0)
                    {
                        if (keyValuePair.Value == null)
                        {
                        }
                        else if (!(keyValuePair.Value is JArray))
                        {
                            errorMessage = "传入的数据格式不正确";
                        }
                        else
                        {
                            structures = new BizStructure[keyValuePair.Value.Count()];
                            int i = 0;
                            foreach (JObject o in keyValuePair.Value)
                            {
                                structures[i] = new BizStructure(schema);
                                if (!JObjectToStructure(o, structures[i], out errorMessage))
                                {
                                    return false;
                                }
                                i++;
                            }
                        }
                    }
                }
                result = new ListResult(resultCode, message, structures, count);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }
    }
}
