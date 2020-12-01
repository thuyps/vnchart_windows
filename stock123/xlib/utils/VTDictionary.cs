using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using System.Drawing.Text;
using Newtonsoft.Json;
using System.IO;
using xlib.utils;

namespace xlib.utils
{
    public class VTDictionary
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        ArrayList rootArray;

        public bool isArray = false;

        public VTDictionary()
        {
        }

        static public void test()
        {
            String json = "{"

   + "'TEST_Long':1500222333444,"
      + "'TEST_Double':1500222333444.12,"
      + "'TEST_Int':1500,"
      + "'TEST_Float':44.12,"
   + "'CPU': 'Intel',"
   + "'PSU': '500W',"
   + "'Price':1500222333444,"
   + "'Computer': {'Type': 1, 'Price': 2000.5, 'Name': 'Dell 6600', 'IsLaptop': true},"
   + "'Drives': ["
   +  "'DVD read/writer'"
   +  "/*(broken)*/,"
   +  "'500 gigabyte hard drive',"
   +  "'200 gigabyte hard drive'"
   +"]"
+"}";
            VTDictionary d = new VTDictionary(json);
            String s = d.toJson();

            System.Console.Out.Write(s);
        }

        public VTDictionary(String json)
        {
            try
            {
                JsonReader reader = new JsonTextReader(new StringReader(json));
                reader.Read();

                if (reader.TokenType == JsonToken.StartObject)
                {
                    jsonReadDictionary(reader, this);
                }
                if (reader.TokenType == JsonToken.StartArray)
                {
                    rootArray = new ArrayList();
                    isArray = true;
                    jsonReadArray(reader, rootArray);
                }
            }
            catch (Exception e)
            {
                System.Console.Out.Write(e.ToString());
            }
        }

        void jsonReadDictionary(JsonReader reader, VTDictionary dict)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    String name = reader.Value.ToString();
                    reader.Read();

                    //  object can be: dictionary, array, or primitive types

                    //  array
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        ArrayList arr = new ArrayList();
                        jsonReadArray(reader, arr);

                        dict.setValue(name, arr);
                    }
                    //  dictionary
                    else if (reader.TokenType == JsonToken.StartObject){
                        VTDictionary d = new VTDictionary();
                        jsonReadDictionary(reader, d);

                        setValue(name, d);
                    }
                    else if (reader.TokenType == JsonToken.Comment)
                    {
                        continue;
                    }
                    //  primitive types
                    else if (reader.TokenType == JsonToken.String)
                    {
                        dict.setValueString(name, reader.Value.ToString());
                    }
                    else if (reader.TokenType == JsonToken.Float)
                    {
                        //  double
                        dict.setValue(name, reader.Value);
                    }
                    else if (reader.TokenType == JsonToken.Integer)
                    {
                        //  long
                        dict.setValue(name, reader.Value);
                    }
                    else if (reader.TokenType == JsonToken.Boolean)
                    {
                        dict.setValueBool(name, (bool)reader.Value);
                    }
                    else
                    {
                        dict.setValue(name, reader.Value.ToString());
                    }
                }
            }
        }

        void jsonReadArray(JsonReader reader, ArrayList arr)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    VTDictionary d = new VTDictionary();
                    jsonReadDictionary(reader, d);

                    arr.Add(d);
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    break;
                }
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    ArrayList arrSub = new ArrayList();
                    jsonReadArray(reader, arrSub);
                    arr.Add(arrSub);
                }
                else if (reader.TokenType == JsonToken.Comment)
                {
                    continue;
                }
                else if (reader.TokenType == JsonToken.Float)
                {
                    //  double
                    arr.Add(reader.Value);
                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    //  long
                    arr.Add(reader.Value);
                }
                else if (reader.TokenType == JsonToken.Boolean)
                {
                    arr.Add(reader.Value);
                }
                else
                {
                    arr.Add(reader.Value.ToString());
                }
            }
        }

        public void setValueString(String k, String s)
        {
            dict[k] = s;
        }
        public void setValueInt(String k, int v)
        {
            dict[k] = v;
        }
        public void setValueLong(String k, long v)
        {
            dict[k] = v;
        }
        public void setValueFloat(String k, float v)
        {
            dict[k] = v;
        }
        public void setValueDouble(String k, double v)
        {
            dict[k] = v;
        }
        public void setValueBool(String k, bool v)
        {
            dict[k] = v;
        }
        public void setValue(String k, object o)
        {
            dict[k] = o;
        }

        public bool hasKey(String key)
        {
            return dict.ContainsKey(key);
        }

        public int getValueInt(String key)
        {
            if (hasKey(key))
            {
                Type _type = dict[key].GetType();
                if (_type.Equals(typeof(int)) || _type.Equals(typeof(long))
                    || _type.Equals(typeof(float)) || _type.Equals(typeof(double)))
                {
                    return (int)Convert.ToInt32(dict[key]);
                }
                else if (_type.Equals(typeof(bool)))
                {
                    return Convert.ToBoolean(dict[key]) ? 1 : 0;
                }
                else if (_type.Equals(typeof(String)))
                {
                    return Utils.stringToInt((String)dict[key]);
                }
            }

            return 0;
        }
        public long getValueLong(String key)
        {
            if (hasKey(key))
            {
                Type _type = dict[key].GetType();
                if (_type.Equals(typeof(int)) || _type.Equals(typeof(long))
                    || _type.Equals(typeof(float)) || _type.Equals(typeof(double)))
                {
                    return (long)Convert.ToInt64(dict[key]);
                }
                else if (_type.Equals(typeof(bool)))
                {
                    return Convert.ToBoolean(dict[key]) ? 1 : 0;
                }
                else if (_type.Equals(typeof(String)))
                {
                    return (long)Utils.stringToInt((String)dict[key]);
                }
            }

            return 0;
        }
        public double getValueDouble(String key)
        {
            if (hasKey(key))
            {
                Type _type = dict[key].GetType();
                if (_type.Equals(typeof(int)) || _type.Equals(typeof(long))
                    || _type.Equals(typeof(float)) || _type.Equals(typeof(double)))
                {
                    return (double)Convert.ToDouble(dict[key]);
                }
                else if (_type.Equals(typeof(bool)))
                {
                    return Convert.ToBoolean(dict[key]) ? 1 : 0;
                }
                else if (_type.Equals(typeof(String)))
                {
                    return Utils.stringToFloat((String)dict[key]);
                }
            }

            return 0;
        }
        public float getValueFloat(String key)
        {
            if (hasKey(key))
            {
                Type _type = dict[key].GetType();
                if (_type.Equals(typeof(int)) || _type.Equals(typeof(long))
                    || _type.Equals(typeof(float)) || _type.Equals(typeof(double)))
                {
                    return (float)Convert.ToDouble(dict[key]);
                }
                else if (_type.Equals(typeof(bool)))
                {
                    return Convert.ToBoolean(dict[key]) ? 1 : 0;
                }
                else if (_type.Equals(typeof(String)))
                {
                    return Utils.stringToFloat((String)dict[key]);
                }
            }

            return 0;
        }

        public String getValueString(String key)
        {
            if (hasKey(key))
            {
                if (dict[key].GetType().Equals(typeof(String)))
                {
                    return (String)dict[key];
                }
                else
                {
                    return dict[key].ToString();
                }
            }

            return null;
        }

        public ArrayList getArray(String key)
        {
            if (hasKey(key))
            {
                Object o = getValue(key);
                if (o.GetType().Equals(typeof(ArrayList)))
                {
                    return (ArrayList)o;
                }
            }
            return null;
        }

        public object getValue(String key)
        {
            if (hasKey(key))
            {
                return dict[key];
            }
            return null;
        }
        //====================================
        public String toJson()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);
            if (isArray)
            {
                jsonWriteArray(writer, rootArray);
            }
            else
            {
                jsonWriteDictionary(writer, this);
            }

            return sb.ToString();
        }

        void jsonWriteDictionary(JsonWriter writer, VTDictionary dict)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<string, object> entry in dict.dict)
            {
                if (entry.Value.GetType().Equals(typeof(VTDictionary)))
                {
                    writer.WritePropertyName(entry.Key);
                    jsonWriteDictionary(writer, (VTDictionary)entry.Value);
                }
                else if (entry.Value.GetType().Equals(typeof(ArrayList)))
                {
                    writer.WritePropertyName(entry.Key);
                    jsonWriteArray(writer, (ArrayList)entry.Value);
                }
                else
                {
                    writer.WritePropertyName(entry.Key);
                    writer.WriteValue(entry.Value);
                }
            }
            writer.WriteEndObject();
        }
        void jsonWriteArray(JsonWriter writer, ArrayList arr)
        {
            writer.WriteStartArray();
            foreach (Object entry in arr)
            {
                if (entry.GetType().Equals(typeof(VTDictionary)))
                {
                    jsonWriteDictionary(writer, (VTDictionary)entry);
                }
                else if (entry.GetType().Equals(typeof(ArrayList)))
                {
                    jsonWriteArray(writer, (ArrayList)entry);
                }
                else
                {
                    writer.WriteValue(entry);
                }
            }
            writer.WriteEndArray();
        }
    }
}
