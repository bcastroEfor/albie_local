using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ActioBP.General
{
    public static class EnumerationExtension
    {
        public static string GetDescription(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? value.ToString();
        }

    }

    public class EnumG
    {
        private static Hashtable _stringValues = new Hashtable();
        private static Hashtable _stringDescriptions = new Hashtable();
        public static Dictionary<int, string> GetListEnums<TModel>() where TModel : struct, IConvertible
        {
            var result = new Dictionary<int, string>();

            var type = typeof(TModel);

            var names = Enum.GetNames(type);
            var values = Enum.GetValues(type);

            for (int i = 0; i < names.Length; i++)
            {
                int valInt = Convert.ToInt16(values.GetValue(i));
                Enum valEnum = (Enum)Enum.ToObject(typeof(TModel), valInt);
                var description = EnumerationExtension.GetDescription(valEnum);
                result.Add(valInt, description);
            }
            return result;
        }

        public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : struct, IConvertible
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        public static string GetDescription(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            if (_stringDescriptions.ContainsKey(value))
                output = (_stringDescriptions[value] as DescriptionAttribute).Description;
            else
            {
                //Look for our 'StringValueAttribute' in the field's custom attributes
                FieldInfo fi = type.GetField(value.ToString());
                DescriptionAttribute[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attrs.Length > 0 && !_stringValues.ContainsKey(value)) //Siempre da error, parece que es tan lenta la funcion anterior que otro proceso ya lo ha metido... //ERJLA // probar try o catch?
                {
                    _stringDescriptions.Add(value, attrs[0]);
                    output = attrs[0].Description;
                }

            }
            return output;
        }

        public static string GetDescription<EnumType>(String enumValue)
        {
            return GetDescription(typeof(EnumType), enumValue);
        }

        public static string GetDescription(Type type, string stringValue, bool ignoreCase = false)
        {
            string enumStringValue = null;
            string outputDescription = null;

            if (!type.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                DescriptionAttribute[] attrsDescription = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                DefaultValueAttribute[] attrs = fi.GetCustomAttributes(typeof(DefaultValueAttribute), false) as DefaultValueAttribute[];
                if (attrs.Length > 0 && attrsDescription.Length > 0)
                {
                    enumStringValue = attrs[0].Value.ToString();
                }

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    outputDescription = attrsDescription[0].Description;
                    break;
                }
            }

            return outputDescription;
        }

        public static string ConverTo(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            if (_stringValues.ContainsKey(value))
                output = (_stringValues[value] as DefaultValueAttribute).Value.ToString();
            else
            {
                //Look for our 'StringValueAttribute' in the field's custom attributes
                FieldInfo fi = type.GetField(value.ToString());
                DefaultValueAttribute[] attrs = fi.GetCustomAttributes(typeof(DefaultValueAttribute), false) as DefaultValueAttribute[];
                if (attrs.Length > 0 && !_stringValues.ContainsKey(value)) //Siempre da error, parece que es tan lenta la funcion anterior que otro proceso ya lo ha metido... //ERJLA
                {
                    _stringValues.Add(value, attrs[0]);
                    output = attrs[0].Value.ToString();
                }

            }
            return output;

        }

        public static EnumType ConverTo<EnumType>(String enumValue)
        {
            return (EnumType)ConverTo(typeof(EnumType), enumValue);
        }
        
        public static object ConverTo(Type type, string stringValue, bool ignoreCase = false)
        {
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                DefaultValueAttribute[] attrs = fi.GetCustomAttributes(typeof(DefaultValueAttribute), false) as DefaultValueAttribute[];
                if (attrs.Length > 0)
                    enumStringValue = attrs[0].Value.ToString();

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return output;
        }
    }
}

namespace System
{
    public static class StringValueExtension
    {

        public static string ToStringVal(this Enum value)
        {
            return ActioBP.General.EnumG.ConverTo(value);
        }

        public static T ToEnum<T>(this string value)
        {
            return ActioBP.General.EnumG.ConverTo<T>(value);
        }

        public static string GetDescEnum(this Enum value)
        {
            return ActioBP.General.EnumG.GetDescription(value);
        }

        public static string GetDescEnum(this string value, Type type, bool ignoreCase = false)
        {
            return ActioBP.General.EnumG.GetDescription(type, value, ignoreCase);
        }
    }
}