using System;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace H3.BizBus
{
    /// <summary>
    /// 数据，包括：文本、数值、数组等类型，的帮助类
    /// </summary>
    public class Utility
    {

        #region 获得默认值

        public static object GetDefaultValue(System.Type Type)
        {
            if (Type == typeof(string))
            {
                return null;
            }
            else if (Type == typeof(System.DateTime))
            {
                return System.DateTime.MinValue;
            }
            else if (Type == typeof(System.Boolean))
            {
                return false;
            }
            else if (
                Type == typeof(double) ||
                Type == typeof(float))
            {
                return 0.0;
            }
            else if (Type == typeof(System.Int64) ||
                Type == typeof(System.Int32) ||
                Type == typeof(byte) ||
                Type == typeof(Int16) ||
                Type == typeof(short) ||
                Type == typeof(long) ||
                Type == typeof(ushort) ||
                Type == typeof(ulong) ||
                Type == typeof(uint) ||
                Type == typeof(sbyte) ||
                Type == typeof(decimal))
            {
                return 0;
            }
            else if (Type == typeof(System.TimeSpan))
            {
                return new System.TimeSpan(0);
            }
            else if (Type == typeof(System.Data.DataTable))
            {
                return null;
            }
            else if (Type == typeof(System.Data.SqlTypes.SqlDateTime))
            {
                return System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            else if (Type == typeof(Guid))
            {
                return null;
            }
            else
            {
                return null;
            }
        }

        // 获得某种类型的默认值的字符串形式
        public static string GetDefaultStringValue(System.Type Type)
        {
            object defaultValue;
            if (Type == typeof(bool))
            {
                return "false";
            }
            else if ((defaultValue = GetDefaultValue(Type)) == null)
            {
                return "";
            }
            else
            {
                return defaultValue.ToString();
            }
        }

        #endregion

        #region 强制转换

        public static T Convert<T>(object Source)
        {
            return (T)Convert(Source, typeof(T));
        }

        public static T Convert<T>(object Source, bool ignoreException)
        {
            try
            {
                return (T)Convert(Source, typeof(T));
            }
            catch (Exception ex)
            {
                if (ignoreException)
                {
                    return default(T);
                }
                else
                {
                    throw ex;
                }
            }
        }

        public static object Convert(object Source, System.Type ConversionType)
        {
            return Convert(Source, ConversionType, false, null);
        }

        public static bool Convert(object Source, System.Type ConversionType, out object Result)
        {
            Result = null;
            try
            {
                Result = Convert(Source, ConversionType, false, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static object Convert(object source, System.Type conversionType, bool ignoreException, object defaultValue)
        {
            if (conversionType == typeof(object))
            {
                return source;
            }
            else if (source != null && source.GetType() == conversionType)
            {
                return source;
            }
            else if (source == null || source == DBNull.Value || source == "")
            {
                return defaultValue;
            }
            else if (source.GetType() == conversionType)
            {
                return source;
            }
            try
            {
                if (conversionType == typeof(System.DateTime))
                {
                    if (source == "0000-00-00")
                    {
                        return new System.DateTime();
                    }
                    else if (source == null || source == DBNull.Value)
                    {
                        return DateTime.MinValue;
                    }
                    else
                    {
                        return System.DateTime.Parse(source.ToString());
                    }
                }
                else if (conversionType == typeof(System.TimeSpan))
                {
                    return System.TimeSpan.Parse(source.ToString());
                }
                else if (conversionType == typeof(Guid))
                {
                    if (source is byte[])
                    {
                        return new Guid((byte[])source);
                    }
                    else if (string.IsNullOrEmpty(source.ToString()))
                    {
                        return null;
                    }
                    else
                    {
                        return new Guid(source.ToString());
                    }
                }
                else if (conversionType == typeof(bool))
                {
                    if (source == null ||
                        (source is int && (int)source == 0) ||
                        (source is string && (((string)source) == "0" || ((string)source).ToLower() == "false")) ||
                        (source.ToString() == "0" || source.ToString().ToLower() == "false"))
                    {
                        return false;
                    }
                    else if (source != null &&
                        (source is int && (int)source == 1) ||
                        (source is string && (((string)source) == "1" || ((string)source).ToLower() == "true")) ||
                        (source.ToString() == "1" || source.ToString().ToLower() == "true"))
                    {
                        return true;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("无法将数据\"{0}\"转换为bool类型", source));
                    }
                }
                else if (conversionType == typeof(char))
                {
                    if (source == null || source == "")
                    {
                        return (char)0;
                    }
                    else if (source is string)
                    {
                        string s = (string)source;
                        if (s.Length > 1)
                        {
                            throw new InvalidCastException(string.Format("无法将数据\"{0}\"转换为char类型", source));
                        }
                        else
                        {
                            return s[0];
                        }
                    }
                    else
                    {
                        return (char)source;
                    }
                }
                else if (conversionType == typeof(char[]))
                {
                    if (source == null)
                    {
                        return null;
                    }
                    else if (source is string)
                    {
                        return ((string)source).ToCharArray();
                    }
                    else if (source is System.Array)
                    {
                        System.Array sourceArray = (System.Array)source;
                        char[] result = new char[sourceArray.Length];
                        for (int count = 0; count < sourceArray.Length; count++)
                        {
                            result[count] = (char)Convert(sourceArray.GetValue(count), typeof(char));
                        }
                        return result;
                    }
                    else
                    {
                        return System.Convert.ChangeType(source, conversionType);
                    }
                }
                else if (conversionType == typeof(byte))
                {
                    if (source == null)
                    {
                        return 0;
                    }
                    else if (source is string)
                    {
                        return byte.Parse((string)source);
                    }
                    else
                    {
                        return (byte)source;
                    }
                }
                else if (conversionType == typeof(short))
                {
                    if (source == null)
                    {
                        return 0;
                    }
                    else if (source is string)
                    {
                        return short.Parse((string)source);
                    }
                    else
                    {
                        return (short)source;
                    }
                }
                else if (conversionType == typeof(string))
                {
                    if (source == null)
                    {
                        return null;
                    }
                    else if (source is char[])
                    {
                        return new string((char[])source);
                    }
                    else
                    {
                        return source.ToString();
                    }
                }
                else if (conversionType.IsEnum)
                {
                    if (source is int)
                    {
                        int nSource = (int)source;
                        return Enum.ToObject(conversionType, nSource);
                    }
                    else
                    {
                        return Enum.Parse(conversionType, source.ToString());
                    }
                }
                else if (conversionType.IsArray)
                {
                    if (source == null)
                    {
                        return null;
                    }
                    else if (source is string && string.IsNullOrEmpty((string)source))
                    {
                        return null;
                    }
                    else if (source is System.Array)
                    {
                        System.Array sourceArray = (System.Array)source;
                        System.Reflection.ConstructorInfo constructor = conversionType.GetConstructor(new Type[] { typeof(int) });
                        System.Array result = (System.Array)constructor.Invoke(new object[] { sourceArray.Length });
                        System.Type elementType = conversionType.GetElementType();
                        for (int count = 0; count < sourceArray.Length; count++)
                        {
                            object sourceElement = sourceArray.GetValue(count);
                            object element = Convert(sourceElement, elementType);
                            result.SetValue(element, count);
                        }
                        return result;
                    }
                    else
                    {
                        System.Reflection.ConstructorInfo constructor = conversionType.GetConstructor(new Type[] { typeof(int) });
                        System.Array result = (System.Array)constructor.Invoke(new object[] { 1 });
                        System.Type elementType = conversionType.GetElementType();
                        object element = Convert(source, elementType);
                        result.SetValue(element, 0);
                        return result;
                    }
                }
                else if (conversionType.FullName[conversionType.FullName.Length - 1] == '&')
                {
                    Type t = Type.GetType(conversionType.FullName.Substring(0, conversionType.FullName.Length - 1));
                    return Convert(source, t, ignoreException, defaultValue);
                }
                else
                {
                    return System.Convert.ChangeType(source, conversionType);
                }
            }
            catch (Exception ex)
            {
                if (!ignoreException)
                {
                    throw ex;
                }
                else
                {
                    return GetDefaultValue(conversionType);
                }
            }
        }

        #endregion

        /// <summary>
        /// 将逻辑类型转换为实际类型
        /// </summary>
        /// <param name="type">逻辑类型</param>
        /// <returns>运行态的类型</returns>
        public static System.Type ToRealType(BizDataType type)
        {
            switch (type)
            {
                case BizDataType.Bool:
                    return typeof(bool);
                case BizDataType.DateTime:
                    return typeof(System.DateTime);
                case BizDataType.TimeSpan:
                    return typeof(System.TimeSpan);
                case BizDataType.Double:
                    return typeof(double);
                case BizDataType.Int:
                    return typeof(int);
                case BizDataType.Long:
                    return typeof(long);
                case BizDataType.UnitArray:
                    return typeof(string[]);
                case BizDataType.ShortString:
                case BizDataType.Map:
                case BizDataType.Address:
                case BizDataType.String:
                case BizDataType.Unit:
                case BizDataType.Html:
                case BizDataType.Xml:
                case BizDataType.Association:
                    return typeof(string);
                case BizDataType.ByteArray:
                    return typeof(byte[]);
                //case BizDataType.File:
                //    return typeof(DataModel.BizObjectFile[]);
                //case BizDataType.BizObject:
                //    return typeof(DataModel.BizObject);
                //case BizDataType.BizObjectArray:
                //    return typeof(DataModel.BizObject[]);
                case BizDataType.BizStructure:
                    return typeof(BizBus.BizStructure);
                case BizDataType.BizStructureArray:
                    return typeof(BizBus.BizStructure[]);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
